using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Security.AccessControl;
using DesafioProjetoHospedagem.Menus;
using DesafioProjetoHospedagem.Models;
using DesafioProjetoHospedagem.Utils;
using DesafioProjetoHospedagem.Controladoras;
using DesafioProjetoHospedagem.Sessao;

//atualizado com: navegacao reversa, NavegacaoAtual e menurodapeGlobal
namespace DesafioProjetoHospedagem.Menus
{
    public static class MenuReservas
    {
        public static void ExibirMenuReservas() // SessaoAtual.ReservaSelecionada não está sendo usado neste menu, mas mantido por compatibilidade
        {

            // ➤ Registrar para navegação reversa
            NavegacaoAtual.Registrar(() => MenuPrincipal.ExibirPrincipal());
            SessaoAtual.VerificarSessaoGlobal();

            bool continuar = true;

            while (continuar)
            {
                CentralSaida.LimparTela("Menu de Reservas");
                CentralSaida.CabecalhoMenus("MENU RESERVAS");
                CentralSaida.Subtitulo("Reservas");

                Console.WriteLine("\n=== RESERVAS ===\n");
                Console.WriteLine("1 - Criar");
                Console.WriteLine("2 - Listar");
                Console.WriteLine("3 - Cancelar");
                Console.WriteLine("4 - Excluir");

                int opcao = CentralEntradaControladora.LerInteiroMinMaxComSN(
                "Escolha uma opção",
                1,
                4,
                "Menu | Reservas", // título do rodapé
                () => MenuReservas.ExibirMenuReservas(),
                () => MenuPrincipal.ExibirPrincipal()
                );

                switch (opcao)
                {
                    case 1:
                        CriarReserva();
                        break;

                    case 2:
                        ListarCadastroReservas();
                        break;

                    case 3:
                        CancelarReserva();
                        break;

                    case 4:
                        ExcluirCadastroReserva();
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Opção inválida. Tente novamente.");
                        Console.ResetColor();
                        continue;
                }

            }
            MenuRodape.ExibirRodape(
            "Menu | Relatórios",
            aoVoltarParaSubmenu: () => MenuReservas.ExibirMenuReservas(),
            aoVoltarMenuPrincipal: () => MenuPrincipal.ExibirPrincipal()
            );

        }

        // Passando novaReserva para compatibilidade com a assinatura de ExibirMenuReservas (SessaoAtual.ReservaSelecionada)

        public static void CriarReserva()
        {
            NavegacaoAtual.Registrar(() => CriarReserva());

            CentralSaida.LimparTela("CADASTRO DE RESERVA");
            CentralSaida.CabecalhoMenus("Cadastro de Reservas");

            CentralSaida.Exibir(TipoMensagem.Instrucao,
                "Reservas com mais de 10 dias têm desconto de 10%.\n" +
                "Ultrapassando o horário de saída, será cobrada uma taxa de R$5,00 por hora.");

            //Verifica se há espaços disponíveis
            var espacosDisponiveis = SessaoAtual.Espacos
                .Where(e => SessaoAtual.Reservas.All(r => r.Espaco.Id != e.Id || !r.Ativa))
                .ToList();

            while (!espacosDisponiveis.Any())
            {
                CentralSaida.Exibir(TipoMensagem.Alerta, "Nenhum espaço disponível. Você precisa cadastrar ao menos um.");
                EspacoControlador.CadastrarEspaco();
                espacosDisponiveis = SessaoAtual.Espacos.ToList();
            }

            NavegacaoAtual.Registrar(() => CriarReserva());
            MenuEspacos.ListarEspacosGlobal("CriarReserva"); // ← usa seu método padronizado

            int escolhaEspaco = CentralEntradaControladora.LerInteiroMinMax("Selecione o espaço desejado", 1, espacosDisponiveis.Count);
            //r espacoSelecionado = espacosDisponiveis[escolhaEspaco - 1];
            var espacoSelecionado = SessaoAtual.EspacoSelecionado ?? espacosDisponiveis[escolhaEspaco - 1];
            SessaoAtual.EspacoSelecionado = null; // limpa depois de usar


            CentralSaida.ExibirDestaque($"Espaço selecionado: {espacoSelecionado.NomeFantasia} | Capacidade: {espacoSelecionado.Capacidade}");

            //Cadastro de clientes durante reserva
            var clientesSelecionados = new List<Cliente>();
            bool cadastrarAgora = CentralEntradaControladora.SimNao("Deseja cadastrar os hóspedes agora?");

            if (cadastrarAgora)
            {
                while (clientesSelecionados.Count < espacoSelecionado.Capacidade)
                {
                    string nome = CentralEntrada.LerTextoObrigatorio(
                    "Nome completo do hóspede");

                    string telefone = CentralEntrada.LerTextoObrigatorio(
                    "Telefone do hóspede");


                    var clienteEncontrado = SessaoAtual.Clientes.FirstOrDefault(c => c.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase));
                    if (clienteEncontrado != null)
                    {
                        clientesSelecionados.Add(clienteEncontrado);
                        CentralSaida.Exibir(TipoMensagem.Sucesso, $"Cliente {clienteEncontrado.Nome} vinculado à reserva.");
                    }
                    else
                    {
                        CentralSaida.Exibir(TipoMensagem.Alerta, "Cliente não encontrado. Vamos cadastrá-lo agora.");
                        Cliente novoCliente = MenuClientes.CadastrarCliente();
                        SessaoAtual.Clientes.Add(novoCliente);
                        clientesSelecionados.Add(novoCliente);
                        CentralSaida.Exibir(TipoMensagem.Sucesso, $"Cliente {novoCliente.Nome} cadastrado e vinculado.");
                    }

                    if (clientesSelecionados.Count < espacoSelecionado.Capacidade)
                        cadastrarAgora = CentralEntradaControladora.SimNao("Deseja adicionar outro hóspede?");
                    else
                        CentralSaida.Exibir(TipoMensagem.Informativo, "Capacidade máxima atingida.");
                }
            }
            else
            {
                CentralSaida.Exibir(TipoMensagem.Informativo, "Ok, vincule os hóspedes depois.");
            }

