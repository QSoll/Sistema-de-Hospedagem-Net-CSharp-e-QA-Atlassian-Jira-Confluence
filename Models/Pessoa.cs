using System;

namespace DesafioProjetoHospedagem.Models;

public class Pessoa
{

    public DateTime DataCadastro { get; set; } = DateTime.Now;
    public Pessoa() { }

    public Pessoa(string nome)
    {
        Nome = nome;
    }

    public Pessoa(string nome, string sobrenome)
    {
        Nome = nome;
        Sobrenome = sobrenome;
    }

    public string Nome { get; set; } = string.Empty;
    public string Sobrenome { get; set; } = string.Empty;
    public string NomeCompleto => $"{Nome} {Sobrenome}".ToUpper();
}