using DesafioProjetoHospedagem.Models;
using DesafioProjetoHospedagem.Utils;
using DesafioProjetoHospedagem.Menus;
using DesafioProjetoHospedagem.Controladoras;
using DesafioProjetoHospedagem.Servicos;
using System;
using DesafioProjetoHospedagem.Sessao;

namespace DesafioProjetoHospedagem.Controladoras;
public static class ClienteControlador
{
    //é o menu do cadastrarClientes - faz parte do CadastrarClientes abaixo

    public static void MenuDoCadastroDeCliente(List<string> opcoes)
    {
        CentralSaida.LimparTela("Menu Pós-Cadastro");
        CentralSaida.CabecalhoMenus("Cadastro do cliente");

        // Aqui você exibe o menu usando a lista recebida
        foreach (var opcao in opcoes)
        {
            Console.WriteLine(opcao);
        }

        int escolha = CentralEntradaInteira.UmAteLimite(
            "Selecione uma opção",
            opcoes.Count,
            "Cadastro de Cliente",
            () => MenuClientes.ExibirMenuClientes(),
            () => MenuPrincipal.ExibirPrincipal()
        );

        switch (escolha)
        {
            case 1:
                MenuReservas.CriarReserva();
                break;
            case 2:
                MenuReservas.ListarCadastroReservas();
                break;
            case 3:
                MenuClientes.ListarCadastroClientes();
                break;
            case 4:
                if (SessaoAtual.ReservaSelecionada == null)
                {
                    CentralSaida.Exibir(TipoMensagem.Alerta, "Nenhuma reserva selecionada.");
                    bool desejaSelecionarOuCriar = CentralEntradaControladora.SimNao("Deseja criar ou selecionar uma reserva agora?");
                    if (desejaSelecionarOuCriar)
                        MenuReservas.CriarReserva();
                    else
                    {
                        CentralSaida.Exibir(TipoMensagem.Informacao, "Ação cancelada. Retornando ao menu.");
                        MenuClientes.ExibirMenuClientes();
                        return;
                    }
                }

                MenuReservas.VincularClientesAReserva();
                break;
        }

        MenuRodape.ExibirRodape(
            "Cadastro de Cliente",
            () => MenuClientes.ExibirMenuClientes(),
            () => MenuPrincipal.ExibirPrincipal()
        );
    }

    public static Cliente CadastrarCliente()
    {
        CentralSaida.LimparTela("CADASTRO DE CLIENTE");
        CentralSaida.CabecalhoMenus("Cadastro de Cliente");

        // Nome completo – obrigatório com controle emocional
        string nome = CentralEntradaControladora.LerTextoComSN(
            "Nome completo do cliente",
            "Cadastro de Cliente",
            () => MenuClientes.ExibirMenuClientes(),
            () => MenuPrincipal.ExibirPrincipal()
        );

        if (string.IsNullOrWhiteSpace(nome))
            return null!; // ou lidar com retorno nulo/cancelado

        // Verificação de duplicidade
        Cliente? existente = SessaoAtual.Clientes.FirstOrDefault(
            c => c.Nome.Trim().Equals(nome.Trim(), StringComparison.OrdinalIgnoreCase)
        );

        if (existente != null)
        {
            CentralSaida.Exibir(TipoMensagem.Alerta, $"Cliente \"{existente.Nome}\" já está cadastrado.");
            EntradaHelper.ExibirMensagemRetorno();
            return existente;
        }

        // Campos opcionais com ENTER emocional
        string cpf = CentralEntradaControladora.LerTextoComSN("CPF (opcional)", "Cadastro de Cliente", () => { }, () => { });
        string email = CentralEntradaControladora.LerTextoComSN("Email (opcional)", "Cadastro de Cliente", () => { }, () => { });
        string telefone = CentralEntradaControladora.LerTextoComSN("Telefone (opcional)", "Cadastro de Cliente", () => { }, () => { });

        // Criar novo cliente
        Cliente novoCliente = new(nome)
        {
            Cpf = cpf,
            Email = email,
            Telefone = telefone
        };

        SessaoAtual.Clientes.Add(novoCliente);

        CentralSaida.Exibir(TipoMensagem.Sucesso, $"Cliente \"{novoCliente.Nome}\" cadastrado com sucesso!");

        EntradaHelper.ExibirMensagemRetorno("Pressione ENTER para continuar...");

        // Menu vertical de decisão emocional
        CentralSaida.LimparTela("Menu Pós-Cadastro");
        ClienteControlador.MenuDoCadastroDeCliente(new List<string>
        {
            "1 - Criar Reserva",
            "2 - Reservas Ativas",
            "3 - Lista de Clientes",
            "4 - Vincular Cliente à Reserva"
        });

        int escolha = CentralEntradaInteira.UmAteLimite(
            "Selecione uma opção",
            4,
            "Cadastro de Cliente",
            () => MenuClientes.ExibirMenuClientes(),
            () => MenuPrincipal.ExibirPrincipal()
        );

        switch (escolha)
        {
            case 1:
                MenuReservas.CriarReserva();
                break;

            case 2:
                MenuReservas.ListarCadastroReservas();
                break;

            case 3:
                MenuClientes.ListarCadastroClientes();
                break;


            case 4:
                if (SessaoAtual.ReservaSelecionada != null)
                {
                    MenuReservas.VincularClientesAReserva();
                }
                else
                {
                    CentralSaida.Exibir(TipoMensagem.Alerta, "Nenhuma reserva ativa para vínculo.");
                    EntradaHelper.ExibirMensagemRetorno("Pressione ENTER para voltar.");
                }
                break;
        }

        // 🔹 Rodapé de encerramento da página
        MenuRodape.ExibirRodape(
            "Cadastro de Cliente",
            () => MenuClientes.ExibirMenuClientes(),
            () => MenuPrincipal.ExibirPrincipal()
        );
        return novoCliente;
    }

    public static void EditarCadastroCliente()
    {
        MenuClientes.EditarCadastroCliente();
    }

    public static void ListarCadastroClientes()
    {
        CentralSaida.LimparTela("Lista de Clientes");
        CentralSaida.CabecalhoMenus("Clientes cadastrados");

        if (!SessaoAtual.Clientes.Any())
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Nenhum cliente foi cadastrado ainda.");
            Console.ResetColor();
            EntradaHelper.ExibirMensagemRetorno("Pressione ENTER para continuar...");
            return;
        }

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Clientes cadastrados:\n");
        Console.ResetColor();

        foreach (var cliente in SessaoAtual.Clientes)
        {
            Console.WriteLine($"- {cliente.Nome} | CPF: {cliente.Cpf} | Email: {cliente.Email}");
        }

        EntradaHelper.ExibirMensagemRetorno("Pressione ENTER para continuar...");
    }

}
