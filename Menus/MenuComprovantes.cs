using System;

namespace Menus;

public static class MenuComprovantes
{
    public static void Exibir()
    {
        int opcao;

        do
        {
            Console.Clear();
            Console.WriteLine("\n ======== SISTEMA DE HOSPEDAGEM | HOTEL FICTÍCIO ========");
            Console.WriteLine($" Data: {DateTime.Now:dd/MM/yyyy} | Hora: {DateTime.Now:HH:mm:ss}");
            Console.WriteLine(" ========== MENU - COMPROVANTES ==========\n");
            Console.WriteLine("1 - Emitir comprovante de reserva");
            Console.WriteLine("0 - Voltar\n");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Escolha uma opção: ");
            Console.ResetColor();

            string entrada = Console.ReadLine()!;
            if (!int.TryParse(entrada, out opcao))
            {
                opcao = -1;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nOpção inválida. Pressione qualquer tecla para tentar novamente...");
                Console.ResetColor();
                Console.ReadKey();
                continue;
            }

            switch (opcao)
            {
                case 1:
                    EmitirComprovanteReserva(); // esse método precisa existir ou ser criado!
                    break;

                case 0:
                    break;

                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nOpção inválida. Pressione qualquer tecla para tentar novamente...");
                    Console.ResetColor();
                    Console.ReadKey();
                    break;
            }

        } while (opcao != 0);
    }

    private static void EmitirComprovanteReserva()
    {
        Console.Clear();
        Console.WriteLine("Emitindo comprovante de reserva...");
        Console.WriteLine("[Funcionalidade em construção]");
        Console.ReadKey();
    }
}