            //Dias e data da reserva
            int dias = CentralEntradaControladora.LerInteiroMinMax("Quantidade de dias reservados", 1, 30);

            DateTime dataCheckIn = CentralEntrada.LerDataObrigatoria("Data do check-in");

            DateTime dataCheckOut = dataCheckIn.AddDays(dias);

            //Criação da reserva
            var novaReserva = new Reserva(clientesSelecionados, espacoSelecionado, dias)
            {
                DataCheckIn = dataCheckIn,
                DataCheckOut = dataCheckOut
            };

            SessaoAtual.Reservas.Add(novaReserva);
            SessaoAtual.ReservaSelecionada = novaReserva;

            // Exibe resumo visual
            CentralSaida.LimparTela("RESERVA CRIADA");
            Console.WriteLine($"Check-in: {novaReserva.DataCheckIn:dd/MM/yyyy}");
            Console.WriteLine($"Check-out estimado: {novaReserva.DataCheckOut:dd/MM/yyyy}");
            Console.WriteLine($"Espaço: {espacoSelecionado.NomeFantasia}");
            Console.WriteLine($"Valor base diária: {espacoSelecionado.ValorDiaria:C}");

            Console.WriteLine($"Sem desconto: {novaReserva.CalcularValorSemDesconto():C}");
            Console.WriteLine($"Desconto aplicado: {novaReserva.CalcularDesconto():C}");
            Console.WriteLine($"Taxa de acréscimo: {novaReserva.CalcularTaxaAcrescimo():C}");
            Console.WriteLine($"Valor total estimado até o momento: {novaReserva.CalcularValorTotal():C}");

            // Lista de hóspedes
            if (novaReserva.Clientes.Any())
            {
                Console.WriteLine("\nHóspedes:");
                foreach (var c in novaReserva.Clientes)
                    Console.WriteLine($"- {c.Nome}");
            }
            else
            {
                Console.WriteLine("\nHóspedes: cadastrar depois.");
            }

            // Mensagem final + rodapé
            CentralSaida.Exibir(TipoMensagem.Sucesso, "Sucesso! A reserva foi criada.");

            bool desejaComprovante = CentralEntradaControladora.SimNao("Deseja gerar o comprovante da reserva?");
            if (desejaComprovante)
            {
                MenuComprovantes.ExibirMenuComprovantes();
            }

            MenuRodape.ExibirRodape("Reserva Concluída");

        }

