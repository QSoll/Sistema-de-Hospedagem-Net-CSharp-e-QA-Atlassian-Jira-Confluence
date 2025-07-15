using DesafioProjetoHospedagem.Models;
using DesafioProjetoHospedagem.Utils;
using DesafioProjetoHospedagem.Menus;
using DesafioProjetoHospedagem.Servicos;
using System;
using System.Globalization;
using DesafioProjetoHospedagem.Controladoras;
using DesafioProjetoHospedagem.Sessao;

namespace DesafioProjetoHospedagem.Controladoras
{
    public static class EspacoControlador
    {
        public static void CadastrarEspaco()
        {
            NavegacaoAtual.Registrar(() => MenuPrincipal.ExibirPrincipal());

            CentralSaida.LimparTela("CADASTRO DE ESPAÇO");
            CentralSaida.CabecalhoMenus("CADASTRO DE ESPAÇOS");

            string tipoEspaco = CentralEntrada.LerTextoObrigatorio("Tipo do espaço (Ex: Quarto, Suíte, Chalé)");

            string tipoAbreviado = tipoEspaco.Length >= 3
            ? tipoEspaco.Substring(0, 3).ToUpper()
            : tipoEspaco.ToUpper();

            int id = SessaoAtual.Espacos.Count + 1;

            string nomeFantasia = CentralEntrada.LerTextoObrigatorio("Nome fantasia");
            int capacidade = CentralEntradaControladora.LerInteiroMinMax("Capacidade máxima para quantas pessoas ?", 1, 30);
            decimal valorDiaria = CentralEntrada.LerValorMonetario("Valor da diária");
            int quantidadeQuartos = CentralEntradaControladora.LerInteiroMinMax("Quantidade de quartos", 1, 30);

            bool possuiSuite = CentralEntradaControladora.SimNao("Possui suíte?");
            int quantidadeSuites = 0;

            if (possuiSuite)
            {
                while (true)
                {
                    quantidadeSuites = CentralEntradaControladora.LerInteiroMinMax("Quantidade de suítes", 0, 30);

                    if (quantidadeSuites <= quantidadeQuartos)
                        break;

                    CentralSaida.Exibir(TipoMensagem.Erro, "Quantidade de suítes não pode ser maior que a de quartos.");
                }
            }

            string descricao = CentralEntrada.LerTextoObrigatorio("Descrição do espaço");

            EspacoHospedagem novoEspaco = new EspacoHospedagem(
                tipoEspaco,
                id,
                tipoAbreviado,
                nomeFantasia,
                capacidade,
                valorDiaria,
                quantidadeQuartos,
                possuiSuite,
                quantidadeSuites,
                descricao
            );

            SessaoAtual.Espacos.Add(novoEspaco);
            Console.Clear();
            CentralSaida.CabecalhoMenus("MENU CADASTRO DE ESPAÇOS");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Sucesso !!! Espaço cadastrado.");
            Console.WriteLine("");
            Console.ResetColor();


            Console.WriteLine(); // espaço acima da linha
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(new string('─', Console.WindowWidth));
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(novoEspaco.TipoEspaco.ToUpper());
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(new string('─', Console.WindowWidth));
            Console.ResetColor();
            // espaço abaixo da linha

            Console.WriteLine($"{novoEspaco.TipoAbreviado.ToUpper()} {novoEspaco.Id} - {novoEspaco.NomeFantasia}");
            Console.WriteLine($"Capacidade: {novoEspaco.Capacidade} pessoa{(novoEspaco.Capacidade > 1 ? "s" : "")}");
            Console.WriteLine($"Diária: R$ {novoEspaco.ValorDiaria:N2}");
            Console.WriteLine($"{novoEspaco.QuantidadeQuartos} quarto{(novoEspaco.QuantidadeQuartos > 1 ? "s" : "")}, sendo {(novoEspaco.PossuiSuite ? novoEspaco.QuantidadeSuites : 0)} suíte{(novoEspaco.QuantidadeSuites > 1 ? "s" : "")}.");
            Console.WriteLine($"Descrição: {(string.IsNullOrWhiteSpace(novoEspaco.Descricao) ? "Não informada." : novoEspaco.Descricao)}");
            // Perguntar se deseja cadastrar outro
            bool cadastrarOutro = CentralEntradaControladora.SimNao("\nDeseja cadastrar outro espaço?");
            if (cadastrarOutro)
            {
                EspacoControlador.CadastrarEspaco();
                return;
            }

            CentralSaida.Exibir(TipoMensagem.Informacao, "Use o rodapé para navegar !!!");

            MenuRodapeGlobal.ExibirRodapeGlobal("Menu Principal");

            return;
        }

