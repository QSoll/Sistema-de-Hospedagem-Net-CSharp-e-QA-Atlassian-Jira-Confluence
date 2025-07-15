using System;
using System.Collections.Generic;
using DesafioProjetoHospedagem.Models;
using DesafioProjetoHospedagem.Utils;
using DesafioProjetoHospedagem.Controladoras;
using DesafioProjetoHospedagem.Servicos;
using DesafioProjetoHospedagem.Sessao;
using DesafioProjetoHospedagem.Menus;


namespace DesafioProjetoHospedagem.Menus
{
    public static class MenuPrincipal
    {
        public static void ExibirPrincipal()
        {
            while (true)
            {
                Console.Clear();
                CentralSaida.CabecalhoMenus("MENU PRINCIPAL");
                Console.WriteLine("A partir de 10 dias de hospedagem, 10 % de descoonto.");
                Console.WriteLine("Tempo de hospedagem ultrapassado, acrescentar taxa de R$ 5,00 por hora.");
                SessaoAtual.VerificarSessaoGlobal();

                //Console.WriteLine(); // Espaçamento visual

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("1 - Espaços");
                Console.WriteLine("2 - Reservas");
                Console.WriteLine("3 - Clientes");
                Console.WriteLine("4 - Comprovantes de Reserva");
                Console.WriteLine("5 - Relatórios e Estatísticas");
                Console.WriteLine("6 - Sobre o Sistema");
                Console.WriteLine("0 - Encerrar");
                Console.ResetColor();

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Escolha uma opção: ");
                Console.ResetColor();

                string? entrada = Console.ReadLine()?.Trim();
                Console.WriteLine();

                switch (entrada)
                {
                    case "1":
                        NavegacaoAtual.Registrar(() => ExibirPrincipal());
                        MenuEspacos.ExibirMenuEspacos();
                        break;

                    case "2":
                        NavegacaoAtual.Registrar(() => ExibirPrincipal());
                        MenuReservas.ExibirMenuReservas();
                        break;

                    case "3":
                        NavegacaoAtual.Registrar(() => ExibirPrincipal());
                        Reserva? resultadoReserva = MenuClientes.ExibirMenuClientes();
                        if (resultadoReserva != null)
                            SessaoAtual.ReservaSelecionada = resultadoReserva;
                        break;

                    case "4":
                        NavegacaoAtual.Registrar(() => ExibirPrincipal());
                        MenuComprovantes.ExibirMenuComprovantes();
                        break;

                    case "5":
                        NavegacaoAtual.Registrar(() => ExibirPrincipal());
                        MenuRelatorios.Exibir();
                        break;

                    case "6":
                        NavegacaoAtual.Registrar(() => ExibirPrincipal());
                        MostrarSobreSistema.ExibirSobreSistema();
                        break;

                    case "0":
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\nSistema encerrado. Até logo !!!");
                        Console.ResetColor();
                        Environment.Exit(0);
                        return;

                    default:
                        CentralSaida.Exibir(TipoMensagem.Erro, "Ops... Errou. Tente novamente com uma opção válida.");
                        break;
                }
            }
        }
    }
}

