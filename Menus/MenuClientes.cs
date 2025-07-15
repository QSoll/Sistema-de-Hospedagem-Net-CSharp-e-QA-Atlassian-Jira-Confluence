using System;
using System.Linq;
using DesafioProjetoHospedagem.Controladoras;
using System.Collections.Generic;
using DesafioProjetoHospedagem.Models;
using DesafioProjetoHospedagem.Utils;
using DesafioProjetoHospedagem.Servicos;
using DesafioProjetoHospedagem.Sessao;
using DesafioProjetoHospedagem.Menus;

namespace DesafioProjetoHospedagem.Menus
{
    public static class MenuClientes
    {
        public static Reserva? ExibirMenuClientes() //parâmetros de entrada do método e dentro do escopo são variáveis
        {
            while (true)
            {
                Console.Clear();
                CentralSaida.CabecalhoMenus("MENU – CLIENTES");
                SessaoAtual.VerificarSessaoGlobal();

                Console.WriteLine("1 - Listar todos");
                Console.WriteLine("2 - Listar hospedados");
                Console.WriteLine("3 - Listar não hospedados");
                Console.WriteLine("4 - Cadastrar novo");
                Console.WriteLine("5 - Editar cadastro");
                Console.WriteLine("6 - Excluir cadastro");

                CentralSaida.LinhaSeparadora();

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("Digite sua opção: ");
                Console.ResetColor();

                int entrada = CentralEntradaInteira.UmAteLimite
                (
                    "Escolha uma opção ",
                    6,
                    "Clientes",
                    aoVoltarParaSubmenu: () => ExibirMenuClientes(),
                    aoVoltarMenuPrincipal: () => MenuPrincipal.ExibirPrincipal()
                );
                switch (entrada)
                {
                    case 1:
                        ListarCadastroClientes();
                        break;

                    case 2:
                        ListarClientesHospedados();
                        break;

                    case 3:
                        ListarClientesNaoHospedados();
                        break;

                    case 4:
                        CadastrarCliente();
                        break;

                    case 5:
                        EditarCadastroCliente();
                        break;

                    case 6:
                        ExcluirCadastroCliente();
                        break;

                    default:
                        CentralSaida.Exibir(TipoMensagem.Erro, "Opção inválida. Tente novamente!");
                        break;
                }

                MenuRodape.ExibirRodape(
                "Menu – Clientes",
                aoVoltarParaSubmenu: () => ExibirMenuClientes(),
                aoVoltarMenuPrincipal: () => MenuPrincipal.ExibirPrincipal()
                );
            }
        }

        public static Cliente CadastrarCliente()
        {
            Console.Clear();
            CentralSaida.CabecalhoMenus("Menu – Cadastro de Cliente");

            string nome = CentralEntrada.LerTextoObrigatorio("Nome do cliente");
            string cpf = CentralEntrada.LerTextoObrigatorio("CPF do cliente");
            string telefone = CentralEntrada.LerTextoObrigatorio("Telefone do cliente");
            string email = CentralEntrada.LerTextoObrigatorio("Email do cliente");
            string endereco = CentralEntrada.LerTextoObrigatorio("Endereço do cliente");

            Cliente novo = new Cliente(nome, cpf, telefone, email, endereco);
            SessaoAtual.Clientes.Add(novo);

            CentralSaida.Exibir(TipoMensagem.Sucesso, $"\nCliente \"{nome}\" cadastrado com sucesso.");

            Console.WriteLine("Cadastro finalizado!");

            MenuRodape.ExibirRodape(
                "Cadastro de Cliente",
                () => MenuClientes.ExibirMenuClientes(),
                () => MenuPrincipal.ExibirPrincipal()
            );
            return novo;
        }

        public static void EditarCadastroCliente()
        {
            CentralSaida.LimparTela("EDITAR CLIENTE");
            CentralSaida.CabecalhoMenus("Menu - Editar cadastro de clientes");

            if (!SessaoAtual.Clientes.Any())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Ainda não há clientes cadastrados.");
                Console.ResetColor();
                MenuRodape.ExibirRodape(
                "Menu – Clientes",
                aoVoltarParaSubmenu: () => ExibirMenuClientes(),
                aoVoltarMenuPrincipal: () => MenuPrincipal.ExibirPrincipal()
                );
            }

            Console.WriteLine("Clientes cadastrados:\n");
            for (int i = 0; i < SessaoAtual.Clientes.Count; i++)
                Console.WriteLine($"[{i + 1}] {SessaoAtual.Clientes[i].NomeCompleto}");

