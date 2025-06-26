using System;
using System.Linq;
using System.Collections.Generic;
using DesafioProjetoHospedagem.Models;
using DesafioProjetoHospedagem.Utils;

namespace Menus
{
    public static class MenuEspacos
    {
        public static void Exibir(List<EspacoHospedagem> espacos, List<Reserva> reservas)
        {
            int opcao;

            do
            {
                Console.Clear();
                Console.WriteLine("========================================");
                Console.WriteLine("\n    SISTEMA DE HOSPEDAGEM | HOTEL FICTÍCIO");
                Console.WriteLine($"    Data: {DateTime.Now:dd/MM/yyyy}     | Hora: {DateTime.Now:HH:mm:ss}");
                Console.WriteLine("========================================");
                Console.WriteLine("\n------------ MENU - ESPAÇOS ------------\n");
                Console.WriteLine("1 - Cadastrar novo espaço");
                Console.WriteLine("2 - Listar espaços cadastrados");
                Console.WriteLine("3 - Excluir espaço");
                Console.WriteLine("4 - Ver status dos espaços (ocupado/disponível)");
                Console.WriteLine("0 - Voltar");
                Console.Write("\nEscolha uma opção: ");

                if (!int.TryParse(Console.ReadLine(), out opcao))
                    opcao = -1;

                switch (opcao)
                {
                    case 1:
                        CadastrarNovoEspaco(espacos);
                        break;
                    case 2:
                        ListarEspacos(espacos);
                        break;
                    case 3:
                        ExcluirEspaco(espacos, reservas);
                        break;
                    case 4:
                        ListarEspacosComStatus(espacos, reservas);
                        break;
                    case 0:
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\nOpção inválida. Pressione qualquer tecla para tentar novamente...");
                        Console.ResetColor();
                        Console.ReadKey();
                        break;
                }

            } while (opcao != 0);
        }

        private static void CadastrarNovoEspaco(List<EspacoHospedagem> espacos)
        {
            Console.Clear();
            Console.WriteLine("==== CADASTRO DE NOVO ESPAÇO ====\n");

            string[] opcoesTipo = { "Chalé", "Cabana", "Apartamento", "Casa", "Suíte", "Quarto", "Loft", "Outro" };

            for (int i = 0; i < opcoesTipo.Length; i++)
                Console.WriteLine($"{i + 1} - {opcoesTipo[i]}");

            int escolhaTipo;
            do
            {
                Console.Write("\nSelecione o tipo de espaço: ");
            } while (!int.TryParse(Console.ReadLine(), out escolhaTipo) || escolhaTipo < 1 || escolhaTipo > opcoesTipo.Length);

            string tipoEspaco = opcoesTipo[escolhaTipo - 1];
            int numeroDoTipo = espacos.Count(e => e.TipoEspaco == tipoEspaco) + 1;
            string nomeSugerido = $"{tipoEspaco} {numeroDoTipo}";

            string usarNome;
            int tentativasVazias = 0;
            do
            {
                Console.Write($"\nDeseja usar o nome sugerido \"{nomeSugerido}\"? (S/N): ");
                usarNome = Console.ReadLine()!.Trim().ToUpper();

                if (string.IsNullOrEmpty(usarNome))
                {
                    tentativasVazias++;
                    if (tentativasVazias >= 3)
                    {
                        Console.Write("Você pressionou Enter várias vezes. Deseja cancelar e voltar ao menu? (S/N): ");
                        string voltar = Console.ReadLine()!.Trim().ToUpper();
                        if (voltar == "S")
                        {
                            Console.WriteLine("\nRetornando ao menu...");
                            return;
                        }
                        else
                        {
                            tentativasVazias = 0;
                        }
                    }
                }
            } while (usarNome != "S" && usarNome != "N");

            string? nomeEspaco = (usarNome == "S")
                ? nomeSugerido
                : EntradaHelper.LerTextoComCancelamento($"Digite o nome fantasia do {tipoEspaco.ToUpper()}");

            if (nomeEspaco == null) return;

            int? capacidade = EntradaHelper.LerInteiroComCancelamento("Informe a capacidade de hóspedes");
            if (capacidade == null) return;

            decimal? diaria = EntradaHelper.LerDecimalComCancelamento("Informe o valor da diária (ex: 85.90)");
            if (diaria == null) return;

            int? quartos = EntradaHelper.LerInteiroComCancelamento("Quantidade de quartos");
            if (quartos == null) return;

            int? suites = EntradaHelper.LerInteiroComCancelamento("Quantos desses quartos possuem suíte?");
            if (suites == null) return;

            string? descricao = EntradaHelper.LerTextoComCancelamento("Digite uma breve descrição para este espaço");
            if (descricao == null) return;

            bool temSuite = suites > 0;

            EspacoHospedagem novoEspaco = new EspacoHospedagem(
                id: espacos.Count + 1,
                tipoEspaco: tipoEspaco,
                nomeFantasia: nomeEspaco,
                capacidade: capacidade.Value,
                valorDiaria: diaria.Value,
                quantidadeQuartos: quartos.Value,
                possuiSuite: temSuite,
                descricao: descricao
            );

            espacos.Add(novoEspaco);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n{novoEspaco.Id:D3}-  {novoEspaco.TipoEspaco.ToUpper()} {novoEspaco.NomeFantasia} . Cadastro realizado com sucesso!");
            Console.ResetColor();

            Console.WriteLine("Pressione qualquer tecla para voltar ao menu...");
            Console.ReadKey();
        }


