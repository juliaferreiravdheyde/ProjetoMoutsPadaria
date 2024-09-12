using RazorPagesEstudo.Models;
using System.Collections.Generic;
using System.Linq;

namespace RazorPagesEstudo.Models
{
    public class Venda
    {
        public int Id { get; set; }
        public string FormaPagamento { get; set; }
        public int? ClienteId { get; set; }  // Nullable in case there's no client
        public Cliente Cliente { get; set; } // Navigational property for Cliente
        public List<ItemVenda> ItensVenda { get; set; } = new List<ItemVenda>(); // List of ItemVenda

        public Venda() { }

        public Venda(List<ItemVenda> itensVenda, string formaPagamento, Cliente cliente = null)
        {
            ItensVenda = itensVenda;
            FormaPagamento = formaPagamento;
            Cliente = cliente;
        }
        public decimal CalcularTotal()
        {
            return ItensVenda.Sum(item => item.PrecoTotal);
        }

        public decimal Total
        {
            get
            {
                return ItensVenda.Sum(item => item.PrecoTotal);
            }
        }

        public void GerarCupomFiscal()
        {
            // Logic for printing or displaying the receipt
        }

        public string EscolherFormaPagamento(string input)
        {
            switch (input)
            {
                case "1": return "Dinheiro";
                case "2": return "Cartão";
                case "3": return "Pix";
                default: return "Não especificado";
            }
        }
    }
}
