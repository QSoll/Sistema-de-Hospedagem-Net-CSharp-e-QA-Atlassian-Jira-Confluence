using System;

namespace DesafioProjetoHospedagem.Utils
{
    public static class EntradaHelper
    {
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
    }
}
