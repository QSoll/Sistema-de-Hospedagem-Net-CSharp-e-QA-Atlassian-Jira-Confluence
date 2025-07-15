using System;
using System.Collections.Generic;
using DesafioProjetoHospedagem.Models;
using DesafioProjetoHospedagem.Utils;
using DesafioProjetoHospedagem.Controladoras;
using DesafioProjetoHospedagem.Sessao;
using DesafioProjetoHospedagem.Menus;

namespace DesafioProjetoHospedagem.Menus
{
    public static class MenuEspacos
    {
        public static Reserva? ExibirMenuEspacos()
        {
            //menu principal está aqui
            while (true)
            {
                Console.Clear();
                CentralSaida.CabecalhoMenus("Menu de Espaços");

                Console.WriteLine("A partir de 10 dias: 10 % de desconto.");
                Console.WriteLine("Ultrapassado o tempo da diária será cobrado uma taxa de R$ 5,00 por hora.");
                Console.WriteLine("");
                Console.WriteLine("1- Cadastrar");
                Console.WriteLine("2- Listar");
                Console.WriteLine("3- Editar");
                Console.WriteLine("4- Excluir");

                CentralSaida.LinhaSeparadora('─');

                int? opcao = CentralEntradaControladora.LerInteiroMinMaxComSN(
                "Escolha: 8.Voltar   9.Menu principal   0.Encerrar",
                1,
                4,
                "Menu – Espaços",
                () => MenuEspacos.ExibirMenuEspacos(),

                () => MenuPrincipal.ExibirPrincipal()
                );

                Console.WriteLine();

                switch (opcao)
                {
                    case 1: //cadastrando
                        NavegacaoAtual.Registrar(() => MenuEspacos.ExibirMenuEspacos());
                        EspacoControlador.CadastrarEspaco();
                        break;

                    case 2: //indo para STATUS DO ESPACO
                        NavegacaoAtual.Registrar(() => MenuEspacos.ExibirMenuEspacos());
                        MenuEspacos.ListarEspacosGlobal("Padrao");
                        break;

                    case 3:
                        NavegacaoAtual.Registrar(() => MenuEspacos.ExibirMenuEspacos());
                        EspacoControlador.EditarCadastroEspaco();
                        break;

                    case 4:
                        NavegacaoAtual.Registrar(() => MenuEspacos.ExibirMenuEspacos());
                        EspacoControlador.ExcluirCadastroEspaco();
                        break;

                    case 8:
                        return SessaoAtual.ReservaSelecionada;

                    case 9:
                        MenuPrincipal.ExibirPrincipal();
                        return SessaoAtual.ReservaSelecionada;

                    case 0:
                        CentralSaida.ExibirDespedidaSimples();
                        Environment.Exit(0);
                        break;

                    default:
                        CentralSaida.Exibir(TipoMensagem.Erro, "Ops. Opção inválida.");
                        break;
                }

                MenuRodape.ExibirRodape(
                "Menu – Espaços",
                () => MenuEspacos.ExibirMenuEspacos(),
                () => MenuPrincipal.ExibirPrincipal()
                );

            }
        }

