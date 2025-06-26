using System;
using System.Collections.Generic;
using DesafioProjetoHospedagem.Models;

namespace Menus
{
    public static class MenuPrincipal
    {
        public static void Exibir(
            List<EspacoHospedagem> espacos,
            List<Hospede> hospedes,
            List<Reserva> reservas,
            List<Hospede> hospedesDaReserva,
            Reserva? reservaSelecionada)
        {
            int opcao;

            do
            {
                Console.Clear();
                Console.WriteLine("============================================");
                Console.WriteLine("\n   BEM-VINDO(A) AO SISTEMA DE HOSPEDAGEM");
                Console.WriteLine($"   Data: {DateTime.Now:dd/MM/yyyy} | Hora: {DateTime.Now:HH:mm:ss}");
                Console.WriteLine("============================================");
                Console.WriteLine("\n               MENU PRINCIPAL\n");
                Console.WriteLine("1 - Gerenciar Espaços");
                Console.WriteLine("2 - Gerenciar Hóspedes");
                Console.WriteLine("3 - Gerenciar Reservas");
                Console.WriteLine("4 - Relatórios e Estatísticas");
                Console.WriteLine("5 - Sobre o Sistema");
                Console.WriteLine("0 - Sair");
                Console.Write("\nEscolha uma opção: ");

                if (!int.TryParse(Console.ReadLine(), out opcao))
                    opcao = -1;

                switch (opcao)
                {
                    case 1:
                        MenuEspacos.Exibir(espacos, reservas);
                        break;
                    case 2:
                        MenuHospedes.Exibir(hospedes, reservas);
                        break;
                    case 3:
                        MenuReservas.Exibir(reservas, hospedes, espacos);
                        break;
                    case 4:
                        MenuRelatorios.Exibir(espacos, reservas, hospedes);
                        break;
                    case 5:
                        MostrarSobreSistemas.Exibir();
                        break;
                    case 0:
                        Console.WriteLine("\nSaindo do sistema...");
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\nOpção inválida. Tente novamente.");
                        Console.ResetColor();
                        Console.ReadKey();
                        break;
                }

            } while (opcao != 0);
        }
    }
}
