using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RazorPagesEstudo.Models
{
    public class Venda
    {
        public int Id { get; set; }
        public List<Produto> Produtos { get; set; }
        public string FormaPagamento { get; set; }
        public decimal Total { get; private set; }
        public int? ClienteID { get; set; }
        public Cliente Cliente { get; set; }

        public Venda()
        {
            Produtos = new List<Produto>();
        }
        public Venda(List<Produto> produtos, string formaPagamento, Cliente cliente = null)
        {
            Produtos = produtos;
            FormaPagamento = formaPagamento;
            Cliente = cliente;
            CalcularTotal();
        }

        private void CalcularTotal()
        {
            Total = Produtos.Sum(p => p.Preco);
        }

        public void GerarCupomFiscal()
        {
            // Método de geração de cupom pode ser adaptado para interface web.
        }
    }
}
