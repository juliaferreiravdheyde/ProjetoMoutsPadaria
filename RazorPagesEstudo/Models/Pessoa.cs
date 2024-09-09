namespace RazorPagesEstudo.Models
{
    public abstract class Pessoa
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Endereco { get; set; }
        public string CpfCnpj { get; set; }

        public Pessoa(int id, string nome, string endereco, string cpfCnpj)
        {
            Id = id;
            Nome = nome;
            Endereco = endereco;
            CpfCnpj = cpfCnpj;
        }

        public abstract bool ValidarIdentidade();
       
    }

}
