using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RazorPagesEstudo.Data;
using RazorPagesEstudo.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RazorPagesEstudo.Services
{
    public class VendaService
    {
        private readonly RazorPagesEstudoContext _context;

        public VendaService(RazorPagesEstudoContext context)
        {
            _context = context;
        }

        string connectionString = "Data Source=(localdb)\\BDPADARIA;Initial Catalog=agenda;Integrated Security = True";
        public Venda AddVenda(Venda venda)
        {
            /*
            var clienteExistenteById = _context.Cliente.FirstOrDefault(c => c.Id == venda.ClienteId);

            if (clienteExistenteById == null)
            {
                throw new InvalidOperationException("Cliente não encontrado.");
            }
            */ 
            
            string insertQuery = "INSERT INTO Venda (FormaPagamento, ValorTotal, ClienteId) VALUES (@FormaPagamento, @ValorTotal, @ClienteId); SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            using (SqlCommand command = new SqlCommand(insertQuery, connection))
            {
                // Set up parameters
                command.Parameters.AddWithValue("@FormaPagamento", venda.FormaPagamento);
                command.Parameters.AddWithValue("@ValorTotal", venda.ValorTotal); 
                command.Parameters.AddWithValue("@ClienteId", venda.Cliente?.Id ?? (object)DBNull.Value); 

                try
                {
                    connection.Open();
                   
                    venda.Id = Convert.ToInt32(command.ExecuteScalar());
                    return venda; 
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro: " + ex.Message);
                }
            }

            return null; 
        }

        public void AtualizarPontosFidelidade(Cliente cliente, List<ItemVenda> itensVenda)
        {
            if (cliente != null)
            {
                int pontosGanhosPorItem = itensVenda.Count; 
                cliente.PontosFidelidade += pontosGanhosPorItem;

                _context.Cliente.Update(cliente);
                _context.SaveChanges();
            }
        }
    }
}