        private static void ExibirSubMenuClientes()
        {
            bool continuar = true;

            while (continuar)
            {
                Console.Clear();
                CentralSaida.CabecalhoMenus("Gerenciar clientes");
                var reserva = SessaoAtual.ReservaSelecionada;

                if (reserva != null)
                {
                    InterfaceHelper.ExibirTituloComData($"Gerenciar clientes — {reserva.Espaco.NomeFantasia}");
                }
                else
                {
                    InterfaceHelper.ExibirTituloComData("Gerenciar clientes — reserva não selecionada");
                }

                Console.WriteLine("\n1 - Vincular cliente à reserva");
                Console.WriteLine("2 - Listar clientes vinculados");

                int opcao = CentralEntradaControladora.LerInteiroMinMaxComSN(
                    "Escolha uma opção",
                    1,
                    2,
                    "Menu – Clientes vinculados",
                    () => MenuReservas.ExibirMenuReservas(),
                    () => MenuPrincipal.ExibirPrincipal()
                );

                // se o usuário escolher sair via controle emocional
                if (opcao == 0)
                {
                    continuar = false;
                    continue;
                }

                switch (opcao)
                {
                    case 1:
                        CentralSaida.LimparTela("Vincular cliente à Reserva");
                        SessaoAtual.ReservaSelecionada?.VincularClientes(SessaoAtual.Clientes);
                        break;

                    case 2:
                        CentralSaida.LimparTela("Clientes Vinculados");

                        if (reserva != null && reserva.Clientes.Any())
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("Clientes atualmente vinculados:");
                            Console.ResetColor();

                            foreach (var cliente in reserva.Clientes)
                            {
                                Console.WriteLine($"- {cliente.Nome} ({cliente.Cpf})");
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("\nNenhum cliente está vinculado à reserva.");
                            Console.ResetColor();
                        }

                        MenuRodape.ExibirRodape(
             "Menu | Clientes vinculados",
             () => MenuReservas.ExibirMenuReservas(),
             () => MenuPrincipal.ExibirPrincipal()
         );

                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Opção inválida. Tente novamente.");
                        Console.ResetColor();
                        MenuRodape.ExibirRodape(
             "Menu | Clientes vinculados",
             () => MenuReservas.ExibirMenuReservas(),
             () => MenuPrincipal.ExibirPrincipal()
         );

                        break;
                }

                // rodapé ao final de cada ação
                MenuRodape.ExibirRodape(
                    "Menu – Clientes vinculados",
                    () => MenuReservas.ExibirMenuReservas(),
                    () => MenuPrincipal.ExibirPrincipal()
                );
            }
        }

        public static void VincularClientesAReserva()
        {
            NavegacaoAtual.Registrar(() => MenuReservas.ExibirMenuReservas());

            SessaoAtual.ClientesSelecionados = new List<Cliente>(); // garante lista zerada

            CentralSaida.LimparTela("Vincular Clientes à Reserva");
            CentralSaida.CabecalhoMenus("Vincular Clientes à Reserva");

            if (SessaoAtual.ReservaSelecionada == null)
            {
                CentralSaida.Exibir(TipoMensagem.Alerta, "Nenhuma reserva foi selecionada.");

                MenuRodape.ExibirRodape(
                    "Menu – Vincular Clientes",
                    aoVoltarParaSubmenu: () => MenuReservas.ExibirMenuReservas(),
                    aoVoltarMenuPrincipal: () => MenuPrincipal.ExibirPrincipal()
                );
                return;
            }

            if (SessaoAtual.Clientes.Count == 0)
            {
                CentralSaida.Exibir(TipoMensagem.Alerta, "Não há clientes cadastrados para vincular.");

                MenuRodape.ExibirRodape(
                    "Menu – Vincular Clientes",
                    aoVoltarParaSubmenu: () => MenuReservas.ExibirMenuReservas(),
                    aoVoltarMenuPrincipal: () => MenuPrincipal.ExibirPrincipal()
                );
                return;
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nClientes disponíveis:\n");
            Console.ResetColor();

            for (int i = 0; i < SessaoAtual.Clientes.Count; i++)
            {
                var cliente = SessaoAtual.Clientes[i];
                Console.WriteLine($"[{i + 1}] {cliente.Nome} – CPF: {cliente.Cpf}");
            }

            Console.WriteLine("\nDigite os números dos clientes que deseja vincular (separados por vírgula): ");
            string? entrada = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(entrada))
            {
                CentralSaida.Exibir(TipoMensagem.Alerta, "Nenhum cliente foi selecionado para vincular.");

                MenuRodape.ExibirRodape(
                    "Menu – Vincular Clientes",
                    aoVoltarParaSubmenu: () => MenuReservas.ExibirMenuReservas(),
                    aoVoltarMenuPrincipal: () => MenuPrincipal.ExibirPrincipal()
                );
                return;
            }

            var indices = entrada.Split(',')
                .Select(e => e.Trim())
                .Where(e => int.TryParse(e, out _))
                .Select(int.Parse)
                .Distinct()
                .ToList();

            Console.WriteLine();

            foreach (int indice in indices)
            {
                int posicao = indice - 1;
                if (posicao >= 0 && posicao < SessaoAtual.Clientes.Count)
                {
                    var clienteSelecionado = SessaoAtual.Clientes[posicao];

                    if (!SessaoAtual.ClientesSelecionados.Contains(clienteSelecionado))
                    {
                        SessaoAtual.ClientesSelecionados.Add(clienteSelecionado);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"{clienteSelecionado.Nome} vinculado com sucesso.");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"{clienteSelecionado.Nome} já está vinculado à reserva.");
                        Console.ResetColor();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Número {indice} é inválido.");
                    Console.ResetColor();
                }
            }