            int? indice = null;
            do
            {
                indice = CentralEntradaControladora.LerInteiroMinMax(
                "Digite o número do cliente que deseja editar",
                1,
                SessaoAtual.Clientes.Count
            );

                if (indice == null || indice < 1 || indice > SessaoAtual.Clientes.Count)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Número inválido. Tente novamente.");
                    Console.ResetColor();
                    indice = null;
                }
            } while (indice == null);

            var cliente = SessaoAtual.Clientes[indice.Value - 1];

            CentralSaida.LimparTela($"Editando cadastro de: {cliente.NomeCompleto}");

            string novoNome = CentralEntradaControladora.LerTextoComSN(
            "Novo primeiro nome",
            "Menu – Clientes",
            () => MenuClientes.ExibirMenuClientes(),
            () => MenuPrincipal.ExibirPrincipal());

            string novoSobrenome = CentralEntradaControladora.LerTextoComSN(
            "Novo primeiro nome",
            "Menu – Clientes",
            () => MenuClientes.ExibirMenuClientes(),
            () => MenuPrincipal.ExibirPrincipal()
            );

            string novoEmail = CentralEntradaControladora.LerEmailComSN(
            "Novo e-mail",
            "Menu – Clientes",
            () => MenuClientes.ExibirMenuClientes(),
            () => MenuPrincipal.ExibirPrincipal()
            );

