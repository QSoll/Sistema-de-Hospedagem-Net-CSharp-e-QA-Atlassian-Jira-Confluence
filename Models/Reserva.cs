using System;
using System.Collections.Generic;
using DesafioProjetoHospedagem.Models;
using DesafioProjetoHospedagem.Utils;
using DesafioProjetoHospedagem.Menus;
using DesafioProjetoHospedagem.Controladoras;

namespace DesafioProjetoHospedagem.Models
{
    public class Reserva
    {
        public bool Ativa { get; set; } = true;
        public List<Cliente> Clientes { get; set; } = new();
        public EspacoHospedagem Espaco { get; set; } = null!;
        public int DiasReservados { get; set; }
        public int HorasExtras { get; set; } = 0;
        public DateTime DataCheckIn { get; set; } = DateTime.Now;
        public DateTime? DataCheckOut { get; set; } // opcional, se ainda não foi definido
        public DateTime? DataSaidaPrevista { get; set; }

        public Reserva(List<Cliente> clientes, EspacoHospedagem espaco, int diasReservados)
        {
            Ativa = true;
            Clientes = clientes;
            Espaco = espaco;
            DiasReservados = diasReservados;
            DataCheckIn = DateTime.Now;
            
        }

        public int HorasUltrapassadas
        {
            get
            {
                if (!DataCheckOut.HasValue || !DataSaidaPrevista.HasValue)
                    return 0;

                var atraso = DataCheckOut.Value - DataSaidaPrevista.Value;
                return atraso.TotalHours > 0 ? (int)atraso.TotalHours : 0;
            }
        }

        // Finaliza a reserva e libera o espaço
        public void FinalizarReserva()
        {
            Ativa = false;
            Espaco = null!;
        }

        // Retorna a quantidade de clientes vinculados à reserva
        public int ObterQuantidadeClientes()
        {
            return Clientes.Count;
        }

        // CONSTRUTOR COM PARAMETROS
        
        public void AtualizarClientesDaReserva(List<Cliente> clientes)
        {
            if (Espaco.Capacidade < clientes.Count)
                throw new InvalidOperationException("A capacidade do espaço é menor que o número de hóspedes.");

            Clientes = clientes;
        }

        // Vincula clientes à reserva, validando a capacidade do espaço
        public void VincularClientes(List<Cliente> clientes)
        {
            if (Espaco.Capacidade >= clientes.Count)
            {
                Clientes = clientes;
            }
            else
            {
                throw new InvalidOperationException("A capacidade do espaço é menor que o número de hóspedes.");
            }
        }

        // Vincula um espaço à reserva
        public void VincularEspaco(EspacoHospedagem espaco)
        {
            Espaco = espaco;
        }

        /* //exibir cálculos financeiros da reserva. Como usar:
        // colar onde quer que apareça os clculos financeiros da reserva
        var resumo = reserva.CalcularResumoFinanceiro();
        Console.WriteLine($"Valor sem desconto: R$ {resumo.SemDesconto:N2}");
        Console.WriteLine($"Desconto aplicado: R$ {resumo.Desconto:N2}");
        Console.WriteLine($"Acréscimos por horas extras: R$ {resumo.ACrescimoHoras:N2}");
        Console.WriteLine($"Valor total: R$ {resumo.TotalFinal:N2}");
        */

        // Derivada útil (recalcula se datas forem atualizadas)
        public int TotalDias =>
            DataCheckOut.HasValue ? (DataCheckOut.Value - DataCheckIn).Days : DiasReservados;

        // 💲 Valor bruto (sem descontos/acréscimos)
        public decimal CalcularValorSemDesconto()
        {
            return TotalDias * Espaco.ValorDiaria;
        }

        // Desconto de 10% para 10 dias ou mais
        public decimal CalcularDesconto()
        {
            return TotalDias >= 10 ? CalcularValorSemDesconto() * 0.10M : 0;
        }

        // ➖ Valor com desconto aplicado
        public decimal CalcularValorComDesconto()
        {
            return CalcularValorSemDesconto() - CalcularDesconto();
        }

        // ➕ Acréscimo de R$ 5,00 por hora extra
        public decimal CalcularTaxaAcrescimo()
        {
            return HorasExtras * 5.00M;
        }

        // Valor total final com desconto e acréscimos
        public decimal CalcularValorTotal()
        {
            return CalcularValorComDesconto() + CalcularTaxaAcrescimo();
        }
        // Resumo financeiro agrupado
        public ResumoFinanceiro CalcularResumoFinanceiro()
        {
            var resumo = new ResumoFinanceiro();

            resumo.ValorDiaria = Espaco?.ValorDiaria ?? 0;
            resumo.ValorSemDesconto = DiasReservados * resumo.ValorDiaria;

            // Desconto: 10% se mais de 10 dias
            bool temDesconto = DiasReservados > 10;
            resumo.ValorDoDesconto = temDesconto ? resumo.ValorSemDesconto * 0.10m : 0;
            resumo.ValorComDesconto = resumo.ValorSemDesconto - resumo.ValorDoDesconto;

            // Taxa de acréscimo se reserva estiver finalizada e houve atraso
            resumo.ValorTaxaExtra = 0;
            if (!Ativa && HorasUltrapassadas > 0)
            {
                resumo.ValorTaxaExtra = HorasUltrapassadas * 5.00m;
            }

            resumo.ValorTotalEstimado = resumo.ValorComDesconto + resumo.ValorTaxaExtra;
            return resumo;
        }


    }
}
