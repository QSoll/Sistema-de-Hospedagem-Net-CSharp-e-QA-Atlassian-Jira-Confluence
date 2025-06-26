using System;
using DesafioProjetoHospedagem.Models;
using DesafioProjetoHospedagem.Utils;


namespace Menus;

public static class MenuComprovantes
{
    public static void Exibir(List<Reserva> reservas)

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
        Console.WriteLine("=== EMISSÃO DE COMPROVANTE ===\n");

        int? codigoReserva = EntradaHelper.LerInteiroComCancelamento("Digite o número da reserva");
        if (codigoReserva == null)
        {
            Console.WriteLine("\nEmissão cancelada. Retornando ao menu...");
            return;
        }

        // Aqui entraria sua lógica para localizar a reserva com esse ID.
        // Exemplo (quando estiver implementado):
        // var reserva = reservas.FirstOrDefault(r => r.Id == codigoReserva.Value);
        // if (reserva == null) { ... }

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\nEsta funcionalidade está em construção. Em breve você poderá emitir comprovantes detalhados aqui!");
        Console.ResetColor();

        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
    }

}