            try
            {
                SessaoAtual.ReservaSelecionada.VincularClientes(SessaoAtual.ClientesSelecionados);
                CentralSaida.Exibir(TipoMensagem.Sucesso, "Clientes vinculados à reserva com sucesso.");
            }
            catch (InvalidOperationException ex)
            {
                CentralSaida.Exibir(TipoMensagem.Alerta, ex.Message);
            }

            // Rodapé final para navegação emocional
            MenuRodape.ExibirRodape(
                "Menu – Vincular Clientes",
                () => MenuReservas.ExibirMenuReservas(),
                () => MenuPrincipal.ExibirPrincipal()
            );
        }

        public static void ListarCadastroReservas()
        {
            NavegacaoAtual.Registrar(() => MenuReservas.ExibirMenuReservas());

            CentralSaida.LimparTela("Reservas Ativas");
            CentralSaida.CabecalhoMenus("Reservas Ativas");
            //InterfaceHelper.ExibirTituloComData("Reservas");

            var reservasAtivas = SessaoAtual.Reservas.Where(r => r.Ativa).ToList();

            if (!reservasAtivas.Any())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nNenhuma reserva ativa foi encontrada.");
                Console.ResetColor();

                MenuRodape.ExibirRodape(
    "Menu – Cadastro de Reservas",
    aoVoltarParaSubmenu: () => NavegacaoAtual.Voltar(),

    aoVoltarMenuPrincipal: () => MenuPrincipal.ExibirPrincipal()
    );

                return;
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Reservas ativas registradas:\n");
            Console.ResetColor();

            //Console.ForegroundColor = ConsoleColor.DarkGray;
            //Console.WriteLine(new string('─', Console.WindowWidth));
            //Console.ResetColor();
            //Console.WriteLine();

            foreach (var r in reservasAtivas)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nReserva #{r.Espaco.Id:D3} – \"{r.Espaco.NomeFantasia}\"");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(new string('─', Console.WindowWidth));
                Console.ResetColor();

                Console.WriteLine($"Tipo: {r.Espaco.TipoEspaco}");
                Console.WriteLine($"Check-in: {r.DataCheckIn:dd/MM/yyyy}");
                Console.WriteLine($"Check-out previsto: {r.DataCheckIn.AddDays(r.DiasReservados):dd/MM/yyyy}");
                Console.WriteLine($" Valor estimado: {r.CalcularValorSemDesconto():C2}");
                Console.WriteLine($"Status: {(r.Ativa ? " Ativa" : "Inativa")}");

                Console.WriteLine("👥 Hóspedes vinculados:");

                if (r.Clientes.Any())
                {
                    foreach (var h in r.Clientes)
                        Console.WriteLine($"  - {h.NomeCompleto} ({h.Email})");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("  (Nenhum hóspede vinculado)");
                    Console.ResetColor();
                }

                Console.WriteLine(); // espaço entre reservas
            }

            // 🌟 Oferta emocional de vínculo
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\nDeseja vincular um hóspede a uma reserva agora? (S/N): ");
            Console.ResetColor();

            string? resposta = Console.ReadLine()?.Trim().ToUpper();

            if (resposta == "S")
            {
                int idReservaEscolhida = CentralEntradaControladora.LerInteiroMinMax(
                    "Digite o ID da reserva desejada",
                    reservasAtivas.Min(r => r.Espaco.Id),
                    reservasAtivas.Max(r => r.Espaco.Id)
                );

                var reservaSelecionada = reservasAtivas.FirstOrDefault(r => r.Espaco.Id == idReservaEscolhida && r.Ativa);

                SessaoAtual.ReservaSelecionada = reservaSelecionada;

                if (reservaSelecionada != null)
                {
                    MenuReservas.VincularClientesAReserva();
                }
                else
                {
                    CentralSaida.Exibir(TipoMensagem.Erro, "Reserva não encontrada ou inativa.");

                    MenuRodape.ExibirRodape(
                        "Menu – Cadastro de Reservas",
                        aoVoltarParaSubmenu: () => NavegacaoAtual.Voltar(),  //retorno dinâmico
                        aoVoltarMenuPrincipal: () => MenuPrincipal.ExibirPrincipal()
                    );
                }
            }

            MenuRodape.ExibirRodape(
            "Menu – Cadastro de Reservas",
            aoVoltarParaSubmenu: () => NavegacaoAtual.Voltar(),
            aoVoltarMenuPrincipal: () => MenuPrincipal.ExibirPrincipal()
            );

        }

        public static void CancelarReserva()
        {
            NavegacaoAtual.Registrar(() => MenuReservas.ExibirMenuReservas());

            CentralSaida.LimparTela("Cancelar Reserva");
            CentralSaida.CabecalhoMenus("Cancelar reserva");

            var reservasAtivas = SessaoAtual.Reservas.Where(r => r.Ativa).ToList();

            if (!reservasAtivas.Any())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nNenhuma reserva ativa disponível para cancelamento.");
                Console.ResetColor();

                MenuRodape.ExibirRodape(
                "Menu – Cancelar Reserva",
                aoVoltarParaSubmenu: () => NavegacaoAtual.Voltar(),
                aoVoltarMenuPrincipal: () => MenuPrincipal.ExibirPrincipal()
                );

                return;
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Reservas ativas registradas:\n");
            Console.ResetColor();

            for (int i = 0; i < reservasAtivas.Count; i++)
            {
                var r = reservasAtivas[i];
                Console.WriteLine($"[{i + 1}] {r.Espaco.NomeFantasia} | Check-in: {r.DataCheckIn:dd/MM/yyyy}");
            }

            int? escolha = CentralEntradaInteira.LerComCancelamento("Escolha a reserva a ser cancelada (pelo número)");

            if (escolha == null || escolha < 1 || escolha > reservasAtivas.Count)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nCancelamento não realizado. Retornando...");
                Console.ResetColor();

                MenuRodape.ExibirRodape(
                "Menu – Cancelar Reserva",
                aoVoltarParaSubmenu: () => NavegacaoAtual.Voltar(),
                aoVoltarMenuPrincipal: () => MenuPrincipal.ExibirPrincipal()
                );

                return;
            }

            var selecionada = reservasAtivas[escolha.Value - 1];
            selecionada.Ativa = false;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\nReserva do espaço \"{selecionada.Espaco.NomeFantasia}\" cancelada com sucesso.");
            Console.ResetColor();

            EntradaHelper.ExibirMensagemRetorno("Pressione ENTER para continuar...");

            MenuRodape.ExibirRodape(
            "Menu – Cancelar Reserva",
            aoVoltarParaSubmenu: () => NavegacaoAtual.Voltar(),
            aoVoltarMenuPrincipal: () => MenuPrincipal.ExibirPrincipal()
            );

        }

        private static void EncerrarReserva()
        {
            NavegacaoAtual.Registrar(() => MenuReservas.ExibirMenuReservas());

            CentralSaida.LimparTela("Encerrar Reserva");
            CentralSaida.CabecalhoMenus("Encerrar Reserva");

            var reservasAtivas = SessaoAtual.Reservas.Where(r => r.Ativa).ToList();

            if (!reservasAtivas.Any())
            {
                CentralSaida.Exibir(TipoMensagem.Alerta, "Nenhuma reserva ativa disponível para encerramento.");

                MenuRodape.ExibirRodape(
                "Menu – Cancelar Reserva",
                aoVoltarParaSubmenu: () => NavegacaoAtual.Voltar(),
                aoVoltarMenuPrincipal: () => MenuPrincipal.ExibirPrincipal()
                );

                return;
            }

            int? escolha = CentralEntradaInteira.LerComCancelamento("Escolha a reserva a ser encerrada (pelo número)");

            if (escolha == null || escolha < 1 || escolha > reservasAtivas.Count)
            {
                CentralSaida.Exibir(TipoMensagem.Alerta, "Encerramento cancelado ou seleção inválida.");

                MenuRodape.ExibirRodape(
        "Menu – Cancelar Reserva",
        aoVoltarParaSubmenu: () => NavegacaoAtual.Voltar(),
        aoVoltarMenuPrincipal: () => MenuPrincipal.ExibirPrincipal()
        );

                return;
            }

            var reservaEncerrada = reservasAtivas[escolha.Value - 1];
            reservaEncerrada.Ativa = false;
            reservaEncerrada.DataCheckOut = DateTime.Now;

            CentralSaida.Exibir(TipoMensagem.Sucesso, "Reserva encerrada com sucesso!");

            MenuRodape.ExibirRodape(
            "Menu – Cancelar Reserva",
            aoVoltarParaSubmenu: () => NavegacaoAtual.Voltar(),
            aoVoltarMenuPrincipal: () => MenuPrincipal.ExibirPrincipal()
            );

        }

        public static void ExcluirCadastroReserva()
        {
            NavegacaoAtual.Registrar(() => MenuReservas.ExibirMenuReservas());

            CentralSaida.LimparTela("Excluir Reserva");
            CentralSaida.CabecalhoMenus("Excluir Reservas");

            if (!SessaoAtual.Reservas.Any())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nNão há reservas cadastradas.");
                Console.ResetColor();

                MenuRodape.ExibirRodape(
                "Menu – Excluir Cadastro Reserva",
                aoVoltarParaSubmenu: () => NavegacaoAtual.Voltar(),
                aoVoltarMenuPrincipal: () => MenuPrincipal.ExibirPrincipal()
                );

                return;
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Reservas disponíveis para exclusão:\n");
            Console.ResetColor();

            for (int i = 0; i < SessaoAtual.Reservas.Count; i++)
            {
                var r = SessaoAtual.Reservas[i];
                string status = r.Ativa ? "Ativa" : "Encerrada";
                Console.WriteLine($"[{i + 1}] {r.Espaco.NomeFantasia} | Check-in: {r.DataCheckIn:dd/MM/yyyy} | Status: {status}");
            }

            int? escolha = CentralEntradaInteira.LerComCancelamento("Digite o número da reserva que deseja excluir");

            if (escolha == null || escolha < 1 || escolha > SessaoAtual.Reservas.Count)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nExclusão cancelada. Retornando...");
                Console.ResetColor();

                MenuRodape.ExibirRodape(
               "Menu – Excluir Cadastro Reserva",
               aoVoltarParaSubmenu: () => NavegacaoAtual.Voltar(),
               aoVoltarMenuPrincipal: () => MenuPrincipal.ExibirPrincipal()
               );
                return;
            }

            var selecionada = SessaoAtual.Reservas[escolha.Value - 1];

            bool confirmar = CentralEntradaControladora.SimNao(
                $"\nTem certeza que deseja excluir a reserva do espaço \"{selecionada.Espaco.NomeFantasia}\"?");

            if (!confirmar)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nExclusão cancelada pelo usuário.");
                Console.ResetColor();
            }
            else
            {
                SessaoAtual.Reservas.Remove(selecionada);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nReserva excluída com sucesso!");
                Console.ResetColor();
            }

            EntradaHelper.ExibirMensagemRetorno("Cliente vinculado!", "Menu – Clientes", () => MenuClientes.ExibirMenuClientes());


            MenuRodape.ExibirRodape(
               "Menu – Excluir Cadastro Reserva",
               aoVoltarParaSubmenu: () => NavegacaoAtual.Voltar(),
               aoVoltarMenuPrincipal: () => MenuPrincipal.ExibirPrincipal()
               );
        }



    }
}
