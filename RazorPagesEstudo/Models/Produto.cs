namespace RazorPagesEstudo.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public int QuantidadeEstoque { get; set; }

        public Produto(int id, string nome, decimal preco, int quantidadeEstoque)
        {
            Id = id;
            Nome = nome;
            Preco = preco;
            QuantidadeEstoque = quantidadeEstoque;
        }

        public void AtualizarEstoque(int quantidadeVendida)
        {
            if (quantidadeVendida <= QuantidadeEstoque)
            {
                QuantidadeEstoque -= quantidadeVendida;
            }
            else
            {
                throw new Exception("Quantidade vendida excede o estoque disponível.");
            }
        }
    }
}
