namespace RazorPagesEstudo.Models
{
    public class ItemVenda
    {
        public int Id { get; set; }
        public int VendaId { get; set; } 
        public int ProdutoId { get; set; } 
        public int Quantidade { get; set; }
        public decimal ValorTotal { get; set; } 

        public Produto Produto { get; set; }
     
        public ItemVenda() { }

        public ItemVenda(Produto produto, int quantidade)
        {
            Produto = produto;
            ProdutoId = produto.Id; 
            Quantidade = quantidade;
            ValorTotal = produto.Preco * quantidade;
        }
    }
}