        public static void EditarCadastroEspaco()
        {
            CentralSaida.LimparTela("Editar Espaço");
            CentralSaida.CabecalhoMenus("Editar Cadastro de Espaço");

            if (SessaoAtual.Espacos.Count == 0)
            {
                CentralSaida.Exibir(TipoMensagem.Alerta, "Nenhum espaço cadastrado para editar.");
                MenuRodape.ExibirRodape(
                    "Cadastro de Espaços",
                    aoVoltarParaSubmenu: () => MenuEspacos.ExibirMenuEspacos(),
                    aoVoltarMenuPrincipal: () => MenuPrincipal.ExibirPrincipal()
                );
                return;
            }

            Console.WriteLine("Selecione o espaço que deseja editar:\n");
            for (int i = 0; i < SessaoAtual.Espacos.Count; i++)
            {
                var espacoListagem = SessaoAtual.Espacos[i]; // nome diferente para evitar conflito
                Console.WriteLine($"[{i + 1}] {espacoListagem.Id} - {espacoListagem.TipoEspaco.ToUpper()} {espacoListagem.NomeFantasia}");
            }


            int escolha = CentralEntradaControladora.LerInteiroMinMaxComSN(
                "Escolha o número do espaço",
                1,
                SessaoAtual.Espacos.Count,
                "Cadastro de Espaços",
                () => MenuEspacos.ExibirMenuEspacos(),
                () => MenuPrincipal.ExibirPrincipal()
            );

            if (escolha == 0) return;

            EspacoHospedagem espaco = SessaoAtual.Espacos[escolha - 1];
            bool continuarEditando = true;

            while (continuarEditando)
            {
                Console.Clear();
                CentralSaida.CabecalhoMenus($"Editando: {espaco.Id} - {espaco.TipoEspaco.ToUpper()} {espaco.NomeFantasia}");

                Console.WriteLine($"\nTipo do espaço: {espaco.TipoEspaco}");
                Console.WriteLine($"Nome Fantasia: {espaco.NomeFantasia}");
                Console.WriteLine($"Capacidade: {espaco.Capacidade} pessoa{(espaco.Capacidade > 1 ? "s" : "")}");
                Console.WriteLine($"Valor da Diária: {espaco.ValorDiaria.ToString("C", new CultureInfo("pt-BR"))}");
                Console.WriteLine($"Quantidade de Quartos: {espaco.QuantidadeQuartos}");
                Console.WriteLine($"Possui Suíte: {(espaco.PossuiSuite ? "Sim" : "Não")}");
                Console.WriteLine($"Quantidade de Suítes: {espaco.QuantidadeSuites}");
                Console.WriteLine($"Descrição: {espaco.Descricao}");
                Console.WriteLine();

                Console.WriteLine("Qual campo deseja editar?");
                Console.WriteLine("1- Tipo do espaço");
                Console.WriteLine("2- Nome Fantasia");
                Console.WriteLine("3- Capacidade");
                Console.WriteLine("4- Valor da Diária");
                Console.WriteLine("5- Quantidade de Quartos");
                Console.WriteLine("6- Possui Suíte");
                Console.WriteLine("7- Quantidade de Suítes");
                Console.WriteLine("8- Descrição");
                Console.WriteLine("0- Finalizar edição");

                int opcao = CentralEntradaControladora.LerInteiroMinMaxComSN(
                    "Digite a opção",
                    0,
                    8,
                    "Editar Espaço",
                    () => MenuEspacos.ExibirMenuEspacos(),
                    () => MenuPrincipal.ExibirPrincipal()
                );

                switch (opcao)
                {
                    case 1:
                        espaco.TipoEspaco = CentralEntrada.LerTextoObrigatorio("Novo tipo do espaço");
                        break;
                    case 2:
                        espaco.NomeFantasia = CentralEntrada.LerTextoObrigatorio("Novo nome fantasia");
                        break;
                    case 3:
                        espaco.Capacidade = CentralEntradaControladora.LerInteiroMinMax("Nova capacidade", 1, 100);
                        break;
                    case 4:
                        string diariaStr = CentralEntrada.LerTextoObrigatorio("Nova diária");
                        if (decimal.TryParse(diariaStr.Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture, out decimal novaDiaria))
                            espaco.ValorDiaria = novaDiaria;
                        else
                            CentralSaida.Exibir(TipoMensagem.Erro, "Valor inválido. Diária mantida.");
                        break;
                    case 5:
                        espaco.QuantidadeQuartos = CentralEntradaControladora.LerInteiroMinMax("Nova quantidade de quartos", 0, 50);
                        break;
                    case 6:
                        bool possuiSuite = CentralEntradaControladora.SimNao("Este espaço possui suíte?");

                        espaco.QuantidadeSuites = possuiSuite
                            ? CentralEntradaControladora.LerInteiroMinMax("Quantidade de suítes", 1, 100)
                            : 0;
                        break;
                    case 7:
                        espaco.QuantidadeSuites = CentralEntradaControladora.LerInteiroMinMax("Nova quantidade de suítes", 0, 10);
                        break;
                    case 8:
                        espaco.Descricao = CentralEntrada.LerTextoObrigatorio("Nova descrição do espaço");
                        break;
                    case 0:
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\nSucesso !!! Edição concluída.");
                        Console.ResetColor();

                        CentralSaida.CabecalhoMenus("Resumo Atualizado do Espaço");
                        EspacoControlador.ExibirResumoEspacoCadastrado(espaco);

                        continuarEditando = CentralEntradaControladora.SimNao("\nDeseja editar mais alguma informação?");
                        break;
                }

                if (opcao != 0 && continuarEditando)
                {
                    continuarEditando = CentralEntradaControladora.SimNao("Deseja editar outro campo?");
                }
            }
        }

