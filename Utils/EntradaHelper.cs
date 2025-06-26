using System;

namespace DesafioProjetoHospedagem.Utils
{
    public static class EntradaHelper
    {
        // ======== VERSÕES BÁSICAS =========

        public static string LerTexto(string mensagem)
        {
            string? entrada;
            do
            {
                Console.Write($"{mensagem}: ");
                entrada = Console.ReadLine();
            }
            while (string.IsNullOrWhiteSpace(entrada));

            return entrada.Trim();
        }

        public static int LerInteiro(string mensagem)
        {
            int valor;
            while (true)
            {
                Console.Write($"{mensagem}: ");
                if (int.TryParse(Console.ReadLine(), out valor))
                    return valor;

                Console.WriteLine("Valor inválido. Tente novamente.");
            }
        }

        public static decimal LerDecimal(string mensagem)
        {
            decimal valor;
            while (true)
            {
                Console.Write($"{mensagem}: ");
                if (decimal.TryParse(Console.ReadLine(), out valor))
                    return valor;

                Console.WriteLine("Valor inválido. Tente novamente.");
            }
        }

        // ======== VERSÕES COM CANCELAMENTO =========

        public static string? LerTextoComCancelamento(string mensagem, int maxVazios = 3)
        {
            int vazios = 0;
            while (true)
            {
                Console.Write($"{mensagem}: ");
                string? entrada = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(entrada))
                    return entrada.Trim();

                vazios++;

                if (vazios >= maxVazios)
                {
                    Console.Write("\nVocê pressionou Enter várias vezes. Deseja cancelar e voltar ao menu atual? (S/N): ");
                    string resposta = Console.ReadLine()!.Trim().ToUpper();
                    if (resposta == "S")
                        return null;

                    vazios = 0;
                }
            }
        }

        public static int? LerInteiroComCancelamento(string mensagem, int maxVazios = 3)
        {
            int vazios = 0;
            while (true)
            {
                Console.Write($"{mensagem}: ");
                string? entrada = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(entrada) && int.TryParse(entrada, out int valor))
                    return valor;

                if (string.IsNullOrWhiteSpace(entrada))
                {
                    vazios++;
                    if (vazios >= maxVazios)
                    {
                        Console.Write("\nVocê pressionou Enter várias vezes. Deseja cancelar e voltar ao menu atual? (S/N): ");
                        string resposta = Console.ReadLine()!.Trim().ToUpper();
                        if (resposta == "S")
                            return null;

                        vazios = 0;
                    }
                }
                else
                {
                    Console.WriteLine("Valor inválido. Tente novamente.");
                }
            }
        }

        public static decimal? LerDecimalComCancelamento(string mensagem, int maxVazios = 3)
        {
            int vazios = 0;
            while (true)
            {
                Console.Write($"{mensagem}: ");
                string? entrada = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(entrada) && decimal.TryParse(entrada, out decimal valor))
                    return valor;

                if (string.IsNullOrWhiteSpace(entrada))
                {
                    vazios++;
                    if (vazios >= maxVazios)
                    {
                        Console.Write("\nVocê pressionou Enter várias vezes. Deseja cancelar e voltar ao menu atual? (S/N): ");
                        string resposta = Console.ReadLine()!.Trim().ToUpper();
                        if (resposta == "S")
                            return null;

                        vazios = 0;
                    }
                }
                else
                {
                    Console.WriteLine("Valor inválido. Tente novamente.");
                }
            }
        }
    }
}
