using System;
using System.Linq;
using System.Collections.Generic;
using DesafioProjetoHospedagem.Models;
using DesafioProjetoHospedagem.Utils;

namespace Menus
{
    public static class MenuHospedes
    {
        public static void Exibir(List<Hospede> hospedes, List<Reserva> reservas)
{
    int opcao;

    do
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("========================================");
        Console.WriteLine("\n   SISTEMA DE HOSPEDAGEM | HOTEL FICTÍCIO");
        Console.WriteLine($"   Data: {DateTime.Now:dd/MM/yyyy}   | Hora: {DateTime.Now:HH:mm:ss}");
        Console.WriteLine("========================================");
        Console.ResetColor();

        Console.WriteLine("\n------------ MENU - HÓSPEDES ------------\n");
        Console.WriteLine("1 - Cadastrar novo hóspede");
        Console.WriteLine("2 - Ver hóspedes (ativos/cadastrados)");
        Console.WriteLine("3 - Excluir hóspede (em breve)");
        Console.WriteLine("0 - Voltar para o menu principal");

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("\nEscolha uma opção: ");
        Console.ResetColor();

        string entrada = Console.ReadLine()!;
        if (!int.TryParse(entrada, out opcao))
            opcao = -1;

        switch (opcao)
        {
            case 1:
                AdicionarHospedeCadastrado(hospedes);
                break;

            case 2:
                ListarHospedes(hospedes, reservas);
                break;

            case 3:
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n[Excluir hóspede] - Recurso em desenvolvimento...");
                Console.ResetColor();
                Console.WriteLine("\nPressione qualquer tecla para continuar...");
                Console.ReadKey();
                break;

            case 0:
                break;

            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nOpção inválida.");
                Console.ResetColor();
                Console.WriteLine("\nPressione qualquer tecla para tentar novamente...");
                Console.ReadKey();
                break;
        }

    } while (opcao != 0);
}

        private static void AdicionarHospedeCadastrado(List<Hospede> hospedes)
        {
            Console.Clear();
            Console.WriteLine("=== CADASTRO DE HÓSPEDE ===\n");

            string? nome = EntradaHelper.LerTextoComCancelamento("Informe o primeiro nome");
            if (nome == null) return;

            string? sobrenome = EntradaHelper.LerTextoComCancelamento("Informe o sobrenome");
            if (sobrenome == null) return;

            string nomeCompleto = $"{nome} {sobrenome}".ToUpperInvariant();

            if (hospedes.Any(h => h.NomeCompleto.ToUpperInvariant() == nomeCompleto))
            {
                Console.WriteLine("\nJá existe um hóspede com esse nome completo.");
            }
            else
            {
                string? email = EntradaHelper.LerTextoComCancelamento("Informe o e-mail");
                if (email == null) return;

                Hospede novoHospede = new(nome, sobrenome, email);
                hospedes.Add(novoHospede);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nHóspede \"{novoHospede.NomeCompleto}\" cadastrado com sucesso!");
                Console.WriteLine($"Data do cadastro: {novoHospede.DataCadastro:dd/MM/yyyy HH:mm}");
                Console.ResetColor();
            }