        /*
        private static void CadastrarNovoEspaco(List<EspacoHospedagem> espacos)
        {
            Console.Clear();
            Console.WriteLine("==== CADASTRO DE NOVO ESPAÇO ====\n");

            string[] opcoesTipo = { "Chalé", "Cabana", "Apartamento", "Casa", "Suíte", "Quarto", "Loft", "Outro" };

            for (int i = 0; i < opcoesTipo.Length; i++)
                Console.WriteLine($"{i + 1} - {opcoesTipo[i]}");

            int escolhaTipo;
            do
            {
                Console.Write("\nSelecione o tipo de espaço: ");
            } while (!int.TryParse(Console.ReadLine(), out escolhaTipo) || escolhaTipo < 1 || escolhaTipo > opcoesTipo.Length);

            string tipoEspaco = opcoesTipo[escolhaTipo - 1];
            int numeroDoTipo = espacos.Count(e => e.TipoEspaco == tipoEspaco) + 1;
            string nomeSugerido = $"{tipoEspaco} {numeroDoTipo}";

            Console.Write($"\nDeseja usar o nome sugerido \"{nomeSugerido}\"? (S/N): ");
            string usarNome = Console.ReadLine()!.Trim().ToUpper();

            string nomeEspaco = (usarNome == "S")
            ? nomeSugerido
            : EntradaHelper.LerTexto($"Digite o nome fantasia do {tipoEspaco.ToUpper()}:");

            int capacidade = EntradaHelper.LerInteiro("Informe a capacidade de hóspedes");
            decimal diaria = EntradaHelper.LerDecimal("Informe o valor da diária (ex: 85.90)");
            int quartos = EntradaHelper.LerInteiro("Quantidade de quartos");
            int suites = EntradaHelper.LerInteiro("Quantos desses quartos possuem suíte?");
            string descricao = EntradaHelper.LerTexto("Digite uma breve descrição para este espaço");

            bool temSuite = suites > 0;

            EspacoHospedagem novoEspaco = new EspacoHospedagem(
                id: espacos.Count + 1,
                tipoEspaco: tipoEspaco,
                nomeFantasia: nomeEspaco,
                capacidade: capacidade,
                valorDiaria: diaria,
                quantidadeQuartos: quartos,
                possuiSuite: temSuite,
                descricao: descricao
            );

            espacos.Add(novoEspaco);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n✅ {novoEspaco.Id:D3}-  {novoEspaco.TipoEspaco.ToUpper()} {novoEspaco.NomeFantasia} cadastrada com sucesso!");
            Console.ResetColor();

            Console.WriteLine("Pressione qualquer tecla para voltar ao menu...");
            Console.ReadKey();
        }
        */