        public static void ListarEspacosGlobal(string origemChamada = "Padrao")
        {
            Console.Clear();

            CentralSaida.CabecalhoMenus("Catálogo de Espaços");

            if (!SessaoAtual.Espacos.Any())
            {
                CentralSaida.Exibir(TipoMensagem.Alerta, "Nenhum espaço disponível.");

                bool desejaCadastrar = CentralEntradaControladora.SimNao("Deseja cadastrar agora?");
                if (desejaCadastrar)
                {
                    EspacoControlador.CadastrarEspaco();
                    ListarEspacosGlobal(); // volta à tela atual
                }
                else
                {
                    MenuRodapeGlobal.ExibirRodapeGlobal("Catálogo de Espaços");
                }
                return;
            }

            var espacosAgrupados = SessaoAtual.Espacos

                .GroupBy(e => e.TipoEspaco.Trim().ToUpper())
                //GroupBy(e => e.TipoEspaco.ToUpper())
                .OrderBy(g => g.Key);

            foreach (var grupo in espacosAgrupados)
            {
                string tipoEspaco = grupo.Key;

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(new string('─', Console.WindowWidth));
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(tipoEspaco.PadLeft((Console.WindowWidth + tipoEspaco.Length) / 2));
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(new string('─', Console.WindowWidth));
                Console.ResetColor();
                Console.WriteLine();

                int contadorVisual = 1;

                foreach (var espaco in grupo)
                {
                    string idVisual = $"{espaco.TipoAbreviado} {contadorVisual:D3}";
                    contadorVisual++;

                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write(idVisual);
                    Console.ResetColor();
                    Console.WriteLine($" - {espaco.NomeFantasia}");

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("Capacidade: ");
                    Console.ResetColor();
                    Console.WriteLine($"{espaco.Capacidade} pessoa{(espaco.Capacidade > 1 ? "s" : "")}");

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("Diária: ");
                    Console.ResetColor();
                    Console.WriteLine($"R$ {espaco.ValorDiaria:N2}");

                    Console.Write($"{espaco.QuantidadeQuartos} quarto{(espaco.QuantidadeQuartos > 1 ? "s" : "")}, sendo ");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write($"{espaco.QuantidadeSuites}");
                    Console.ResetColor();
                    Console.WriteLine($" suíte{(espaco.QuantidadeSuites > 1 ? "s" : "")}.");

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("Descrição: ");
                    Console.ResetColor();
                    Console.WriteLine(string.IsNullOrWhiteSpace(espaco.Descricao) ? "Não informada." : espaco.Descricao);

                    Console.WriteLine();
                    Console.WriteLine("DETALHES");
                    Console.WriteLine();

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("Status: ");
                    Console.ResetColor();
                    Console.WriteLine(string.IsNullOrWhiteSpace(espaco.Status) ? "Indefinido" : espaco.Status);

                    var reserva = SessaoAtual.Reservas.FirstOrDefault(r => r.Ativa && r.Espaco?.Id == espaco.Id);

                    if (reserva != null)
                    {
                        var resumo = reserva.CalcularResumoFinanceiro();
                        ReservaControladora.VisualizadorFinanceiro.ExibirResumoFinanceiro(resumo);

                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("Hóspedes:");
                        Console.ResetColor();

                        if (reserva.Clientes.Any())
                        {
                            int i = 1;
                            foreach (var cliente in reserva.Clientes)
                                Console.WriteLine($"{i++}- {cliente.Nome}");
                        }
                        else
                        {
                            Console.WriteLine("Ainda não cadastrados");
                        }
                    }
                }

                Console.WriteLine();
            }

            Console.WriteLine();

            bool desejaEscolher = CentralEntradaControladora.SimNao("Deseja selecionar um espaço para continuar alguma ação?");
            if (desejaEscolher)
            {
                EspacoHospedagem espacoSelecionado = EspacoControlador.BuscarEspacoPorFiltro();

                if (espacoSelecionado != null)
                {
                    Reserva novaReserva = new Reserva(new List<Cliente>(), espacoSelecionado, 1);

                    if (origemChamada != "Padrao")
                    {
                        CentralSaida.Exibir(TipoMensagem.Sucesso, $"Espaço vinculado à ação existente.");

                        switch (origemChamada)
                        {
                            case "CriarReserva": CriarReservaControladora.ContinuarCriacao(novaReserva); break;
                            case "EditarReserva": EditarReservaControladora.ContinuarEdicao(novaReserva); break;
                            case "ExcluirReserva": MenuReservas.ExcluirCadastroReserva(); break;
                            case "CancelarReserva": MenuReservas.CancelarReserva(); break;
                            case "ListarCadastroReservas": MenuReservas.ExibirMenuReservas(); break;
                            default:
                                CriarReservaControladora.IniciarComEspacoVinculado(espacoSelecionado);

                                break;
                        }
                    }
                    else
                    {
                        CentralSaida.Exibir(TipoMensagem.Informacao, $"Espaço \"{espacoSelecionado.NomeFantasia}\" selecionado.");

                        while (true)
                        {
                            Console.WriteLine("\nO que deseja fazer agora?");
                            Console.WriteLine("1 - Cadastrar novo espaço");
                            Console.WriteLine("2 - Editar Espaço");
                            Console.WriteLine("3 - Excluir Espaço");
                            Console.WriteLine("4 - Criar Reserva");

                            string? escolha = Console.ReadLine()?.Trim();

                            switch (escolha)
                            {
                                case "1":
                                    NavegacaoAtual.Registrar(() => MenuEspacos.ListarEspacosGlobal());
                                    EspacoControlador.CadastrarEspaco();
                                    return;

                                case "2":
                                    EspacoControlador.EditarCadastroEspaco();
                                    return;

                                case "3":
                                    EspacoControlador.ExcluirCadastroEspaco();
                                    return;

                                case "4":
                                    CriarReservaControladora.IniciarComEspacoVinculado(espacoSelecionado);

                                    return;

                                default:
                                    CentralSaida.Exibir(TipoMensagem.Alerta, "Opção inválida. Por favor, digite novamente.");
                                    Console.WriteLine();
                                    break;
                            }
                        }
                    }
                }
            }
            else
            {
                MenuRodapeGlobal.ExibirRodapeGlobal("Catálogo de Espaços");
            }
        }