        //exibe o resumo do cadastro de espaços
        public static void ExibirResumoEspacoCadastrado(EspacoHospedagem espaco)
        {
            CentralSaida.CabecalhoMenus("Resumo Espaço cadastrado");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{espaco.Id:D3} - {espaco.TipoEspaco.ToUpper()} {espaco.NomeFantasia}");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(new string('─', Console.WindowWidth));
            Console.ResetColor();

            Console.WriteLine($"Capacidade: {espaco.Capacidade} pessoa{(espaco.Capacidade > 1 ? "s" : "")}");
            Console.WriteLine($"Diária: R$ {espaco.ValorDiaria:N2}");
            Console.WriteLine($"{espaco.QuantidadeQuartos} quarto{(espaco.QuantidadeQuartos > 1 ? "s" : "")}, sendo {(espaco.PossuiSuite ? espaco.QuantidadeSuites : 0)} suíte{(espaco.QuantidadeSuites > 1 ? "s" : "")}");
            Console.WriteLine($"Descrição: {(string.IsNullOrWhiteSpace(espaco.Descricao) ? "Não informada." : espaco.Descricao)}");

            MenuRodape.ExibirRodape(
            "Cadastro de Espaços",
            aoVoltarParaSubmenu: () => MenuEspacos.ExibirMenuEspacos(),
            aoVoltarMenuPrincipal: () => MenuPrincipal.ExibirPrincipal()
            );
        }
        public static void ExcluirCadastroEspaco()