            Console.WriteLine("\nPressione qualquer tecla para retornar ao menu...");
            Console.ReadKey();
        }

        private static void ListarHospedes(List<Hospede> hospedes, List<Reserva> reservas)
        {
            Console.Clear();
            Console.WriteLine("=== HÓSPEDES ===\n");

            if (!hospedes.Any())
            {
                Console.WriteLine("Nenhum hóspede cadastrado.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("1 - Ver hóspedes atualmente hospedados");
            Console.WriteLine("2 - Consultar hóspede por nome");
            Console.WriteLine("0 - Voltar");
            Console.Write("\nEscolha uma opção: ");
            string? opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    ListarHospedadosAtivos(reservas);
                    break;
                case "2":
                    Console.Clear();
                    Console.WriteLine("=== CONSULTA DE HÓSPEDE ===\n");

                    string? termo = EntradaHelper.LerTextoComCancelamento("Digite parte do nome ou sobrenome para buscar");
                    if (termo == null) return;

                    var encontrados = hospedes
                        .Where(h => h.Nome.ToUpper().Contains(termo.ToUpper()) || h.Sobrenome.ToUpper().Contains(termo.ToUpper()))
                        .ToList();

                    if (!encontrados.Any())
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\nNenhum hóspede encontrado com esse termo.");
                        Console.ResetColor();
                        break;
                    }

                    Console.WriteLine("\nHóspedes encontrados:");
                    for (int i = 0; i < encontrados.Count; i++)
                        Console.WriteLine($"[{i + 1}] {encontrados[i].NomeCompleto} ({encontrados[i].Email})");

                    int escolha;
                    do
                    {
                        Console.Write("\nDigite o número do hóspede que deseja consultar: ");
                    } while (!int.TryParse(Console.ReadLine(), out escolha) || escolha < 1 || escolha > encontrados.Count);

                    var selecionado = encontrados[escolha - 1];

                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"=== DETALHES DO HÓSPEDE: {selecionado.NomeCompleto} ===\n");
                    Console.ResetColor();
                    Console.WriteLine($"Email: {selecionado.Email}");
                    Console.WriteLine($"Data de cadastro: {selecionado.DataCadastro:dd/MM/yyyy HH:mm}");

                    var reservaAtual = reservas.FirstOrDefault(r => r.Ativa && r.Hospedes.Any(h => h.NomeCompleto == selecionado.NomeCompleto));

                    if (reservaAtual != null)
                    {
                        var checkOut = reservaAtual.DataCheckIn.AddDays(reservaAtual.DiasReservados);

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\nStatus: HOSPEDADO");
                        Console.ResetColor();
                        Console.WriteLine($"- Espaço: {reservaAtual.Espaco.NomeFantasia} (ID {reservaAtual.Espaco.Id})");
                        Console.WriteLine($"- Check-in: {reservaAtual.DataCheckIn:dd/MM/yyyy}");
                        Console.WriteLine($"- Check-out previsto: {checkOut:dd/MM/yyyy}");
                        Console.WriteLine($"- Valor total: R$ {reservaAtual.CalcularValorDiaria():F2}");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\nStatus: NÃO HOSPEDADO");
                        Console.ResetColor();
                    }
                    break;

                default:
                    return;
            }

            Console.WriteLine("\nPressione qualquer tecla para retornar...");
            Console.ReadKey();
        }
        private static void ListarHospedadosAtivos(List<Reserva> reservas)
        {
            Console.Clear();
            Console.WriteLine("=== HÓSPEDES ATUALMENTE HOSPEDADOS ===\n");

            var hospedados = reservas
                .Where(r => r.Ativa)
                .SelectMany(r => r.Hospedes.Select(h => new
                {
                    Nome = h.NomeCompleto,
                    Espaco = r.Espaco.NomeFantasia,
                    Id = r.Espaco.Id,
                    Dias = r.DiasReservados,
                    Valor = r.CalcularValorDiaria()
                }))
                .ToList();

            if (!hospedados.Any())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Nenhum hóspede está hospedado no momento.");
                Console.ResetColor();
            }
            else
            {
                foreach (var h in hospedados)
                {
                    Console.WriteLine($"- {h.Nome}");
                    Console.WriteLine($"  Espaço: {h.Espaco} (ID: {h.Id:D3})");
                    Console.WriteLine($"  Duração: {h.Dias} dia(s)");
                    Console.WriteLine($"  Valor total: R$ {h.Valor:F2}\n");
                }
            }

            Console.WriteLine("Pressione qualquer tecla para retornar...");
            Console.ReadKey();
        }


        private static void BuscarClienteComDetalhes(List<Hospede> hospedes, List<Reserva> reservas)
        {
            Console.Clear();
            Console.WriteLine("=== CONSULTA DE HÓSPEDE ===\n");

            string? termo = EntradaHelper.LerTextoComCancelamento("Digite parte do nome ou sobrenome para buscar");
            if (termo == null) return;

            var encontrados = hospedes
                .Where(h => h.Nome.ToUpper().Contains(termo.ToUpper()) || h.Sobrenome.ToUpper().Contains(termo.ToUpper()))
                .ToList();

            if (!encontrados.Any())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nNenhum hóspede encontrado com esse termo.");
                Console.ResetColor();
                return;
            }

            Console.WriteLine("\nHóspedes encontrados:");
            for (int i = 0; i < encontrados.Count; i++)
                Console.WriteLine($"[{i + 1}] {encontrados[i].NomeCompleto} ({encontrados[i].Email})");

            int escolha;
            do
            {
                Console.Write("\nDigite o número do hóspede que deseja consultar: ");
            } while (!int.TryParse(Console.ReadLine(), out escolha) || escolha < 1 || escolha > encontrados.Count);

            var selecionado = encontrados[escolha - 1];

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"=== DETALHES DO HÓSPEDE: {selecionado.NomeCompleto} ===\n");
            Console.ResetColor();

            Console.WriteLine($"Email: {selecionado.Email}");
            Console.WriteLine($"Data de cadastro: {selecionado.DataCadastro:dd/MM/yyyy HH:mm}");

            var reservaAtual = reservas.FirstOrDefault(r =>
                r.Ativa && r.Hospedes.Any(h => h.NomeCompleto == selecionado.NomeCompleto));

            if (reservaAtual != null)
            {
                var checkOut = reservaAtual.DataCheckIn.AddDays(reservaAtual.DiasReservados);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nStatus: HOSPEDADO");
                Console.ResetColor();
                Console.WriteLine($"- Espaço: {reservaAtual.Espaco.NomeFantasia} (ID {reservaAtual.Espaco.Id:D3})");
                Console.WriteLine($"- Check-in: {reservaAtual.DataCheckIn:dd/MM/yyyy}");
                Console.WriteLine($"- Check-out previsto: {checkOut:dd/MM/yyyy}");
                Console.WriteLine($"- Valor total: R$ {reservaAtual.CalcularValorDiaria():F2}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nStatus: NÃO HOSPEDADO");
                Console.ResetColor();
            }

            Console.WriteLine("\nPressione qualquer tecla para retornar...");
            Console.ReadKey();
        }

    }
}