        private static string GerarCodigoTipo(string tipoNome, HashSet<string> codigosUsados)
        {
            tipoNome = RemoverAcentos(tipoNome.ToUpper().Replace(" ", ""));
            string prefixo = tipoNome.Length >= 3 ? tipoNome.Substring(0, 3) : tipoNome;

            var letrasUsadas = codigosUsados
                .Where(c => c.StartsWith(prefixo) && c.Length == 4)
                .Select(c => c[3]) // quarta letra usada no código
                .ToHashSet();

            // Tenta usar letras do próprio nome que ainda não foram usadas
            foreach (char letra in tipoNome.Skip(3))
            {
                if (!letrasUsadas.Contains(letra))
                {
                    string tentativa = prefixo + letra;
                    codigosUsados.Add(tentativa);
                    return tentativa;
                }
            }

            // Se todas as letras da palavra foram usadas, tenta letra aleatória
            foreach (char letra in "ABCDEFGHIJKLMNOPQRSTUVWXYZ")
            {
                if (!letrasUsadas.Contains(letra))
                {
                    string tentativa = prefixo + letra;
                    codigosUsados.Add(tentativa);
                    return tentativa;
                }
            }

            // Último recurso (caso todas opções estejam usadas)
            return $"{prefixo}X";
        }

        //Removeracentos: código auxiliar de GerarCodigoTipo
        private static string RemoverAcentos(string texto)
        {
            var bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(texto);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }

        public static EspacoHospedagem SelecionarEspaco()
        {
            CentralSaida.LimparTela("Selecionar Espaço");
            CentralSaida.CabecalhoMenus("Seleção de Espaço");

            if (!SessaoAtual.Espacos.Any())
            {
                CentralSaida.Exibir(TipoMensagem.Alerta, "Nenhum espaço disponível.");
                EntradaHelper.ExibirMensagemRetorno("Pressione ENTER para retornar...");
                return null!;
            }

            for (int i = 0; i < SessaoAtual.Espacos.Count; i++)
            {
                var espaco = SessaoAtual.Espacos[i];
                Console.WriteLine($"[{i + 1}] {espaco.NomeFantasia} – Diária: {espaco.ValorDiaria:C}");
            }

            int? escolha = CentralEntradaInteira.LerComCancelamento("Digite o número do espaço desejado:");

            if (escolha == null || escolha < 1 || escolha > SessaoAtual.Espacos.Count)
            {
                CentralSaida.Exibir(TipoMensagem.Alerta, "Seleção cancelada ou inválida.");
                return null!;
            }

            return SessaoAtual.Espacos[escolha.Value - 1];
        }


    }


}