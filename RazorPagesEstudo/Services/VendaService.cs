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

        public void AddVenda(Venda venda)
        {
            if (venda.Total <= 0)
            {
                throw new InvalidOperationException("O valor total deve ser maior do que zero.");
            }

            _context.Venda.Add(venda);
            _context.SaveChanges();
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
