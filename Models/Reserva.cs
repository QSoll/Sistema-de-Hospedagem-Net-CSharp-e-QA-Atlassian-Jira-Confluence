using System;
using System.Collections.Generic;
using DesafioProjetoHospedagem.Models;

namespace DesafioProjetoHospedagem.Models
{
    public class Reserva
    {
        public bool Ativa { get; set; } = true;
        public List<Hospede> Hospedes { get; set; } = new();
        public EspacoHospedagem Espaco { get; set; } = null!;
        public int DiasReservados { get; set; }
        public DateTime DataCheckIn { get; set; } = DateTime.Now;

        // Construtor padrão
        public Reserva() { }

        // Construtor com parâmetros
        public Reserva(List<Hospede> hospedes, EspacoHospedagem espaco, int diasReservados)
        {
            Hospedes = hospedes;
            Espaco = espaco;
            DiasReservados = diasReservados;
            DataCheckIn = DateTime.Now;
            Ativa = true;
        }

        // Cadastra os hóspedes na reserva, verificando a capacidade
        public void CadastrarHospedes(List<Hospede> hospedes)
        {
            if (Espaco.Capacidade >= hospedes.Count)
            {
                Hospedes = hospedes;
            }
            else
            {
                throw new InvalidOperationException("A capacidade do espaço é menor que o número de hóspedes.");
            }
        }

        // Define o espaço da reserva
        public void CadastrarEspaco(EspacoHospedagem espaco)
        {
            Espaco = espaco;
        }

        // Retorna a quantidade de hóspedes
        public int ObterQuantidadeHospedes()
        {
            return Hospedes.Count;
        }

        // Calcula o valor total da diária com ou sem desconto
        public decimal CalcularValorDiaria()
        {
            decimal valorSemDesconto = DiasReservados * Espaco.ValorDiaria;

            if (DiasReservados >= 10)
                return valorSemDesconto * 0.90M; // 10% de desconto para estadias longas

            return valorSemDesconto; // valor cheio
        }
    }
}