            if (!string.IsNullOrWhiteSpace(novoNome)) cliente.Nome = novoNome;
            if (!string.IsNullOrWhiteSpace(novoSobrenome)) cliente.Sobrenome = novoSobrenome;
            if (!string.IsNullOrWhiteSpace(novoEmail))
                cliente.Email = novoEmail;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nCliente \"{cliente.NomeCompleto}\" atualizado com sucesso!");
            Console.ResetColor();

            MenuRodape.ExibirRodape(
            "Menu – Clientes",
            aoVoltarParaSubmenu: () => ExibirMenuClientes(),
            aoVoltarMenuPrincipal: () => MenuPrincipal.ExibirPrincipal()
            );
        }

        public static void ExcluirCadastroCliente()
        {
            CentralSaida.LimparTela("EXCLUSÃO DE CLIENTE");
            CentralSaida.CabecalhoMenus("Menu - Excluir clientes");

            if (!SessaoAtual.Clientes.Any())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Ainda não há clientes cadastrados.");
                Console.ResetColor();

                MenuRodape.ExibirRodape(
                "Menu – Clientes",
                aoVoltarParaSubmenu: () => ExibirMenuClientes(),
                aoVoltarMenuPrincipal: () => MenuPrincipal.ExibirPrincipal()
                );
            }

            Console.WriteLine("Clientes disponíveis para exclusão:\n");
            for (int i = 0; i < SessaoAtual.Clientes.Count; i++)
                Console.WriteLine($"[{i + 1}] {SessaoAtual.Clientes[i].NomeCompleto}");

            int? indice = null;
            do
            {
                indice = CentralEntradaControladora.LerInteiroMinMaxComSN(
                    "Digite o número do cliente que deseja excluir",
                    1,
                    SessaoAtual.Clientes.Count,
                    "Menu – Clientes",
                    () => MenuClientes.ExibirMenuClientes(),
                    () => MenuPrincipal.ExibirPrincipal()
                );

                if (indice == null || indice < 1 || indice > SessaoAtual.Clientes.Count)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Número inválido. Tente novamente.");
                    Console.ResetColor();
                    indice = null;
                }
            } while (indice == null);

            var cliente = SessaoAtual.ClienteSelecionado;

            if (cliente == null)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nNenhum cliente selecionado.");
                Console.ResetColor();
                return;
            }

            // Verifica se o cliente está vinculado a alguma reserva ativa
            bool possuiReservaAtiva = SessaoAtual.Reservas
                .Any(r => r.Ativa && r.Clientes.Contains(cliente));

            if (possuiReservaAtiva)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nNão é possível excluir \"{cliente.NomeCompleto}\" pois está vinculado a uma reserva ativa.");
                Console.ResetColor();
            }
            else
            {
                SessaoAtual.Clientes.Remove(cliente);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nCliente \"{cliente.NomeCompleto}\" removido com sucesso.");
                Console.ResetColor();
            }

            MenuRodape.ExibirRodape(
                "Menu – Clientes",
                aoVoltarParaSubmenu: () => ExibirMenuClientes(),
                aoVoltarMenuPrincipal: () => MenuPrincipal.ExibirPrincipal()
                );
        }

        public static void ListarCadastroClientes()
        {
            CentralSaida.LimparTela("Cadastro de Clientes");
            Console.Clear();
            CentralSaida.CabecalhoMenus("Menu - Listar cadastro de clientes");

            if (!SessaoAtual.Clientes.Any())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Nenhum cliente cadastrado no momento.");
                Console.ResetColor();

                MenuRodape.ExibirRodape(
                "Menu – Clientes",
                aoVoltarParaSubmenu: () => ExibirMenuClientes(),
                aoVoltarMenuPrincipal: () => MenuPrincipal.ExibirPrincipal()
                );
            }

            var ordenados = SessaoAtual.Clientes
                .OrderBy(c => c.NomeCompleto)
                .ThenBy(c => c.DataCadastro)
                .ToList();

            foreach (var cliente in ordenados)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"\n• {cliente.NomeCompleto}");
                Console.ResetColor();

                Console.WriteLine($"  Email: {cliente.Email}");
                Console.WriteLine($"  Cadastrado em: {cliente.DataCadastro:dd/MM/yyyy HH:mm}");

                var reservasConcluidas = SessaoAtual.Reservas
                    .Where(r => !r.Ativa && r.Clientes.Contains(cliente))
                    .OrderByDescending(r => r.DataCheckIn)
                    .ToList();

                if (reservasConcluidas.Any())
                {
                    Console.WriteLine("Reservas concluídas:");
                    foreach (var r in reservasConcluidas)
                    {
                        var checkOut = r.DataCheckIn.AddDays(r.DiasReservados);
                        Console.WriteLine($"     - {r.DataCheckIn:dd/MM/yyyy} até {checkOut:dd/MM/yyyy} | Espaço: {r.Espaco.NomeFantasia}");
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("  Nenhuma reserva concluída.");
                    Console.ResetColor();
                }
            }

            MenuRodape.ExibirRodape(
                "Menu – Clientes",
                aoVoltarParaSubmenu: () => ExibirMenuClientes(),
                aoVoltarMenuPrincipal: () => MenuPrincipal.ExibirPrincipal()
                );


            MenuRodape.ExibirRodape(
    "Menu – Clientes",
    aoVoltarParaSubmenu: () => ExibirMenuClientes(),
    aoVoltarMenuPrincipal: () => MenuPrincipal.ExibirPrincipal()
    );
        }

        public static void ListarClientesHospedados()
        {
            CentralSaida.LimparTela("Clientes Hospedados");
            Console.Clear();
            CentralSaida.CabecalhoMenus("MENU – clientes hospedados");

            if (!SessaoAtual.Clientes.Any())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Nenhum cliente cadastrado.");
                Console.ResetColor();
                MenuRodape.ExibirRodape(
                 "Menu – Clientes",
                 aoVoltarParaSubmenu: () => ExibirMenuClientes(),
                 aoVoltarMenuPrincipal: () => MenuPrincipal.ExibirPrincipal()
                 );
            }

            Console.WriteLine("1 - Listar todos os clientes hospedados");
            Console.WriteLine("2 - Buscar cliente por nome");

            int opcao = CentralEntradaInteira.UmAteLimite(
                "Escolha uma opção",
                2,
                "Menu Clientes",
                aoVoltarParaSubmenu: () => ExibirMenuClientes(),
                aoVoltarMenuPrincipal: () => MenuPrincipal.ExibirPrincipal()
                );

            switch (opcao)
            {
                case 1:
                    var hospedados = SessaoAtual.Reservas
                        .Where(r => r.Ativa)
                        .SelectMany(r => r.Clientes.Select(h => new
                        {
                            Nome = h.NomeCompleto,
                            Espaco = r.Espaco.NomeFantasia,
                            Id = r.Espaco.Id,
                            Dias = r.DiasReservados,
                            Valor = r.CalcularValorSemDesconto()

                        }))
                        .ToList();

                    if (!hospedados.Any())
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Nenhum cliente está hospedado no momento.");
                        Console.ResetColor();
                    }
                    else
                    {
                        foreach (var h in hospedados)
                        {
                            Console.WriteLine($"- {h.Nome}");
                            Console.WriteLine($"  Espaço: {h.Espaco} (ID: {h.Id:D3})");
                            Console.WriteLine($"  Duração: {h.Dias} dia(s)");
                            Console.WriteLine($"  Valor total: {h.Valor:C}\n");
                        }
                    }

                    // MenuRodape ao final do case 1
                    MenuRodape.ExibirRodape(
                    "Menu – Clientes",
                    aoVoltarParaSubmenu: () => ExibirMenuClientes(),
                    aoVoltarMenuPrincipal: () => MenuPrincipal.ExibirPrincipal()
                    );
                    break;

                case 2:
                    string termo = CentralEntradaControladora.LerTextoComSN(
                        "Digite parte do nome ou sobrenome para buscar",
                        "Menu – Clientes",
                        () => MenuClientes.ExibirMenuClientes(),
                        () => MenuPrincipal.ExibirPrincipal()
                    );

                    var encontrados = SessaoAtual.Reservas
                    .Where(r => r.Ativa)
                    .SelectMany(r => r.Clientes)
                    .Where(c => c.NomeCompleto.ToUpper().Contains(termo.ToUpper()))
                    .Distinct()
                    .ToList();


                    if (!encontrados.Any())
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\nNenhum cliente encontrado com esse termo.");
                        Console.ResetColor();

                        MenuRodape.ExibirRodape(
                    "Menu – Clientes",
                    aoVoltarParaSubmenu: () => ExibirMenuClientes(),
                    aoVoltarMenuPrincipal: () => MenuPrincipal.ExibirPrincipal()
                    );
                        break;
                    }

                    Console.WriteLine("\nClientes encontrados:");
                    for (int i = 0; i < encontrados.Count; i++)
                        Console.WriteLine($"[{i + 1}] {encontrados[i].NomeCompleto} ({encontrados[i].Email})");

                    int escolha = CentralEntradaControladora.LerInteiroMinMaxComSN(
                        "Digite o número do cliente que deseja consultar",
                        1,
                        encontrados.Count,
                        "Menu – Clientes",
                        () => MenuClientes.ExibirMenuClientes(),
                        () => MenuPrincipal.ExibirPrincipal()
                    );

                    var selecionado = encontrados[escolha - 1];
                    CentralSaida.LimparTela($"Detalhes de {selecionado.NomeCompleto}");

                    Console.WriteLine($"Email: {selecionado.Email}");
                    Console.WriteLine($"Data de cadastro: {selecionado.DataCadastro:dd/MM/yyyy HH:mm}");

                    var reservaAtual = SessaoAtual.Reservas.FirstOrDefault(r =>
                        r.Ativa && r.Clientes.Contains(selecionado));

                    if (reservaAtual != null)
                    {
                        var checkOut = reservaAtual.DataCheckIn.AddDays(reservaAtual.DiasReservados);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\nStatus: HOSPEDADO");
                        Console.ResetColor();
                        Console.WriteLine($"- Espaço: {reservaAtual.Espaco.NomeFantasia} (ID {reservaAtual.Espaco.Id})");
                        Console.WriteLine($"- Check-in: {reservaAtual.DataCheckIn:dd/MM/yyyy}");
                        Console.WriteLine($"- Check-out previsto: {checkOut:dd/MM/yyyy}");
                        Console.WriteLine($"- Valor total: {reservaAtual.CalcularValorSemDesconto():C}");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\nStatus: NÃO HOSPEDADO");
                        Console.ResetColor();
                    }

                    // MenuRodape ao final do case 2
                    MenuRodape.ExibirRodape(
    "Menu – Clientes",
    aoVoltarParaSubmenu: () => ExibirMenuClientes(),
    aoVoltarMenuPrincipal: () => MenuPrincipal.ExibirPrincipal()
);
                    break;
            }
        }

        public static void ListarClientesNaoHospedados()
        {
            CentralSaida.LimparTela("CLIENTES NÃO HOSPEDADOS");
            Console.Clear();
            CentralSaida.CabecalhoMenus("Menu - Listar clientes não hospedados");

            if (!SessaoAtual.Clientes.Any())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Ainda não há clientes cadastrados.");
                Console.ResetColor();
                MenuRodape.ExibirRodape(
                 "Menu – Clientes",
                 aoVoltarParaSubmenu: () => ExibirMenuClientes(),
                 aoVoltarMenuPrincipal: () => MenuPrincipal.ExibirPrincipal()
                 );
            }

            var naoHospedados = SessaoAtual.Clientes
                .Where(c =>
                    !SessaoAtual.Reservas.Any(r =>
                        r.Ativa && r.Clientes.Contains(c)))
                .OrderBy(c => c.NomeCompleto)
                .ToList();

            if (!naoHospedados.Any())
            {
                //Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Todos os clientes estão atualmente hospedados.");
                //Console.ResetColor();
            }
            else
            {
                Console.WriteLine("Lista de clientes não hospedados:\n");

                foreach (var cliente in naoHospedados)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"• {cliente.NomeCompleto}");
                    Console.ResetColor();

                    Console.WriteLine($"  Email: {cliente.Email}");
                    Console.WriteLine($"  Cadastrado em: {cliente.DataCadastro:dd/MM/yyyy HH:mm}\n");
                }
            }
            MenuRodape.ExibirRodape(
            "Menu – Clientes",
            aoVoltarParaSubmenu: () => ExibirMenuClientes(),
            aoVoltarMenuPrincipal: () => MenuPrincipal.ExibirPrincipal()
            );

        }

    }
}



