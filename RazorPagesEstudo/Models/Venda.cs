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

        public string GerarCupomFiscal()
        {
            var receipt = new System.Text.StringBuilder();
            receipt.AppendLine("********** CUPOM FISCAL **********");
            receipt.AppendLine($"Data: {DateTime.Now}");
            receipt.AppendLine($"Forma de Pagamento: {FormaPagamento}");

          
            if (Cliente != null)
            {
                receipt.AppendLine("------ Dados do Cliente ------");
                receipt.AppendLine($"Nome: {Cliente.Nome}");
                receipt.AppendLine($"CPF: {Cliente.CpfCnpj}");  
            }
            receipt.AppendLine("------------------------------");

         
            receipt.AppendLine("------ Produtos Comprados ------");
            foreach (var item in ItensVenda)
            {
                receipt.AppendLine($"{item.Produto.Nome} - Qtd: {item.Quantidade} - Preço: {item.PrecoTotal:C}");
            }

            receipt.AppendLine("-------------------------------");
            receipt.AppendLine($"Total: {Total:C}");
            receipt.AppendLine("********************************");

            
            return receipt.ToString();
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