        {
            CentralSaida.LimparTela("Excluir Espaço");
            CentralSaida.CabecalhoMenus("Exclusão de Espaço");

            if (SessaoAtual.Espacos.Count == 0)
            {
                CentralSaida.Exibir(TipoMensagem.Alerta, "Nenhum espaço cadastrado para excluir.");
                MenuRodape.ExibirRodape(
                "Cadastro de Espaços",
                aoVoltarParaSubmenu: () => MenuEspacos.ExibirMenuEspacos(),
                aoVoltarMenuPrincipal: () => MenuPrincipal.ExibirPrincipal()
                );
            }

            Console.WriteLine("Selecione o espaço que deseja excluir:\n");

            for (int i = 0; i < SessaoAtual.Espacos.Count; i++)
            {
                string valorFormatado = SessaoAtual.Espacos[i].ValorDiaria.ToString("C", new CultureInfo("pt-BR"));
                Console.WriteLine($"[{i + 1}] {SessaoAtual.Espacos[i].NomeFantasia} - {valorFormatado}");
            }
            try
            {
                int escolha = CentralEntradaControladora.LerInteiroMinMaxComSN(
                    "Digite o número do espaço a ser excluído:",
                    1, SessaoAtual.Espacos.Count, "Menu – Espaços de Hospedagem",
                    () => MenuEspacos.ExibirMenuEspacos(),
                    () => MenuPrincipal.ExibirPrincipal()
                );

                EspacoHospedagem espaco = SessaoAtual.Espacos[escolha - 1];

                bool ocupado = SessaoAtual.Reservas.Any(r => r.Ativa && r.Espaco.Id == espaco.Id);

                if (ocupado)
                {
                    CentralSaida.Exibir(TipoMensagem.Erro, "Este espaço está vinculado a uma reserva ativa e não pode ser excluído.");
                }
                else
                {
                    SessaoAtual.Espacos.Remove(espaco);
                    CentralSaida.Exibir(TipoMensagem.Sucesso, $"Espaço \"{espaco.NomeFantasia}\" removido com sucesso!");
                }
            }
            catch (Exception ex)
            {
                CentralSaida.Exibir(TipoMensagem.Erro, $"Ocorreu um erro inesperado ao excluir o espaço: {ex.Message}");
                MenuRodape.ExibirRodape(
                "Cadastro de Espaços",
                aoVoltarParaSubmenu: () => MenuEspacos.ExibirMenuEspacos(),
                aoVoltarMenuPrincipal: () => MenuPrincipal.ExibirPrincipal()
                );
            }

        }

        public static EspacoHospedagem BuscarEspacoPorFiltro()
        {
            while (true)
            {
                CentralSaida.Exibir(TipoMensagem.Instrucao,
                    "Digite o código ou nome do espaço (ex: CHA 001 ou CHA Albatroz):");

                string entrada = Console.ReadLine()?.Trim().ToUpper() ?? "";

                if (string.IsNullOrWhiteSpace(entrada))
                {
                    CentralSaida.Exibir(TipoMensagem.Erro, "Entrada inválida. Por favor, digite um código ou nome válido.");
                    continue; // força nova tentativa
                }

                var espacoEncontrado = SessaoAtual.Espacos.FirstOrDefault(e =>
                e.Id.ToString().Contains(entrada, StringComparison.OrdinalIgnoreCase) ||
                e.NomeFantasia.Contains(entrada, StringComparison.OrdinalIgnoreCase));

                if (espacoEncontrado != null)
                    return espacoEncontrado;

                CentralSaida.Exibir(TipoMensagem.Alerta, $"Nenhum espaço correspondente a '{entrada}' foi encontrado.");
            }
        }





