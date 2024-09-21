using Microsoft.Data.SqlClient;
using RazorPagesEstudo.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RazorPagesEstudo.Services
{
    public class ProdutoService
    {
        private readonly string _connectionString;

        public ProdutoService(string connectionString)
        {
            _connectionString = connectionString;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task BaixaEstoqueAsync(Venda venda)
        {
            if (venda.ItensVenda == null || venda.ItensVenda.Count == 0)
            {
                throw new InvalidOperationException("Não é possível registrar uma venda sem itens.");
            }

            using (var connection = GetConnection())
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in venda.ItensVenda)
                        {
                            var produtoId = item.ProdutoId;
                            int quantidadeVenda = item.Quantidade;

                            string checkStockQuery = @"
                                SELECT QuantidadeEstoque
                                FROM Produto
                                WHERE Id = @produtoId";

                            using (var stockCommand = new SqlCommand(checkStockQuery, connection, transaction))
                            {
                                stockCommand.Parameters.AddWithValue("@produtoId", produtoId);

                                var quantidadeEstoque = (int)await stockCommand.ExecuteScalarAsync();

                                if (quantidadeEstoque < quantidadeVenda)
                                {
                                    throw new InvalidOperationException($"Quantidade para o produto '{item.Produto.Nome}' excede o estoque. Quantidade disponível: {quantidadeEstoque}.");
                                }

                                string updateStockQuery = @"
                                    UPDATE Produto
                                    SET QuantidadeEstoque = QuantidadeEstoque - @quantidade
                                    WHERE Id = @produtoId";

                                using (var updateCommand = new SqlCommand(updateStockQuery, connection, transaction))
                                {
                                    updateCommand.Parameters.AddWithValue("@quantidade", quantidadeVenda);
                                    updateCommand.Parameters.AddWithValue("@produtoId", produtoId);
                                    await updateCommand.ExecuteNonQueryAsync();
                                }
                            }
                        }
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public async Task<List<Produto>> GetAvailableProductsAsync()
        {
            var produtos = new List<Produto>();

            using (var connection = GetConnection())
            {
                string query = "SELECT * FROM Produto where QuantidadeEstoque > 0";
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
    }
}
