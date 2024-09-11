using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RazorPagesEstudo.Models
{
    public class Venda
    {
        public int Id { get; set; }
        public List<ItemVenda> Itens { get; set; } = new List<ItemVenda>();
        public decimal ValorTotal { get; private set; }

        // Chave estrangeira para Cliente
        public int? ClienteId { get; set; }
        public Cliente Cliente { get; set; } // Propriedade de navegação

        public string FormaPagamento { get; set; }

        // Construtor sem parâmetros necessário para o Entity Framework
        public Venda() { }

        // Construtor adicional para uso manual, fora do Entity Framework
        public Venda(Cliente cliente, string formaPagamento)
        {
            Cliente = cliente;
            ClienteId = cliente.Id; // Relaciona o ID do cliente
            FormaPagamento = formaPagamento;
        }

        public void AdicionarItem(ItemVenda item)
        {
            Itens.Add(item);
            ValorTotal += item.PrecoTotal;
        }

        public void FinalizarVenda()
        {
            if (Cliente != null)
            {
                Cliente.PontosFidelidade += (int)(ValorTotal / 10); // 1 ponto por cada dez reais
            }
        }
    }
}
