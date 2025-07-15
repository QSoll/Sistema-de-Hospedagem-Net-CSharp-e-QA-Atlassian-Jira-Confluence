using System;
using System.Collections.Generic;
using DesafioProjetoHospedagem.Models;
using DesafioProjetoHospedagem.Utils;
using DesafioProjetoHospedagem.Controladoras;
using DesafioProjetoHospedagem.Sessao;
using DesafioProjetoHospedagem.Menus;

namespace DesafioProjetoHospedagem.Utils
{
    public static class CentralEntradaData
    {
        public static DateTime LerData(string mensagem)
        {
            DateTime data;
            bool sucesso = false;

            do
            {
                Console.WriteLine(mensagem);
                string? entrada = Console.ReadLine();

                if (DateTime.TryParse(entrada, out data))
                {
                    sucesso = true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Data inválida. Use o formato dd/MM/yyyy.");
                    Console.ResetColor();
                }

            } while (!sucesso);

            return data;
        }
    }
}
