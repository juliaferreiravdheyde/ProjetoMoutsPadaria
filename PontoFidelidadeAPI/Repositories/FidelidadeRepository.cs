using MySql.Data.MySqlClient;
using PontoFidelidadeAPI.Interfaces;
using PontoFidelidadeAPI.Models;

namespace PontoFidelidadeAPI.Repositories
{
    public class FidelidadeRepository
    {
        string connectionString = "server=localhost;uid=root;pwd=root;database=api-fidelidade-mouts;";
        private MySqlConnection conexao;

        public MySqlConnection Conexao()
        {
            return this.conexao;
        }

        public FidelidadeRepository()
        {
            try
            {
                conexao = new MySqlConnection(connectionString);
                conexao.Open();
                Console.WriteLine("Conectado com sucesso...");
            }
            catch (Exception)
            {
                Console.WriteLine("Erro de conexão");
            }
            finally
            {
                conexao.Close();
            }
        }
        public void Delete(int id)
        {
            string deleteQuery = "DELETE FROM tb_clientes WHERE id = @Id";
            MySqlCommand command = new MySqlCommand(deleteQuery, conexao);
            try
            {
                command.Parameters.AddWithValue("@Id", id);
                conexao.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                conexao.Close();
            }
        }


        public Cliente Get(int id)
        {
            Cliente cliente = null;
            string selectQuery = "SELECT id, nome, cpf, pontos FROM tb_clientes where id = @Id";
            MySqlCommand command = new MySqlCommand(selectQuery, conexao);

            try
            {
                command.Parameters.AddWithValue("@Id", id);
                conexao.Open();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        cliente = new Cliente
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            Nome = reader.GetString(reader.GetOrdinal("nome")),
                            CPF = reader.GetString(reader.GetOrdinal("cpf")),
                            PontosFidelidade = reader.GetInt32(reader.GetOrdinal("pontos")),
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                conexao.Close();
            }


            return cliente;
        }

        public IEnumerable<Cliente> GetAll()
        {
            List<Cliente> pessoas = new List<Cliente>();
            string selectQuery = "SELECT * FROM tb_clientes";
            MySqlCommand command = new MySqlCommand(selectQuery, conexao);
            {
                try
                {
                    conexao.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            pessoas.Add(new Cliente
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                Nome = reader.GetString(reader.GetOrdinal("nome")),
                                CPF = reader.GetString(reader.GetOrdinal("cpf")),
                                PontosFidelidade = reader.GetInt32(reader.GetOrdinal("pontos")),
                            });
                        }
                    }

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    conexao.Close();
                }
            }
            return pessoas;
        }

        public Cliente Save(Cliente entity)
        {
            try
            {
                conexao.Open();
                string insertQuery = "insert into tb_clientes(nome, cpf, pontos) values (@Nome, @CPF, @PontosFidelidade);SELECT LAST_INSERT_ID();";
                MySqlCommand command = new MySqlCommand(insertQuery, conexao);
                {
                    command.Parameters.AddWithValue("@Nome", entity.Nome);
                    command.Parameters.AddWithValue("@CPF", entity.CPF);
                    command.Parameters.AddWithValue("@PontosFidelidade", entity.PontosFidelidade);
                    entity.Id = Convert.ToInt32(command.ExecuteScalar());
                    Console.WriteLine("Dados gravados com sucesso...");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                conexao.Close();
            }
            return entity;
        }

        public void Update(Cliente entity)
        {
            try
            {
                conexao.Open();
                string updateQuery = "UPDATE tb_clientes SET nome = @Nome, cpf = @CPF, pontos = @PontosFidelidade WHERE id = @Id";
                MySqlCommand command = new MySqlCommand(updateQuery, conexao);
                {
                    command.Parameters.AddWithValue("@Nome", entity.Nome);
                    command.Parameters.AddWithValue("@CPF", entity.CPF);
                    command.Parameters.AddWithValue("@Id", entity.Id);
                    command.Parameters.AddWithValue("@PontosFidelidade", entity.PontosFidelidade);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Dados atualizados com sucesso...");
                    }
                    else
                    {
                        Console.WriteLine("Nenhuma pessoa encontrada com o ID fornecido.");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                conexao.Close();
            }
        }
    }
}

