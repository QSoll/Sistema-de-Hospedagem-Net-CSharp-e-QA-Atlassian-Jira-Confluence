using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using DesafioProjetoHospedagem.Models;

namespace Menus
{
    public static class ComprovanteReserva
{
    public static void Emitir(List<Reserva> reservas, ref Reserva? reservaSelecionada)
    {
        Console.Clear();
        Console.WriteLine("=== EMISSÃO DO COMPROVANTE DE RESERVA ===");

        if (!reservas.Any())
        {
            Console.WriteLine("Nenhuma reserva encontrada no sistema.");
            Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("Deseja buscar a reserva por:");
        Console.WriteLine("1 - Número da reserva (ID)");
        Console.WriteLine("2 - Nome ou sobrenome do hóspede");
        Console.Write("\nSua escolha: ");
        string? opcaoBusca = Console.ReadLine();

        if (opcaoBusca == "1")
        {
            for (int i = 0; i < reservas.Count; i++)
            {
                var r = reservas[i];
                Console.WriteLine($"{i + 1} - Suíte {r.Espaco.TipoEspaco} ({r.Hospedes.Count} hóspede[s], {r.DiasReservados} dia[s])");
            }

            Console.Write("\nDigite o número da reserva desejada: ");
            if (int.TryParse(Console.ReadLine(), out int idEscolhido) && idEscolhido >= 1 && idEscolhido <= reservas.Count)
            {
                reservaSelecionada = reservas[idEscolhido - 1];
            }
            else
            {
                Console.WriteLine("\nID inválido. Voltando ao menu.");
                Console.ReadKey();
                return;
            }
        }
        else if (opcaoBusca == "2")
        {
            string? termo;
            do
            {
                Console.Write("Digite o nome ou sobrenome do hóspede: ");
                termo = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(termo))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Termo de busca obrigatório. Por favor, tente novamente.\n");
                    Console.ResetColor();
                }

            } while (string.IsNullOrWhiteSpace(termo));

            termo = termo.ToUpper();

            var resultados = reservas
                .Where(r => r.Hospedes.Any(h =>
                    h.NomeCompleto?.ToUpper().Contains(termo) == true))
                .ToList();

            if (!resultados.Any())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nNenhuma reserva encontrada com esse nome.");
                Console.ResetColor();
                Console.ReadKey();
                return;
            }

            if (resultados.Count == 1)
            {
                reservaSelecionada = resultados.First();
            }
            else
            {
                Console.WriteLine("\nForam encontradas várias reservas:");
                for (int i = 0; i < resultados.Count; i++)
                {
                    var r = resultados[i];
                    Console.WriteLine($"{i + 1} - Suíte {r.Espaco.TipoEspaco}, {r.DiasReservados} dia(s), {r.Hospedes.Count} hóspede(s)");
                }

                int escolha;
                bool escolhaValida = false;

                do
                {
                    Console.Write("\nDigite o número da reserva desejada: ");
                    if (int.TryParse(Console.ReadLine(), out escolha) && escolha >= 1 && escolha <= resultados.Count)
                    {
                        reservaSelecionada = resultados[escolha - 1];
                        escolhaValida = true;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Escolha inválida. Tente novamente.");
                        Console.ResetColor();
                    }

                } while (!escolhaValida);
            }
        }
        else
        {
            Console.WriteLine("\nOpção inválida.");
            Console.ReadKey();
            return;
        }

        // Com a reserva selecionada:
        Directory.CreateDirectory("Comprovantes");
        string nomeArquivo = Path.Combine("Comprovantes", $"Comprovante_{DateTime.Now:yyyyMMdd_HHmmss}.txt");

        using (StreamWriter writer = new StreamWriter(nomeArquivo))
        {
            if (reservaSelecionada != null)
            {
                writer.WriteLine("\n= SISTEMA DE HOSPEDAGEM - HOTEL FICTÍCIO =");
                writer.WriteLine("==========================================");
                writer.WriteLine($"Data de emissão: {DateTime.Now:dd/MM/yyyy} às {DateTime.Now:HH:mm:ss}");
                writer.WriteLine("========= COMPROVANTE DE RESERVA =========");
                writer.WriteLine();
                writer.WriteLine($"Suíte: {reservaSelecionada.Espaco.TipoEspaco}");
                writer.WriteLine($"Dias reservados: {reservaSelecionada.DiasReservados}");
                writer.WriteLine($"Valor estimado: {reservaSelecionada.CalcularValorDiaria():C}");
                writer.WriteLine();
                writer.WriteLine("Hóspedes:");

                if (reservaSelecionada.Hospedes?.Any() == true)
                {
                    foreach (var h in reservaSelecionada.Hospedes)
                    {
                        writer.WriteLine($"- {h.NomeCompleto}");
                    }
                }
                else
                {
                    writer.WriteLine("- Nenhum hóspede cadastrado.");
                }

                writer.WriteLine("\nObs: Valores podem variar em caso de consumo adicional ou permanência extra.");
            }
            else
            {
                writer.WriteLine("\n[ERRO] Nenhuma reserva foi selecionada para gerar o comprovante.");
            }
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\nComprovante gerado com sucesso: {Path.GetFullPath(nomeArquivo)}");
        Console.ResetColor();

        Console.WriteLine("\nPressione qualquer tecla para retornar ao menu...");
        Console.ReadKey();
    }
}
}