        private static void ListarEspacos(List<EspacoHospedagem> espacos)
        {
            Console.Clear();
            Console.WriteLine("==== LISTA DE ESPAÇOS ====\n");

            if (!espacos.Any())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Ainda não há espaços cadastrados.");
                Console.ResetColor();
            }
            else
            {
                foreach (var espaco in espacos
                    .OrderBy(e => e.TipoEspaco)
                    .ThenBy(e => e.Id))
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"[{espaco.Id:00}] {espaco.TipoEspaco.ToUpper()} - {espaco.NomeFantasia}");
                    Console.ResetColor();
                    Console.WriteLine($"Capacidade: até {espaco.Capacidade} pessoas");
                    Console.WriteLine($"Diária: {espaco.ValorDiaria:C}\n");
                }
            }

            Console.WriteLine("Pressione qualquer tecla para voltar...");
            Console.ReadKey();
        }
        private static void ListarEspacosComStatus(List<EspacoHospedagem> espacos, List<Reserva> reservas)
        {
            Console.Clear();
            Console.WriteLine("=== STATUS DOS ESPAÇOS ===\n");

            if (!espacos.Any())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Nenhum espaço cadastrado no sistema.");
                Console.ResetColor();
            }
            else
            {
                foreach (var espaco in espacos.OrderBy(e => e.TipoEspaco).ThenBy(e => e.Id))
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"{espaco.TipoEspaco.ToUpper()} - {espaco.NomeFantasia} (ID {espaco.Id:00})");

                    Console.ResetColor();

                    var reserva = reservas.FirstOrDefault(r => r.Ativa && r.Espaco.Id == espaco.Id);
                    if (reserva == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Situação: Disponível para reserva\n");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Situação: Ocupado 🔒");
                        Console.ResetColor();
                        Console.WriteLine($" - Check-in: {reserva.DataCheckIn:dd/MM/yyyy}");
                        Console.WriteLine($" - Dias reservados: {reserva.DiasReservados}");
                        Console.WriteLine($" - Hóspedes:");
                        foreach (var h in reserva.Hospedes)
                            Console.WriteLine($"    • {h.NomeCompleto}");
                        Console.WriteLine();
                    }
                }
            }

            Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
            Console.ReadKey();
        }

        private static void ExcluirEspaco(List<EspacoHospedagem> espacos, List<Reserva> reservas)
        {
            Console.Clear();
            Console.WriteLine("=== EXCLUSÃO DE ESPAÇO ===\n");

            if (!espacos.Any())
            {
                Console.WriteLine("Nenhum espaço cadastrado.");
                Console.ReadKey();
                return;
            }

            foreach (var e in espacos.OrderBy(e => e.TipoEspaco).ThenBy(e => e.Id))
            {
                Console.WriteLine($"[ID {e.Id}] {e.TipoEspaco} - {e.NomeFantasia} | Capacidade: {e.Capacidade} | Diária: {e.ValorDiaria:C}");
            }

            Console.Write("\nDigite o ID do espaço que deseja excluir: ");
            if (!int.TryParse(Console.ReadLine(), out int idSelecionado))
            {
                Console.WriteLine("Entrada inválida.");
                Console.ReadKey();
                return;
            }

            var espacoSelecionado = espacos.FirstOrDefault(e => e.Id == idSelecionado);
            if (espacoSelecionado == null)
            {
                Console.WriteLine("Espaço não encontrado.");
                Console.ReadKey();
                return;
            }

            bool ocupado = reservas.Any(r => r.Ativa && r.Espaco.Id == espacoSelecionado.Id);
            if (ocupado)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nEste espaço está com uma reserva ativa e não pode ser excluído.");
                Console.ResetColor();
            }
            else
            {
                Console.Write($"\nTem certeza que deseja excluir o espaço \"{espacoSelecionado.NomeFantasia}\"? (S/N): ");
                string? resposta = Console.ReadLine()?.Trim().ToUpper();

                if (resposta == "S")
                {
                    espacos.Remove(espacoSelecionado);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nEspaço excluído com sucesso!");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine("\nOperação cancelada. Nenhuma alteração foi feita.");
                }
            }

            Console.WriteLine("\nPressione qualquer tecla para retornar ao menu...");
            Console.ReadKey();
        }
    }
}
