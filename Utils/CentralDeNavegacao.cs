using System;
using DesafioProjetoHospedagem.Models;
using DesafioProjetoHospedagem.Utils;
using DesafioProjetoHospedagem.Menus;
using DesafioProjetoHospedagem.Controladoras;

namespace DesafioProjetoHospedagem.Utils
{
    public static class CentralDeNavegacao

    {
        /*digitou zero retorna oa menu não é mais usado
        public static void DigitouZeroRetornaMenu(string tituloRetorno = "Menu anterior")
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nOk, vamos retornar ao menu anterior.");
            Console.ResetColor();

            InterfaceHelper.AnimacaoGiratoria("Retornando ao menu anterior");

            CentralSaida.LimparTelaRetornoMenus(tituloRetorno);
        }
        */

        public static void EncerrarSistema(string mensagem = "Encerrando o sistema...")
        {
            CentralSaida.ExibirDespedidaSimples("Encerrando o sistema...");

        }

        public static void ReiniciarEntrada(string mensagem = "Vamos tentar novamente!")
        {
            Console.Clear();
            CentralSaida.Exibir(TipoMensagem.Sucesso, mensagem);
        }

               
        public static void VoltarAoMenu(string titulo = "Menu anterior")
        {
            CentralSaida.Exibir(TipoMensagem.Informacao, $"\nRetornando para {titulo}...");
            InterfaceHelper.AnimacaoGiratoria("Carregando " + titulo);
            CentralSaida.LimparTelaRetornoMenus(titulo);
        }
    }

}

