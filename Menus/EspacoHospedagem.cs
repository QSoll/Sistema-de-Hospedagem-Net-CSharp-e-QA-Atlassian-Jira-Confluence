namespace DesafioProjetoHospedagem.Models;

public class EspacoHospedagem
{
    public EspacoHospedagem(int id, string tipoEspaco, string nomeFantasia, int capacidade, decimal valorDiaria, int quantidadeQuartos, bool possuiSuite, string descricao)
    {
        Id = id;
        TipoEspaco = tipoEspaco;
        NomeFantasia = nomeFantasia;
        Capacidade = capacidade;
        ValorDiaria = valorDiaria;
        QuantidadeQuartos = quantidadeQuartos;
        PossuiSuite = possuiSuite;
        Descricao = descricao;

        string prefixo = tipoEspaco.ToUpper().Replace(" ", "").Substring(0, Math.Min(3, tipoEspaco.Length)).PadRight(3, 'X');
        CodigoInterno = $"{prefixo}{id}";
    }

    public int Id { get; set; }
    public string TipoEspaco { get; set; }
    public string NomeFantasia { get; set; }
    public int Capacidade { get; set; }
    public decimal ValorDiaria { get; set; }
    public int QuantidadeQuartos { get; set; }
    public bool PossuiSuite { get; set; }
    public string Descricao { get; set; }
    public string CodigoInterno { get; set; }
}
