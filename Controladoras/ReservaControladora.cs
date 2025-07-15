using DesafioProjetoHospedagem.Utils;
using DesafioProjetoHospedagem.Menus;
using DesafioProjetoHospedagem.Controladoras;
using DesafioProjetoHospedagem.Models;
using System.Globalization;
using System;
using DesafioProjetoHospedagem.Sessao;

namespace DesafioProjetoHospedagem.Controladoras
{
    public class ReservaControladora
    {
        public static string ObterStatusEspaco(EspacoHospedagem espaco)
        {
            //Console.Clear();
            //CentralSaida.CabecalhoMenus("Status do espaço");

            var reservaAtiva = SessaoAtual.Reservas
    .FirstOrDefault(r => r.Ativa && r.Espaco?.Id == espaco.Id);

            string status = reservaAtiva == null ? "LIVRE" : "OCUPADO";

            //Exibe cabeçalho emocional com tempo real
            DateTime agora = DateTime.Now;
            Console.WriteLine($"\nEm {agora:dd/MM/yyyy}, às {agora:HH:mm}, o status desse espaço é:");

            Console.ForegroundColor = status == "LIVRE" ? ConsoleColor.Green : ConsoleColor.Red;
            Console.WriteLine($"Status do espaço: {status}");
            Console.ResetColor();

            //Se houver reserva ativa, exibe detalhes financeiros
            if (reservaAtiva != null)
            {
                var resumo = reservaAtiva.CalcularResumoFinanceiro();

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\nResumo financeiro:");
                Console.ResetColor();

                Console.WriteLine($"Diária: R$ {resumo.ValorDiaria:N2}");
                Console.WriteLine($"Sem desconto: R$ {resumo.ValorSemDesconto:N2}");
                Console.WriteLine($"Desconto: R$ {resumo.ValorDoDesconto:N2}");
                Console.WriteLine($"Extras: R$ {resumo.ValorTaxaExtra:N2}");

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"↳ Total estimado: R$ {resumo.ValorTotalEstimado:N2}");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\nDatas importantes:");
                Console.ResetColor();

                Console.Write("Check-in: ");
                Console.WriteLine($"{reservaAtiva.DataCheckIn:dd/MM/yyyy}");

                Console.Write("Previsão de checkout: ");
                Console.WriteLine($"{reservaAtiva.DataSaidaPrevista:dd/MM/yyyy}");


                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\nHóspedes:");
                Console.ResetColor();

                if (reservaAtiva.Clientes.Any())
                {
                    int i = 1;
                    foreach (var cliente in reservaAtiva.Clientes)
                        Console.WriteLine($"{i++} - {cliente.Nome}");
                }
                else
                {
                    Console.WriteLine("Ainda não cadastrados.");
                }
            }

            Console.WriteLine();

            // Rodapé de navegação
            MenuRodape.ExibirRodape(
                "Cadastro de Espaços",
                aoVoltarParaSubmenu: () => MenuEspacos.ExibirMenuEspacos(),
                aoVoltarMenuPrincipal: () => MenuPrincipal.ExibirPrincipal()
            );

            return status;

        }

        public static class VisualizadorFinanceiro
        {
            /// <summary>
            /// Exibe visualmente o resumo financeiro de uma reserva.
            /// Inclui detalhes de diária, descontos, extras e total estimado.
            /// </summary>
            public static void ExibirResumoFinanceiro(ResumoFinanceiro resumo)
            {
                Console.WriteLine(); // Espaço emocional

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Resumo financeiro:");
                Console.ResetColor();

                Console.WriteLine($"Diária: R$ {resumo.ValorDiaria:N2}");
                Console.WriteLine($"Sem desconto: R$ {resumo.ValorSemDesconto:N2}");
                Console.WriteLine($"Desconto: R$ {resumo.ValorDoDesconto:N2}");
                Console.WriteLine($"Com desconto: R$ {resumo.ValorComDesconto:N2}");
                Console.WriteLine($"Extras: R$ {resumo.ValorTaxaExtra:N2}");

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"↳ Total estimado: R$ {resumo.ValorTotalEstimado:N2}");
                Console.ResetColor();
            }
        }

    }
}
