using DesafioProjetoHospedagem.Models;
using DesafioProjetoHospedagem.Utils;
using DesafioProjetoHospedagem.Menus;
using DesafioProjetoHospedagem.Controladoras;
using System;
using System.Globalization;

namespace DesafioProjetoHospedagem.Models;

public class EspacoHospedagem
{
    //public List<Cliente> Clientes { get; set; } = new List<Cliente>();

    public EspacoHospedagem(
        string tipoEspaco,
        int id,
        string tipoAbreviado,
        string nomeFantasia,
        int capacidade,
        decimal valorDiaria,
        int quantidadeQuartos,
        bool possuiSuite,
        int quantidadeSuites,
        string descricao)
    {
        TipoEspaco = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(tipoEspaco.ToLower());
        TipoEspaco = tipoEspaco.ToUpper();
        Id = id;
        TipoAbreviado = tipoAbreviado.ToUpper();
        // Reduz o tipo para as 3 primeiras letras em caixa alta
        TipoEspaco = tipoEspaco.ToUpper(); // Valor completo
        TipoAbreviado = tipoEspaco.Length >= 3
            ? tipoEspaco.Substring(0, 3).ToUpper()
            : tipoEspaco.ToUpper(); // Versão abreviada

        // Usa o tipo reduzido no código interno               
        CodigoInterno = $"{tipoAbreviado.ToUpper()} {id} - {nomeFantasia}";
        NomeFantasia = nomeFantasia.ToLower();
        Capacidade = capacidade;
        ValorDiaria = valorDiaria;
        QuantidadeQuartos = quantidadeQuartos;
        QuantidadeSuites = quantidadeSuites;
        Descricao = descricao;
        /*TipoEspaco = tipoEspaco;
        CodigoInterno = $"{id} - {tipoEspaco.ToUpper()} {nomeFantasia}";*/


    }


    public string TipoEspaco { get; set; }
    public string TipoAbreviado { get; set; }
    public int Id { get; set; }
    public string NomeFantasia { get; set; }
    public int Capacidade { get; set; }
    public decimal ValorDiaria { get; set; }
    public int QuantidadeQuartos { get; set; }
    public int QuantidadeSuites { get; set; }
    public bool PossuiSuite { get; set; }

    public string Descricao { get; set; }
    public string CodigoInterno { get; set; }
    public string Status { get; set; } = "LIVRE";


}

