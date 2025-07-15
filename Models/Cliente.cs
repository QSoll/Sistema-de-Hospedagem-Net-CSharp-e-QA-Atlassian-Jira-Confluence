using System;

namespace DesafioProjetoHospedagem.Models
{
    public class Cliente
    {
        // Propriedades principais
        public string Nome { get; set; } = string.Empty;
        public string Sobrenome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
        public DateTime DataCadastro { get; set; }

        // Propriedade derivada para exibição
        public string NomeCompleto => $"{Nome} {Sobrenome}".ToUpper();

        // Construtor padrão (para inicialização sem dados)
        public Cliente()
        {
            DataCadastro = DateTime.Now;
        }

        // Construtor mínimo com nome obrigatório
        public Cliente(string nome)
        {
            Nome = nome;
            DataCadastro = DateTime.Now;
        }

        // Construtor completo com todos os dados relevantes
        public Cliente(
            string nome,
            string sobrenome,
            string cpf,
            string telefone,
            string email,
            string endereco,
            DateTime dataNascimento)
        {
            Nome = nome;
            Sobrenome = sobrenome;
            Cpf = cpf;
            Telefone = telefone;
            Email = email;
            Endereco = endereco;
            DataNascimento = dataNascimento;
            DataCadastro = DateTime.Now;
        }

        public Cliente(string nome, string cpf, string telefone, string email, string endereco)
        {
            Nome = nome;
            Cpf = cpf;
            Telefone = telefone;
            Email = email;
            Endereco = endereco;
            DataCadastro = DateTime.Now;
        }

    }
}
