using System;
using DesafioProjetoHospedagem.Models;
using DesafioProjetoHospedagem.Utils;
using DesafioProjetoHospedagem.Menus;
using DesafioProjetoHospedagem.Controladoras;

namespace DesafioProjetoHospedagem.Utils
{
    public enum TipoMensagem
    {
        Erro,
        Sucesso,
        Instrucao,
        Informacao,
        Alerta,
        Informativo
    }

    public static class CentralSaida
    {

        //A
        public static void AguardarTeclaParaRetornar()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\nPressione qualquer tecla para continuar...");
            Console.ResetColor();
            Console.ReadKey(true);
        }
        //B
        public static void Borracha(string contexto = "", bool exibirInstrucao = true)
        {
            Console.Clear();

            // Visual inspirado no MiniMenu
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("BORRACHA ATIVADA – Tela limpa com sucesso!");
            Console.ResetColor();

            Thread.Sleep(300); // Efeito visual leve

            if (!string.IsNullOrWhiteSpace(contexto))
                CabecalhoMenus(contexto);

            if (exibirInstrucao)
                Exibir(TipoMensagem.Instrucao, "Vamos continuar!");
        }

        //C
        public static void CabecalhoMenus(string nomeMenu)
        {
            Console.Clear();
            CentralizarTexto("SISTEMA DE HOSPEDAGEM – HOTEL FICTÍCIO");
            CentralizarTexto(DateTime.Now.ToString("dd/MM/yyyy  HH:mm:ss"), ConsoleColor.Blue);
            LinhaSeparadora('─');
            CentralizarTexto(nomeMenu.ToUpper(), ConsoleColor.Blue);
            Console.WriteLine();
        }
        public static void CabecalhoMensagem(string titulo)
        {
            Console.Clear(); // ou LimparTelaRetornoMenus(titulo) se preferir reaproveitar estilo
            LinhaSeparadora();
            CentralizarTexto(titulo);
            LinhaSeparadora();
        }
        public static void CentralizarTexto(string texto, ConsoleColor? cor = null)
        {
            int posicao = Math.Max((Console.WindowWidth - texto.Length) / 2, 0);
            Console.SetCursorPosition(posicao, Console.CursorTop);

            if (cor.HasValue)
                Console.ForegroundColor = cor.Value;

            Console.WriteLine(texto);

            if (cor.HasValue)
                Console.ResetColor();
        }

        //E
        public static void Exibir(TipoMensagem tipo, string mensagem)
        {Console.ForegroundColor = tipo switch
{
    TipoMensagem.Erro => ConsoleColor.Red,
    TipoMensagem.Sucesso => ConsoleColor.Green,
    TipoMensagem.Instrucao => ConsoleColor.DarkCyan,
    TipoMensagem.Informacao => ConsoleColor.White,
    TipoMensagem.Alerta => ConsoleColor.DarkMagenta,
    _ => ConsoleColor.Gray
};


            Console.WriteLine($"\n{mensagem}");
            Console.ResetColor();
        }

        //Quando usar, chamar com: CentralSaida.ExibirDespedidaSimples("Encerrando o sistema...");
        // Quando quiser encerrar o sistema: CentralSaida.EncerrarSistema("Nos vemos em breve!");
        public static void ExibirDespedidaSimples(string mensagem = "Até logo!")
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n" + mensagem);
            Console.ResetColor();
            Console.WriteLine("Obrigado por utilizar nosso sistema. Encerrando...");
        }
        public static void ExibirDestaque(string mensagem)
        {
            Console.WriteLine(); // quebra superior
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine($"● {mensagem}");
            Console.ResetColor();
            Console.WriteLine(); // quebra inferior
        }

        /*//Exibir pergunta
            public static void ExibirPergunta(string mensagem)
            {
                Exibir(TipoMensagem.Instrucao, mensagem);

                Console.Write("> ");
            }
        */
        public static void ErroIntervalo(int minimo, int maximo)
        {
            Exibir(TipoMensagem.Erro, $"Digite um número entre {minimo} e {maximo}.");
        }

        // L
        public static void LimparTela(string titulo)
        {
            Thread.Sleep(500); // Pequeno delay para suavizar o "piscar" da troca de telas
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine($" {titulo.ToUpper()} \n");
            Console.ResetColor();
        }
        public static void LimparTelaMesmoMenu()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("••• Atualizando tela •••");
            Console.ResetColor();
            Thread.Sleep(300); // pequena pausa
            Console.Clear();
        }
        public static void LimparTelaRetornoMenus(string titulo)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($" {titulo.ToUpper()} \n");
            Console.ResetColor();
        }
        public static void LinhaSeparadora(char caractere = '─', ConsoleColor? cor = null)
        {
            if (cor.HasValue)
                Console.ForegroundColor = cor.Value;

            Console.WriteLine(new string(caractere, Console.WindowWidth));

            if (cor.HasValue)
                Console.ResetColor();
        }

        //M
        public static void MensagemPadraoSucesso(string contexto)
        {
            Exibir(TipoMensagem.Sucesso, $"{contexto} concluído com sucesso!");
        }
        public static void MensagemDeCancelamento(string contexto = "Operação")
        {
            Exibir(TipoMensagem.Informacao, $"{contexto} cancelada pelo usuário.");
        }

        // S
        public static void Submenu(string titulo)
        {
            Console.Clear();
            CabecalhoMenus(titulo);
            Exibir(TipoMensagem.Instrucao, "Você está em um submenu. Utilize os atalhos abaixo:");
        }
        public static void Subtitulo(string texto)
        {
            Console.WriteLine(); // espaçamento superior

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"== {texto.ToUpper()} ==");
            Console.ResetColor();

            Console.WriteLine(); // espaçamento inferior
        }



    }
}

