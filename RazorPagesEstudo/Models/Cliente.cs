namespace RazorPagesEstudo.Models
{
    public class Cliente : Pessoa
    {
        public int PontosFidelidade { get; set; } = 0;
        public override bool ValidarIdentidade()
        {
            return !string.IsNullOrEmpty(CpfCnpj);
        }
    }
}
