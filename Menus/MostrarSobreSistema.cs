using System;
using DesafioProjetoHospedagem.Utils;
using DesafioProjetoHospedagem.Models;
using DesafioProjetoHospedagem.Menus;
using DesafioProjetoHospedagem.Sessao;

namespace DesafioProjetoHospedagem.Menus
{
    public static class MostrarSobreSistema
    {
        public static void ExibirSobreSistema()

        {
            Console.Clear();
            CentralSaida.CabecalhoMenus("Sobre o Sistema");

            Console.WriteLine("Sistema de Gestão Hoteleira - v1.0");
            Console.WriteLine(new string('=', 40));

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Desenvolvido por ");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Magenta;
            InterfaceHelper.EscreverComAnimacao("SolMorcillo 💛");
            Console.ResetColor();

            Console.WriteLine("Suporte: +55 028 99945-2754");

            Console.WriteLine();
            Console.WriteLine("© 2025 - Todos os direitos reservados");

            MenuRodape.ExibirRodape(
    "Menu – Sobre o Sistema",
    aoVoltarMenuPrincipal: () => MenuPrincipal.ExibirPrincipal()
            );




        }
    }
}
