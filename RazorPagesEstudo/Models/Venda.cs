namespace RazorPagesEstudo.Models
{
    public class Venda
    {
        public List<Produto> Produtos { get; set; }
        public string FormaPagamento { get; set; }
       // public Cliente Cliente { get; set; }
        public double Total { get; private set; }
    }
}
