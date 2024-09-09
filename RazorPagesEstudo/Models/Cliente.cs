namespace RazorPagesEstudo.Models
{
    public class Cliente : Pessoa
    {
        public int PontosFidelidade { get; set; }

        public Cliente()
        {

        }

        public Cliente(int id, string nome, string endereco, string cpfcnpj) : base(id, nome, endereco, cpfcnpj)
        {
            PontosFidelidade = 0;
        }

        public override bool ValidarIdentidade()
        {
            return CpfCnpj.Length == 11 || CpfCnpj.Length == 14;
        }

        public int AddPontoFidelidade(int pontos)
        {
            return PontosFidelidade += pontos;
        }
    }
}
