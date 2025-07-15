using System;
using System.Threading;
using DesafioProjetoHospedagem.Models;
using DesafioProjetoHospedagem.Utils;
using DesafioProjetoHospedagem.Menus;
using DesafioProjetoHospedagem.Controladoras;

namespace DesafioProjetoHospedagem.Utils
{
    public static class InterfaceHelper
    {
        // A
        public static void AnimacaoGiratoria(string mensagem = "Retornando", int ciclos = 12, int delay = 80)
        {
            string[] simbolos = new[] { "⠋", "⠙", "⠹", "⠸", "⠼", "⠴", "⠦", "⠧", "⠇", "⠏" };

            Console.WriteLine(); // espaçamento
            Console.ForegroundColor = ConsoleColor.Green; // 💚 Cor da animação
            Console.Write($" {mensagem}: ");

            for (int i = 0; i < ciclos; i++)
            {
                Console.Write(simbolos[i % simbolos.Length]);
                Thread.Sleep(delay);
                Console.Write("\b"); // volta o cursor
            }

            //Console.Write("✔");
            Console.ResetColor();
            Thread.Sleep(400); // pausa final
            Console.WriteLine();
        }
        //chamar nos docs: InterfaceHelper.AnimacaoGiratoria("Retornando ao menu anterior");
        //CentralSaida.LimparTelaRetornoMenus("Menu Principal");


        /* como usar AzulBranco:
        foreach (var espaco in espacos)
        {
            InterfaceHelper.AzulBranco(espaco);
        }
        */
        public static void AzulBranco(EspacoHospedagem espaco)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"{espaco.Id} – {espaco.TipoEspaco.ToUpper()} ");

            Console.ResetColor();
            Console.WriteLine(espaco.NomeFantasia);
        }

        // C
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
        public static int CentralSaidaMensagens(string mensagem)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"\n{mensagem}: ");
            Console.ResetColor();

            string? entrada = Console.ReadLine();

            while (!int.TryParse(entrada, out int valor))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Ops. Digite novamente: ");
                Console.ResetColor();
                entrada = Console.ReadLine();
            }

            return int.Parse(entrada);
        }
        //chamar nos com CentralSaidaMensagens.ExibirPerguntaOpcao(" ex: Menu - Espaços");

        // E
        public static void EscreverComAnimacao(string texto, int delay = 30)
        {
            foreach (char c in texto)
            {
                Console.Write(c);
                Thread.Sleep(delay);
            }
            Console.WriteLine();
        }

        public static string EditarTexto(string campo, string valorAtual)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"→ {campo} [atual: {valorAtual}]: ");
            Console.ResetColor();

            string? entrada = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(entrada))
                return valorAtual;

            return entrada;

        }

        public static void ExibirTituloComData(string titulo)
        {
            Console.Clear();

            //Console.WriteLine("===================================");
            Console.WriteLine("___________________________________\n");
            Console.WriteLine($"     {titulo.ToUpper()}");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{DateTime.Now:dd/MM/yyyy}  |  {DateTime.Now:HH:mm:ss}");
            Console.ResetColor();

            Console.WriteLine("___________________________________\n");
        }


    }





}

