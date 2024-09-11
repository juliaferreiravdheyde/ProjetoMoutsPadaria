namespace RazorPagesEstudo.Models
{
    public abstract class Pessoa
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Endereco { get; set; }
        public string CpfCnpj { get; set; }

        public abstract bool ValidarIdentidade();
    }
}
