using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using RazorPagesEstudo.Models;
using System.Collections.Generic;
using System.Linq;

namespace RazorPagesEstudo.Models
{
    public class Venda
    {
        public int Id { get; set; }
        public string FormaPagamento { get; set; }
        public int? ClienteId { get; set; }  
        public Cliente Cliente { get; set; } 
        public List<ItemVenda> ItensVenda { get; set; } = new List<ItemVenda>();


        public Venda() { }

        public Venda(List<ItemVenda> itensVenda, string formaPagamento, Cliente cliente = null)
        {
            ItensVenda = itensVenda;
            FormaPagamento = formaPagamento;
            Cliente = cliente;
        }

        public decimal CalcularTotal()
        {
            return ItensVenda.Sum(item => item.ValorTotal);
        }

        public decimal Total
        {
            get
            {
                return ItensVenda.Sum(item => item.ValorTotal);
            }
        }

        public string GerarCupomFiscal()
        {
            var receipt = new System.Text.StringBuilder();
            receipt.AppendLine("********** CUPOM FISCAL **********");
            receipt.AppendLine($"Data: {DateTime.Now}");
            // receipt.AppendLine($"Forma de Pagamento: {FormaPagamento}");
            receipt.AppendLine($"Forma de Pagamento: {EscolherFormaPagamento(FormaPagamento)}");


            if (Cliente != null)
            {
                receipt.AppendLine("------ Dados do Cliente ------");
                receipt.AppendLine($"Nome: {Cliente.Nome}");
                receipt.AppendLine($"CPF: {Cliente.CpfCnpj}");
                receipt.AppendLine($"Pontos Fidelidade: {Cliente.PontosFidelidade}");
            }
            else
            {
               
                receipt.AppendLine("Cliente: Não informado");
            }

            receipt.AppendLine("------------------------------");

            receipt.AppendLine("------ Produtos Comprados ------");
            foreach (var item in ItensVenda)
            {
                receipt.AppendLine($"{item.Produto.Nome} - Qtd: {item.Quantidade} - Preço: {item.ValorTotal:C}");
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