        /*public static EspacoHospedagem BuscarEspacoPorFiltro()
        {
            Console.Clear();
            Console.WriteLine("Você pode digitar o código ou nome (ex: CHA 001 ou CHA Albatroz), ou pressione Enter para buscar por filtros:");
            string? entrada = Console.ReadLine()?.Trim();

            if (!string.IsNullOrWhiteSpace(entrada))
            {
                var espaco = BuscarEspacoPorCodigoOuNome(entrada.ToUpper());
                if (espaco != null)
                    return espaco;
                else
                    CentralSaida.Exibir(TipoMensagem.Alerta, "Espaço não encontrado. Vamos filtrar por características...");
            }

            CentralSaida.CabecalhoMenus("Busca de Espaços por Filtros");

            string tipoBuscado = CentralEntrada.LerTextoObrigatorio("Tipo de espaço desejado");

            int minQuartos = CentralEntradaControladora.LerInteiroMinMax(
                "Quantidade mínima de quartos", 0, 20);

            int minSuites = CentralEntradaControladora.LerInteiroMinMax(
                "Quantidade mínima de suítes", 0, 20);

            var espacosFiltrados = SessaoAtual.Espacos
                .Where(e =>
                    e.QuantidadeQuartos >= minQuartos &&
                    e.QuantidadeSuites >= minSuites &&
                    e.Status.Equals("LIVRE", StringComparison.OrdinalIgnoreCase)
                )
                .ToList();

            if (!espacosFiltrados.Any())
            {
                CentralSaida.Exibir(TipoMensagem.Alerta,
                    "Nenhum espaço atende aos critérios mínimos informados.");

                MenuRodape.ExibirRodape("Menu | Espaços");

                return new EspacoHospedagem(
                "Indefinido",     // tipoEspaco
                0,                // id
                "IND",            // tipoAbreviado
                "Espaço padrão",  // nomeFantasia
                0,                // capacidade
                0m,               // valorDiaria
                0,                // quantidadeQuartos
                false,            // possuiSuite
                0,                // quantidadeSuites
                "Espaço gerado por padrão" // descricao
                );

            }

            Console.WriteLine("\nEspaços disponíveis que atendem aos critérios mínimos:");

            var agrupadosPorTipo = espacosFiltrados
                .GroupBy(e => e.TipoEspaco.ToUpper())
                .OrderBy(g => g.Key);

            foreach (var grupo in agrupadosPorTipo)
            {
                Console.WriteLine($"\n{grupo.Key}");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(new string('─', Console.WindowWidth));
                Console.ResetColor();

                int contador = 1;
                foreach (var espaco in grupo)
                {
                    Console.WriteLine($"{contador} - {espaco.NomeFantasia} (ID: {espaco.Id})");
                    Console.WriteLine($"Capacidade: {espaco.Capacidade} pessoa{(espaco.Capacidade > 1 ? "s" : "")}");
                    Console.WriteLine($"Quartos: {espaco.QuantidadeQuartos} | Suítes: {espaco.QuantidadeSuites}");
                    Console.WriteLine($"Valor da Diária: {espaco.ValorDiaria:C}");
                    Console.WriteLine($"Status: {(espaco.Status == "LIVRE" ? " Livre" : "Ocupado")}");
                    Console.WriteLine(); // espaço entre blocos
                    contador++;
                }
            }

            int idEscolhido = CentralEntradaControladora.LerInteiroMinMaxComSN(
                "Digite o ID do espaço que deseja vincular",
                espacosFiltrados.Min(e => e.Id),
                espacosFiltrados.Max(e => e.Id),
                "Reserva de Espaços",
                () => MenuReservas.ExibirMenuReservas(),
                () => MenuPrincipal.ExibirPrincipal()
            );

            // ✅ Espaço selecionado via ID
            var espacoSelecionado = espacosFiltrados.First(e => e.Id == idEscolhido);

            // ✅ Guardar espaço na sessão para uso reverso
            //aoAtual.EspacoSelecionado = espacoSelecionado;

            CentralSaida.Exibir(
                TipoMensagem.Informativo,
                $"Espaço \"{espacoSelecionado.NomeFantasia}\" selecionado. Continue com o fluxo para criar a reserva."
            );

            //✅ Redireciona para o menu de reservas (onde CriarReserva //usará esse espaço automaticamente)
           //enuReservas.ExibirMenuReservas();

            //CentralSaida.Exibir(
                //TipoMensagem.Sucesso,
                //$"Espaço \"{espacoSelecionado.NomeFantasia}\" vinculado //com sucesso!");


            //MenuRodape.ExibirRodape("Menu | Espaços");
            


            //return espacoSelecionado;
        }
        */

        public static EspacoHospedagem? BuscarEspacoPorCodigoOuNome(string entrada)
        {
            if (string.IsNullOrWhiteSpace(entrada))
                return null;

            entrada = entrada.Trim().ToUpper();

            return SessaoAtual.Espacos.FirstOrDefault(e =>
                entrada == $"{e.TipoAbreviado} {e.Id:D3}" || // Ex: CHA 001
                entrada == $"{e.TipoAbreviado} {e.NomeFantasia.ToUpper()}" // Ex: CHA Albatroz
            );

        }

    }
}