/*void EmitirComprovanteReserva()
{
    Console.Clear();
    Console.WriteLine("=== EMISSÃO DO COMPROVANTE DE RESERVA ===");

    if (!reservas.Any())
    {
        Console.WriteLine("Nenhuma reserva encontrada no sistema.");
        Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
        Console.ReadKey();
        return;
    }

    Console.WriteLine("Deseja buscar a reserva por:");
    Console.WriteLine("1 - Número da reserva (ID)");
    Console.WriteLine("2 - Nome ou sobrenome do hóspede");
    Console.Write("\nSua escolha: ");
    string? opcaoBusca = Console.ReadLine();

    if (opcaoBusca == "1")
    {
        for (int i = 0; i < reservas.Count; i++)
        {
            var r = reservas[i];
            Console.WriteLine($"{i + 1} - Suíte {r.Espaco.TipoEspaco} ({r.Hospedes.Count} hóspede[s], {r.DiasReservados} dia[s])");
        }

        Console.Write("\nDigite o número da reserva desejada: ");
        if (int.TryParse(Console.ReadLine(), out int idEscolhido) && idEscolhido >= 1 && idEscolhido <= reservas.Count)
        {
            reservaSelecionada = reservas[idEscolhido - 1];
        }
        else
        {
            Console.WriteLine("\nID inválido. Voltando ao menu.");
            Console.ReadKey();
            return;
        }
    }
    else if (opcaoBusca == "2")
    {
        string? termo;
        do
        {
            Console.Write("Digite o nome ou sobrenome do hóspede: ");
            termo = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(termo))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Termo de busca obrigatório. Por favor, tente novamente.\n");
                Console.ResetColor();
            }

        } while (string.IsNullOrWhiteSpace(termo));

        termo = termo.ToUpper();

        var resultados = reservas
            .Where(r => r.Hospedes.Any(h =>
                h.NomeCompleto?.ToUpper().Contains(termo) == true))
            .ToList();

        if (!resultados.Any())
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nNenhuma reserva encontrada com esse nome.");
            Console.ResetColor();
            Console.ReadKey();
            return;
        }

        if (resultados.Count == 1)
        {
            reservaSelecionada = resultados.First();
        }
        else
        {
            Console.WriteLine("\nForam encontradas várias reservas:");
            for (int i = 0; i < resultados.Count; i++)
            {
                var r = resultados[i];
                Console.WriteLine($"{i + 1} - Suíte {r.Espaco.TipoEspaco}, {r.DiasReservados} dia(s), {r.Hospedes.Count} hóspede(s)");
            }

            int escolha;
            bool escolhaValida = false;

            do
            {
                Console.Write("\nDigite o número da reserva desejada: ");
                if (int.TryParse(Console.ReadLine(), out escolha) && escolha >= 1 && escolha <= resultados.Count)
                {
                    reservaSelecionada = resultados[escolha - 1];
                    MenuReservaSelecionada();

                    escolhaValida = true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Escolha inválida. Tente novamente.");
                    Console.ResetColor();
                }

            } while (!escolhaValida);
        }
    }

    else
    {
        Console.WriteLine("\nOpção inválida.");
        Console.ReadKey();
        return;
    }

    // Com a reserva selecionada:
    Directory.CreateDirectory("Comprovantes");
    string nomeArquivo = Path.Combine("Comprovantes", $"Comprovante_{DateTime.Now:yyyyMMdd_HHmmss}.txt");

    using (StreamWriter writer = new StreamWriter(nomeArquivo))
        if (reservaSelecionada != null)
        {
            writer.WriteLine("\n= SISTEMA DE HOSPEDAGEM - HOTEL FICTÍCIO =");
            writer.WriteLine("==========================================");
            writer.WriteLine($"Data de emissão: {DateTime.Now:dd/MM/yyyy} às {DateTime.Now:HH:mm:ss}");
            writer.WriteLine("========= COMPROVANTE DE RESERVA =========");
            writer.WriteLine();
            writer.WriteLine($"Suíte: {reservaSelecionada.Suite.TipoSuite}");
            writer.WriteLine($"Dias reservados: {reservaSelecionada.DiasReservados}");
            writer.WriteLine($"Valor estimado: {reservaSelecionada.CalcularValorDiaria():C}");
            writer.WriteLine();
            writer.WriteLine("Hóspedes:");

            if (reservaSelecionada.Hospedes?.Any() == true)
            {
                foreach (var h in reservaSelecionada.Hospedes)
                {
                    writer.WriteLine($"- {h.NomeCompleto}");
                }
            }
            else
            {
                writer.WriteLine("- Nenhum hóspede cadastrado.");
            }

            writer.WriteLine("\nObs: Valores podem variar em caso de consumo adicional ou permanência extra.");
        }
        else
        {
            writer.WriteLine("\n[ERRO] Nenhuma reserva foi selecionada para gerar o comprovante.");
        }

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"\nComprovante gerado com sucesso: {Path.GetFullPath(nomeArquivo)}");
    Console.ResetColor();

    Console.WriteLine("\nPressione qualquer tecla para retornar ao menu...");
    Console.ReadKey();
}// fim emitirComprovante
*/