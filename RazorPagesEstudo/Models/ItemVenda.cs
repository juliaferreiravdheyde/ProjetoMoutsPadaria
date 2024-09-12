namespace RazorPagesEstudo.Models
{
    public class ItemVenda
    {
        public int Id { get; set; }
        public int VendaId { get; set; } // Chave estrangeira para Venda
        public int ProdutoId { get; set; } // Chave estrangeira para Produto
        public int Quantidade { get; set; }
        public decimal PrecoTotal { get; set; } // Será preenchido manualmente com Produto.Preco * Quantidade

        // Propriedade de navegação
        public Produto Produto { get; set; }

        // Parameterless constructor for EF
        public ItemVenda() { }

        // Constructor for manual usage
        public ItemVenda(Produto produto, int quantidade)
        {
            Produto = produto;
            ProdutoId = produto.Id; // Armazena o ID do Produto
            Quantidade = quantidade;
            PrecoTotal = produto.Preco * quantidade;
        }
    }
}
