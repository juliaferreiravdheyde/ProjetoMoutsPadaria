using Microsoft.Data.SqlClient;
using RazorPagesEstudo.Models;

namespace RazorPagesEstudo.Services
{
    public class ClienteService
    {
        private readonly string _connectionString;

        public ClienteService(string connectionString)
        {
            _connectionString = connectionString;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task<Cliente> GetClienteByCpfCnpjAsync(string cpfCnpj)
        {
            Cliente cliente = null;

            string query = @"
                SELECT c.Id, c.PontosFidelidade
                FROM Cliente c
                INNER JOIN Pessoa p ON c.Id = p.Id
                WHERE p.CpfCnpj = @cpfCnpj";

            using (var connection = GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@cpfCnpj", cpfCnpj);

                try
                {
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
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
                    throw new Exception("Erro ao buscar o cliente: " + ex.Message);
                }
            }

            return cliente;
        }

    }
}
