using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using DesafioProjetoHospedagem.Models;
using DesafioProjetoHospedagem.Utils;
using DesafioProjetoHospedagem.Servicos;
using DesafioProjetoHospedagem.Sessao;   // se for salvar comprovantes futuramente
using DesafioProjetoHospedagem.Menus;
using System;
using System.IO;
using System.Text;


//atualizado com navegacao reversa e menurodapeGlobal
namespace DesafioProjetoHospedagem.Menus;

public static class MenuComprovantes
{
    public static void ExibirMenuComprovantes()
    {
        // ➤ Registrar para navegação reversa
        NavegacaoAtual.Registrar(() => MenuPrincipal.ExibirPrincipal());

        var clientesSelecionados = new List<Cliente>();

        Console.Clear();
        CentralSaida.CabecalhoMenus("Menu Comprovantes");

        Console.WriteLine("Este menu permite visualizar ou salvar o comprovante da reserva.");
        Console.WriteLine("O arquivo .txt será salvo automaticamente na pasta do projeto com nome baseado na data.");
        Console.WriteLine();

        Console.WriteLine("1 - Visualizar comprovante na tela");
        Console.WriteLine("2 - Salvar comprovante como arquivo .txt");

        Console.Write("\nEscolha uma opção: ");
        string? entrada = Console.ReadLine();

        switch (entrada)
        {
            case "1":
                ExibirComprovante(); // só visualiza
                break;

            case "2":
                ExibirComprovante(salvarEmArquivo: true); // visualiza e salva
                break;

            default:
                CentralSaida.Exibir(TipoMensagem.Erro, "Opção inválida. Tente novamente.");
                break;
        }

        CentralSaida.CabecalhoMenus("Menu – Comprovantes");

        MenuRodape.ExibirRodape(
            "Menu – Comprovantes",
            aoVoltarParaSubmenu: () => MenuComprovantes.ExibirMenuComprovantes(),
            aoVoltarMenuPrincipal: () => MenuPrincipal.ExibirPrincipal()
        );
    }
    private static void ExibirComprovante(bool salvarEmArquivo = false)
    {
        NavegacaoAtual.Registrar(() => MenuComprovantes.ExibirMenuComprovantes());

        var reserva = SessaoAtual.ReservaSelecionada;

        if (reserva == null)
        {
            CentralSaida.Exibir(TipoMensagem.Alerta, "Nenhuma reserva selecionada.");
            return;
        }

        var resumo = reserva.CalcularResumoFinanceiro();
        if (resumo == null)
        {
            CentralSaida.Exibir(TipoMensagem.Alerta, "Não foi possível calcular os valores da reserva.");
            return;
        }

        var texto = new StringBuilder();
        texto.AppendLine("=== COMPROVANTE DE RESERVA ===");
        texto.AppendLine($"Emitido em: {DateTime.Now:dd/MM/yyyy HH:mm}");
        texto.AppendLine();
        texto.AppendLine($"Espaço: {reserva.Espaco?.NomeFantasia} (ID {reserva.Espaco?.Id:D3})");
        texto.AppendLine($"Check-in: {reserva.DataCheckIn:dd/MM/yyyy}");
        texto.AppendLine($"Check-out: {reserva.DataCheckOut:dd/MM/yyyy}");
        texto.AppendLine($"Duração: {reserva.DiasReservados} dia(s)");
        texto.AppendLine();
        texto.AppendLine("--- VALORES FINANCEIROS ---");
        texto.AppendLine($"Valor da diária: R$ {reserva.Espaco?.ValorDiaria:N2}");
        texto.AppendLine($"Valor sem desconto: R$ {resumo.ValorSemDesconto:N2}");
        texto.AppendLine($"Desconto aplicado: R$ {resumo.ValorDoDesconto:N2}");
        texto.AppendLine($"Total com desconto: R$ {resumo.ValorComDesconto:N2}");
        texto.AppendLine($"Taxa extra: R$ {resumo.ValorTaxaExtra:N2}");
        texto.AppendLine($"Valor final a pagar: R$ {resumo.ValorTotalEstimado:N2}");
        texto.AppendLine();
        texto.AppendLine("Clientes vinculados:");

        if (reserva.Clientes != null && reserva.Clientes.Any())
        {
            foreach (var h in reserva.Clientes)
                texto.AppendLine($"- {h.NomeCompleto} ({h.Email})");
        }
        else
        {
            texto.AppendLine("- Nenhum cliente vinculado.");
        }

        texto.AppendLine();
        texto.AppendLine("Obrigada pela preferência. Até breve!!!");

        // Exibe na tela primeiro
        Console.Clear();
        CentralSaida.CabecalhoMenus("Comprovante de Reserva");
        Console.WriteLine(texto.ToString());

        // Salva como .txt se solicitado
        if (salvarEmArquivo)
        {
            var utf8SemBOM = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
            string nomeArquivo = $"Comprovante_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
            File.WriteAllText(nomeArquivo, texto.ToString(), utf8SemBOM);
            CentralSaida.Exibir(TipoMensagem.Sucesso, $"Comprovante salvo como \"{nomeArquivo}\".");
        }

        MenuRodapeGlobal.ExibirRodapeGlobal("Comprovante de Reserva");

    }

    /// <summary>
    /// Permite ao usuário selecionar uma reserva dentre os resultados encontrados.
    /// Aplica entrada emocional e navegação gentil via MiniMenu.
    /// </summary>
    private static Reserva? SelecionarReservaPorLista(
    List<Reserva> resultados,
    string contexto,
    List<Cliente> clientes,
    List<EspacoHospedagem> espacosHospedagem
    )
    {
        NavegacaoAtual.Registrar(() => MenuComprovantes.ExibirMenuComprovantes());

        CentralSaida.LimparTela("SELECIONAR RESERVA");
        CentralSaida.CabecalhoMenus($"Reserva por {contexto}");

        if (!resultados.Any())
        {
            CentralSaida.Exibir(TipoMensagem.Alerta, $"Nenhuma reserva encontrada pelo {contexto} informado.");
            EntradaHelper.ExibirMensagemRetorno("Menu – Comprovantes");

            MenuRodape.ExibirRodape(
                "Menu de Comprovantes",
                () => MenuComprovantes.ExibirMenuComprovantes(),
                () => MenuPrincipal.ExibirPrincipal()
            );
            return null;
        }

        if (resultados.Count == 1)
        {
            return resultados.First();
        }

        Console.WriteLine($"\nForam encontradas várias reservas por {contexto}:\n");

        for (int i = 0; i < resultados.Count; i++)
        {
            var r = resultados[i];
            Console.WriteLine($"{i + 1} - Espaço {r.Espaco.TipoEspaco?.ToUpper()} \"{r.Espaco.NomeFantasia}\" (ID: {r.Espaco.Id:D3}) – Check-in: {r.DataCheckIn:dd/MM/yyyy} – {r.Clientes.Count} cliente(s)");
        }

        int escolha = CentralEntradaControladora.LerInteiroMinMaxComSN(
            "\nDigite o número da reserva desejada:",
            1,
            resultados.Count,
            "Menu – Comprovantes",
            () => MenuComprovantes.ExibirMenuComprovantes(),
            () => MenuPrincipal.ExibirPrincipal()
        );

        var reservaSelecionada = resultados[escolha - 1];

        MenuRodapeGlobal.ExibirRodapeGlobal("Selecionar Reserva");

        return reservaSelecionada;
    }



}