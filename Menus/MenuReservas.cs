using System;
using System.Collections.Generic;
using System.Linq;
using DesafioProjetoHospedagem.Models;
using DesafioProjetoHospedagem.Utils;

namespace Menus
{
    public static class MenuReservas
    {
        public static void Exibir(List<Reserva> reservas, List<Hospede> hospedes, List<EspacoHospedagem> espacos)
        {
            int opcao;

            do
            {
                Console.Clear();
                Console.WriteLine("=== MENU DE RESERVAS ===\n");
                Console.WriteLine("1 - Criar nova reserva");
                Console.WriteLine("2 - Ver reservas ativas");
                Console.WriteLine("3 - Cancelar reserva");
                Console.WriteLine("4 - Encerrar reserva");
                Console.WriteLine("0 - Voltar ao menu principal");
                Console.Write("\nEscolha uma opção: ");

                if (!int.TryParse(Console.ReadLine(), out opcao))
                    opcao = -1;

                switch (opcao)
                {
                    case 1:
                        CriarReserva(reservas, hospedes, espacos);
                        break;
                    case 2:
                        ListarReservasAtivas(reservas);
                        break;
                    case 3:
                        CancelarReserva(reservas);
                        break;
                    case 4:
                        EncerrarReserva(reservas);
                        break;
                    case 0:
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\nOpção inválida. Pressione uma tecla para tentar novamente...");
                        Console.ResetColor();
                        Console.ReadKey();
                        break;
                }

            } while (opcao != 0);
        }

        private static void CriarReserva(List<Reserva> reservas, List<Hospede> hospedes, List<EspacoHospedagem> espacos)
{
    Console.Clear();
    Console.WriteLine("=== CRIAÇÃO DE NOVA RESERVA ===\n");

    if (!espacos.Any())
    {
        Console.WriteLine("Nenhum espaço cadastrado ainda.");
        Console.ReadKey();
        return;
    }

    var espacosDisponiveis = espacos
        .Where(e => !reservas.Any(r => r.Ativa && r.Espaco.Id == e.Id))
        .ToList();

    if (!espacosDisponiveis.Any())
    {
        Console.WriteLine("Todos os espaços estão atualmente ocupados.");
        Console.ReadKey();
        return;
    }

    Console.WriteLine("Espaços disponíveis:");
    for (int i = 0; i < espacosDisponiveis.Count; i++)
    {
        var e = espacosDisponiveis[i];
        Console.WriteLine($"[{i + 1}] {e.NomeFantasia} (ID {e.Id:D3}) - R$ {e.ValorDiaria:F2}/dia");
    }

    int? escolha = EntradaHelper.LerInteiroComCancelamento("\nEscolha o espaço pelo número");
    if (escolha == null || escolha < 1 || escolha > espacosDisponiveis.Count)
    {
        Console.WriteLine("\nReserva cancelada. Retornando ao menu...");
        return;
    }

    var espacoSelecionado = espacosDisponiveis[escolha.Value - 1];

    int? dias = EntradaHelper.LerInteiroComCancelamento("Quantos dias de hospedagem");
    if (dias == null || dias <= 0)
    {
        Console.WriteLine("\nReserva cancelada. Retornando ao menu...");
        return;
    }

    List<Hospede> hospedesDaReserva = new();
    bool adicionarMais = true;

    while (adicionarMais)
    {
        AdicionarHospede(hospedes, hospedesDaReserva);

        Console.Write("\nDeseja adicionar outro hóspede? (s/n): ");
        string? continuar = Console.ReadLine()?.Trim().ToLower();
        adicionarMais = continuar == "s";
    }

    var novaReserva = new Reserva(hospedesDaReserva, espacoSelecionado, dias.Value);
    reservas.Add(novaReserva);

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("\nReserva criada com sucesso!");
    Console.WriteLine($"Espaço: {espacoSelecionado.NomeFantasia}");
    Console.WriteLine($"Check-in: {novaReserva.DataCheckIn:dd/MM/yyyy}");
    Console.WriteLine($"Total de hóspedes: {hospedesDaReserva.Count}");
    Console.WriteLine($"Valor total estimado: R$ {novaReserva.CalcularValorDiaria():F2}");
    Console.ResetColor();

    // ✅Exibe o comprovante completo
    GerarComprovante(novaReserva);

    Console.WriteLine("\nPressione qualquer tecla para continuar...");
    Console.ReadKey();
}


