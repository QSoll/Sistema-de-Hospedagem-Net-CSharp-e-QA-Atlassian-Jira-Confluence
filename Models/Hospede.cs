using System;

namespace DesafioProjetoHospedagem.Models
{
    public class Hospede : Pessoa
    {
        public string Email { get; set; } = string.Empty;

        public Hospede() : base() { }

        public Hospede(string nome, string sobrenome, string email)
            : base(nome, sobrenome)
        {
            Email = email;
        }
    }
}
