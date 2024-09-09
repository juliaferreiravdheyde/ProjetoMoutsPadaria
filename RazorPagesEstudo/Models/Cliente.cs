namespace RazorPagesEstudo.Models
{
    public class Cliente
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public int PontoFidelidade { get; set; }

        public int AddPontoFidelidade(int pontos)
        {
            return PontoFidelidade += pontos;
        }
    }
}
