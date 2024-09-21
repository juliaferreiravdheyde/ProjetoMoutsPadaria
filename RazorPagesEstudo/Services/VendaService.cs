using Microsoft.Data.SqlClient;
using RazorPagesEstudo.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RazorPagesEstudo.Services
{
    public class VendaService
    {
        private readonly string _connectionString;
        private readonly ClienteApiClient _clienteApiClient;

        public VendaService(string connectionString, ClienteApiClient clienteApiClient)
        {
            _connectionString = connectionString;
            _clienteApiClient = clienteApiClient;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task<Venda> AddVendaAsync(Venda venda)
        {
            if (venda.ItensVenda == null || venda.ItensVenda.Count == 0)
            {
                throw new InvalidOperationException("Não é possível registrar uma venda sem itens.");
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var transaction = await connection.BeginTransactionAsync() as SqlTransaction)
                {
                    try
                    {
                        var produtoService = new ProdutoService(_connectionString);
                        await produtoService.BaixaEstoqueAsync(venda);

                        string insertVendaQuery = @"
                                INSERT INTO Venda (FormaPagamento, ValorTotal, ClienteId) 
                                VALUES (@FormaPagamento, @ValorTotal, @ClienteId); 
                                SELECT SCOPE_IDENTITY();";

                        using (var insertCommand = new SqlCommand(insertVendaQuery, connection, transaction))
                        {
                            insertCommand.Parameters.AddWithValue("@FormaPagamento", venda.FormaPagamento);
                            insertCommand.Parameters.AddWithValue("@ValorTotal", venda.ValorTotal);
                            insertCommand.Parameters.AddWithValue("@ClienteId", venda.Cliente?.Id ?? (object)DBNull.Value);

                            venda.Id = Convert.ToInt32(await insertCommand.ExecuteScalarAsync());
                        }

                        await transaction.CommitAsync();
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        throw; 
                    }
                }
            }

            return venda;
        }

        public void AddItensVenda(List<ItemVenda> itensVenda, int vendaId)
        {
            using (var connection = GetConnection())
            {
                foreach (var item in itensVenda)
                {
                    string insertQuery = @"
                        INSERT INTO ItemVenda (ProdutoId, Quantidade, ValorTotal, VendaId) 
                        VALUES (@ProdutoId, @Quantidade, @ValorTotal, @VendaId);";

                    SqlCommand command = new SqlCommand(insertQuery, connection);
                    command.Parameters.AddWithValue("@ProdutoId", item.Produto.Id);
                    command.Parameters.AddWithValue("@Quantidade", item.Quantidade);
                    command.Parameters.AddWithValue("@ValorTotal", item.ValorTotal);
                    command.Parameters.AddWithValue("@VendaId", vendaId);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Erro ao Adicionar Itens da Venda " + ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        public void AtualizarPontosFidelidade(Cliente cliente, List<ItemVenda> itensVenda)
        {
            if (cliente != null)
            {
                int pontosGanhosPorItem = itensVenda.Count;
                cliente.PontosFidelidade += pontosGanhosPorItem;

                using (var connection = GetConnection())
                {
                    string updateQuery = "UPDATE Cliente SET PontosFidelidade = @PontosFidelidade WHERE Id = @ClienteId";
                    SqlCommand command = new SqlCommand(updateQuery, connection);
                    command.Parameters.AddWithValue("@PontosFidelidade", cliente.PontosFidelidade);
                    command.Parameters.AddWithValue("@ClienteId", cliente.Id);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Erro ao Atualizar Pontos Fidelidade " + ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
    }
}
