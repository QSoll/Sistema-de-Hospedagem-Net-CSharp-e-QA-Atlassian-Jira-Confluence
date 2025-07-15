namespace DesafioProjetoHospedagem.Models
{
    public class ResumoFinanceiro
    {
        public decimal ValorDiaria { get; set; }
        public decimal ValorSemDesconto { get; set; }
        public decimal ValorDoDesconto { get; set; }
        public decimal ValorComDesconto { get; set; }
        public decimal ValorTaxaExtra { get; set; }
        public decimal ValorTotalEstimado { get; set; }
    }
}
