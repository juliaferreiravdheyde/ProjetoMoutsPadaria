using Microsoft.Data.SqlClient;
using RazorPagesEstudo.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        public async Task<List<Produto>> GetAvailableProductsAsync()
        {
            var produtos = new List<Produto>();

            using (var connection = GetConnection())
            {
                string query = "SELECT * FROM Produto";
                SqlCommand command = new SqlCommand(query, connection);

                await connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    produtos.Add(new Produto
                    {
                        Id = reader.GetInt32(0),
                        Nome = reader.GetString(1),
                        Preco = reader.GetDecimal(2)
                    });
                }
                connection.Close();
            }

            return produtos;
        }

        public async Task<Produto> GetProdutoByIdAsync(int produtoId)
        {
            Produto produto = null;

            using (var connection = GetConnection())
            {
                string query = "SELECT * FROM Produto WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", produtoId);

                await connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    produto = new Produto
                    {
                        Id = reader.GetInt32(0),
                        Nome = reader.GetString(1),
                        Preco = reader.GetDecimal(2)
                    };
                }
                connection.Close();
            }

            return produto;
        }

        public async Task<Cliente> GetClienteByCpfCnpjAsync(string cpfCnpj)
        {
            Cliente cliente = null;

            string query = @"
                SELECT c.Id, c.PontosFidelidade
                FROM Cliente c
                INNER JOIN Pessoa p ON c.Id = p.Id
                WHERE p.CpfCnpj = @CpfCnpj";

            using (SqlConnection connection = GetConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CpfCnpj", cpfCnpj);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            cliente = new Cliente
                            {
                                Id = reader.GetInt32(0),
                                PontosFidelidade = reader.IsDBNull(1) ? 0 : reader.GetInt32(1)
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao buscar o cliente: " + ex.Message);
                }
            }

            return cliente;
        }
   
        public Venda AddVenda(Venda venda)
        {
            if (venda.ItensVenda == null || venda.ItensVenda.Count == 0)
            {
                throw new InvalidOperationException("Não é possível registrar uma venda sem itens.");
            }

            using (var connection = GetConnection())
            {
                connection.Open();

                foreach (var item in venda.ItensVenda)
                {
                    var produtoId = item.ProdutoId;
                    int quantidadeVenda = item.Quantidade;

                    string checkStockQuery = @"
                            SELECT QuantidadeEstoque
                            FROM produto 
                            WHERE Id = @produtoId";

                    using (var stockCommand = new SqlCommand(checkStockQuery, connection))
                    {
                        stockCommand.Parameters.AddWithValue("@produtoId", produtoId);

                        var quantidadeEstoque = (int)stockCommand.ExecuteScalar();

                        if (quantidadeEstoque < quantidadeVenda)
                        {
                            throw new InvalidOperationException($"Quantidade para o produto '{item.Produto.Nome}' excede o estoque. " +$"Quantidade disponível: {quantidadeEstoque}."
 );
                        }

                        string updateStockQuery = @"
                                UPDATE produto 
                                SET QuantidadeEstoque = QuantidadeEstoque - @quantidade 
                                WHERE Id = @produtoId";

                        using (var updateCommand = new SqlCommand(updateStockQuery, connection))
                        {
                            updateCommand.Parameters.AddWithValue("@quantidade", quantidadeVenda);
                            updateCommand.Parameters.AddWithValue("@produtoId", produtoId);
                            updateCommand.ExecuteNonQuery();
                        }
                    }
                }

                string insertQuery = @"
                        INSERT INTO Venda (FormaPagamento, ValorTotal, ClienteId) 
                        VALUES (@FormaPagamento, @ValorTotal, @ClienteId); 
                        SELECT SCOPE_IDENTITY();";

                using (var insertCommand = new SqlCommand(insertQuery, connection))
                {
                    insertCommand.Parameters.AddWithValue("@FormaPagamento", venda.FormaPagamento);
                    insertCommand.Parameters.AddWithValue("@ValorTotal", venda.ValorTotal);
                    insertCommand.Parameters.AddWithValue("@ClienteId", venda.Cliente?.Id ?? (object)DBNull.Value);

                    try
                    {
                        venda.Id = Convert.ToInt32(insertCommand.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Erro: " + ex.Message);
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
                        Console.WriteLine("Erro: " + ex.Message);
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
                                Console.WriteLine("Erro: " + ex.Message);
                            }
                            finally
                            {
                                connection.Close();
                            }
                        }
                    }
                }

                /* ideia para usar a api de pontos         
                public async Task AtualizarPontosFidelidade(Cliente cliente, List<ItemVenda> itensVenda)
                {
                    if (cliente != null)
                    {
                        int pontosGanhosPorItem = itensVenda.Count;
                        cliente.PontosFidelidade += pontosGanhosPorItem;

                        bool success = await _clienteApiClient.UpdatePontosFidelidadeAsync(cliente);

                        if (!success)
                        {
                            Console.WriteLine("Erro ao atualizar pontos de fidelidade no API.");
                        }
                    }
                }
                */

    }
}
