using System;
using System.Linq;
using System.Collections.Generic;
using DesafioProjetoHospedagem.Models;


namespace Menus
{
    public static class MenuRelatorios
    {
        public static void Exibir(List<EspacoHospedagem> espacos, List<Reserva> reservas, List<Hospede> hospedes)
        {
            Console.Clear();
            Console.WriteLine("========================================");
            Console.WriteLine("\n    RELATÓRIO GERAL | HOTEL FICTÍCIO");
            Console.WriteLine($"    Data: {DateTime.Now:dd/MM/yyyy}     | Hora: {DateTime.Now:HH:mm:ss}");
            Console.WriteLine("========================================\n");

            int totalSuites = espacos.Count;
            int suitesOcupadas = reservas.Count(r => r.Ativa);
            int suitesDisponiveis = totalSuites - suitesOcupadas;

            int totalReservas = reservas.Count;
            int reservasAtivas = reservas.Count(r => r.Ativa);
            int reservasEncerradas = reservas.Count(r => !r.Ativa);

            int totalHospedes = hospedes.DistinctBy(h => h.NomeCompleto).Count();

            decimal totalEstimado = reservas.Sum(r => r.CalcularValorDiaria());

            Console.WriteLine($"Suítes cadastradas: {totalSuites}");
            Console.WriteLine($"Suítes disponíveis: {suitesDisponiveis}");
            Console.WriteLine($"Suítes ocupadas: {suitesOcupadas}");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"Total de reservas: {totalReservas}");
            Console.WriteLine($"Ativas: {reservasAtivas}    |  📁 Encerradas: {reservasEncerradas}");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"Total de hóspedes únicos: {totalHospedes}");
            Console.WriteLine($"Valor estimado total das reservas: R$ {totalEstimado:F2}");

            Console.WriteLine("\nPressione qualquer tecla para retornar ao menu...");
            Console.ReadKey();
        }
    }
}
