using System;
using System.Threading;
using DesafioProjetoHospedagem.Models;
using DesafioProjetoHospedagem.Utils;
using DesafioProjetoHospedagem.Menus;
using DesafioProjetoHospedagem.Controladoras;

namespace DesafioProjetoHospedagem.Utils
{
    public static class EntradaHelper
    {
        // ======== MÉTODOS AUXILIARES INTERNOS =========

        //digitou enter 3 vezes
        private static bool ConfirmarCancelamento()
        {
            string? resposta;
            do
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("\nVocê pressionou Enter várias vezes. Deseja cancelar e voltar ao menu atual? (S/N): ");
                Console.ResetColor();

                resposta = Console.ReadLine()?.Trim().ToUpper();

                if (resposta == "S") return true;
                if (resposta == "N") return false;

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Entrada inválida. Digite apenas S ou N.");
                Console.ResetColor();
            } while (true);
        }       // ======== ENTRADAS BÁSICAS =========

        //-----------------------------------------------------

        // digitar de 1 até limite
        private static void ExibirMensagemEntradaNumerada(string mensagem, int limite)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"\n{mensagem} (entre 1 e {limite}): ");
            Console.ResetColor();
        }

        //0 para voltar, até limite
        private static void ExibirMensagemEntrada(string mensagem, int limite)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"\n{mensagem} (0 para voltar, até {limite}): ");
            Console.ResetColor();
        }

        public static void ExibirMensagemRetorno(
    string mensagem = "Operação concluída com sucesso.",
    string nomeMenu = "Menu",
    Action? voltarParaSubmenu = null
)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n{mensagem}");
            Console.ResetColor();

            MenuRodape.ExibirRodape(
                nomeMenu,
                aoVoltarParaSubmenu: voltarParaSubmenu ?? (() => NavegacaoAtual.Voltar()),
                aoVoltarMenuPrincipal: () => MenuPrincipal.ExibirPrincipal()
            );
        }



    }
}