        private static void GerarComprovante(Reserva reserva)
        {
            SalvarComprovanteEmArquivo(reserva);

        }

        private static void SalvarComprovanteEmArquivo(Reserva reserva)
        {
            string nomeArquivo = $"comprovante_{reserva.Espaco.NomeFantasia}_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
            string caminhoCompleto = Path.Combine(Environment.CurrentDirectory, nomeArquivo);

            using StreamWriter writer = new(caminhoCompleto);

            writer.WriteLine("======= COMPROVANTE DE RESERVA =======");
            writer.WriteLine($"Espaço reservado: {reserva.Espaco.NomeFantasia}");
            writer.WriteLine($"Capacidade: {reserva.Espaco.Capacidade} pessoa(s)");
            writer.WriteLine($"Valor da diária: {reserva.Espaco.ValorDiaria:C}");
            writer.WriteLine();
            writer.WriteLine($"Check-in: {reserva.DataCheckIn:dd/MM/yyyy}");
            writer.WriteLine($"Duração: {reserva.DiasReservados} diária(s)");
            writer.WriteLine($"Check-out: {reserva.DataCheckIn.AddDays(reserva.DiasReservados):dd/MM/yyyy}");
            writer.WriteLine();
            writer.WriteLine($"Quantidade de hóspedes: {reserva.Hospedes.Count}");
            writer.WriteLine("Hóspedes:");
            foreach (var h in reserva.Hospedes)
            {
                writer.WriteLine($"- {h.NomeCompleto} | {h.Email}");
            }
            writer.WriteLine();
            writer.WriteLine($"Valor total da reserva: {reserva.CalcularValorDiaria():C}");
            writer.WriteLine("======================================");
            writer.WriteLine("Gerado automaticamente pelo sistema.");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\nComprovante salvo como: {nomeArquivo}");
            Console.ResetColor();
        }
        private static void AdicionarHospede(List<Hospede> hospedesCadastrados, List<Hospede> hospedesDaReserva)
        {
            Console.Clear();
            Console.WriteLine("=== ADICIONAR HÓSPEDE À RESERVA ===\n");

            string? nomeCompleto = EntradaHelper.LerTextoComCancelamento("Digite o nome completo do hóspede");
            if (nomeCompleto == null)
            {
                Console.WriteLine("\nAção cancelada. Retornando ao menu anterior...");
                return;
            }

            nomeCompleto = nomeCompleto.Trim().ToUpperInvariant();

            var existente = hospedesCadastrados
                .FirstOrDefault(h => h.NomeCompleto.ToUpperInvariant() == nomeCompleto);

            if (existente != null)
            {
                Console.WriteLine("\nHóspede já cadastrado. Adicionando à reserva...");
                hospedesDaReserva.Add(existente);
            }
            else
            {
                var partes = nomeCompleto.Split(' ');
                string nome = partes.First();
                string sobrenome = partes.Length > 1 ? string.Join(" ", partes.Skip(1)) : "(sem sobrenome)";

                string? email;
                do
                {
                    email = EntradaHelper.LerTextoComCancelamento("Informe o e-mail");
                    if (email == null) return;

                    if (!email.Contains("@") || !email.Contains(".") || email.StartsWith("@") || email.EndsWith("@"))
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("E-mail inválido. Tente novamente (ex: nome@dominio.com).");
                        Console.ResetColor();
                        email = null;
                    }

                } while (email == null);

                var novoHospede = new Hospede(nome, sobrenome, email);
                hospedesCadastrados.Add(novoHospede);
                hospedesDaReserva.Add(novoHospede);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nNovo hóspede cadastrado e vinculado à reserva.");
                Console.ResetColor();
            }
        }
        private static void SubMenuHospedes(Reserva reservaSelecionada, List<Hospede> hospedesCadastrados)
        {
            string? opcao;

            do
            {
                Console.Clear();
                Console.WriteLine($"=== HÓSPEDES DA RESERVA EM {reservaSelecionada.Espaco.NomeFantasia} ===\n");
                Console.WriteLine("1 - Adicionar hóspede à reserva");
                Console.WriteLine("2 - Ver hóspedes vinculados");
                Console.WriteLine("0 - Voltar");
                Console.Write("\nEscolha uma opção: ");
                opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        AdicionarHospede(hospedesCadastrados, reservaSelecionada.Hospedes);
                        break;
                    case "2":
                        if (reservaSelecionada.Hospedes.Any())
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("\nHóspedes atualmente na reserva:");
                            Console.ResetColor();

                            foreach (var h in reservaSelecionada.Hospedes)
                            {
                                Console.WriteLine($"- {h.NomeCompleto} ({h.Email})");
                            }
                        }

                        else
                        {
                            Console.WriteLine("\nNenhum hóspede ainda foi adicionado.");
                        }
                        Console.WriteLine("\nPressione qualquer tecla para continuar...");
                        Console.ReadKey();
                        break;
                    case "0":
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\nOpção inválida.");
                        Console.ResetColor();
                        Console.ReadKey();
                        break;
                }

            } while (opcao != "0");
        }

        private static void ListarReservasAtivas(List<Reserva> reservas)
        {
            Console.Clear();
            Console.WriteLine("=== RESERVAS ATIVAS ===\n");

            var ativas = reservas.Where(r => r.Ativa).ToList();

            if (!ativas.Any())
            {
                Console.WriteLine("Nenhuma reserva ativa no momento.");
            }
            else
            {
                foreach (var r in ativas)
                {
                    var checkOut = r.DataCheckIn.AddDays(r.DiasReservados);
                    Console.WriteLine($"• Espaço: {r.Espaco.NomeFantasia} (ID {r.Espaco.Id})");
                    Console.WriteLine($"  Check-in: {r.DataCheckIn:dd/MM/yyyy}");
                    Console.WriteLine($"  Check-out previsto: {checkOut:dd/MM/yyyy}");
                    Console.WriteLine($"  Hóspedes: {string.Join(", ", r.Hospedes.Select(h => h.NomeCompleto))}");
                    Console.WriteLine($"  Valor estimado: R$ {r.CalcularValorDiaria():F2}\n");
                }
            }

            Console.WriteLine("Pressione qualquer tecla para continuar...");
            Console.ReadKey();
        }

        private static void CancelarReserva(List<Reserva> reservas)
        {
            Console.Clear();
            Console.WriteLine("=== CANCELAMENTO DE RESERVA ===\n");

            var ativas = reservas.Where(r => r.Ativa).ToList();

            if (!ativas.Any())
            {
                Console.WriteLine("Nenhuma reserva ativa para cancelar.");
                Console.ReadKey();
                return;
            }

            for (int i = 0; i < ativas.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {ativas[i].Espaco.NomeFantasia} | Check-in: {ativas[i].DataCheckIn:dd/MM/yyyy}");
            }

            int? escolha = EntradaHelper.LerInteiroComCancelamento("\nEscolha uma reserva pelo número");
            if (escolha == null || escolha < 1 || escolha > ativas.Count)
            {
                Console.WriteLine("\nCancelamento abortado. Retornando ao menu...");
                return;
            }

            var selecionada = ativas[escolha.Value - 1];
            selecionada.Ativa = false;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nReserva cancelada com sucesso.");
            Console.ResetColor();
            Console.WriteLine("\nPressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
        private static void EncerrarReserva(List<Reserva> reservas)
        {
            Console.Clear();
            Console.WriteLine("=== ENCERRAMENTO DE RESERVA ===\n");

            var ativas = reservas.Where(r => r.Ativa).ToList();

            if (!ativas.Any())
            {
                Console.WriteLine("Nenhuma reserva ativa para encerrar.");
                Console.ReadKey();
                return;
            }

            for (int i = 0; i < ativas.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {ativas[i].Espaco.NomeFantasia} | Check-in: {ativas[i].DataCheckIn:dd/MM/yyyy}");
            }

            int? escolha = EntradaHelper.LerInteiroComCancelamento("\nEscolha uma reserva para encerrar");
            if (escolha == null || escolha < 1 || escolha > ativas.Count)
            {
                Console.WriteLine("\nEncerramento cancelado. Retornando ao menu...");
                return;
            }

            var encerrada = ativas[escolha.Value - 1];
            encerrada.Ativa = false;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nReserva encerrada com sucesso.");
            Console.WriteLine($"Valor final: R$ {encerrada.CalcularValorDiaria():F2}");
            Console.ResetColor();

            Console.WriteLine("\nPressione qualquer tecla para continuar...");
            Console.ReadKey();
        }

    }
}
