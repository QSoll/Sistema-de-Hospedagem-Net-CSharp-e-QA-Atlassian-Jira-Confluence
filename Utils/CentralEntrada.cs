using System;
using System.Globalization;
using DesafioProjetoHospedagem.Models;
using DesafioProjetoHospedagem.Utils;
using DesafioProjetoHospedagem.Menus;
using DesafioProjetoHospedagem.Controladoras;

namespace DesafioProjetoHospedagem.Utils
{
    //────────────────────────────────────────────
    // CentralEntrada – leitura de texto simples com tentativas e cancelamento
    //────────────────────────────────────────────
    public static class CentralEntrada
    {
        /*//LerTextoComCancelametno não é mais usado
        public static string LerTextoComCancelamento(string titulo, int tentativas = 3)
        {
            int contador = 0;
            while (contador < tentativas)
            {
                Console.WriteLine(); // espaço visual acima
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"{titulo}: ");
                Console.ResetColor();

                string? entrada = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(entrada))
                    return entrada.Trim();

                contador++;
                CentralSaida.Exibir(TipoMensagem.Instrucao, "Por favor, insira um texto válido.");
            }

            CentralSaida.MensagemDeCancelamento("Entrada de texto");
            return string.Empty;
        }*/

        // - LerTExtoObrigatorio - Solicita um texto obrigatório com validação inteligente.usados em cadastrar espaco
        public static string LerTextoObrigatorio(string pergunta)
        {
            int contadorEnter = 0;

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($"\n{pergunta}: ");
                Console.ResetColor();

                string? entrada = Console.ReadLine()?.Trim();

                if (!string.IsNullOrWhiteSpace(entrada))
                    return entrada;

                contadorEnter++;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nOps, campo obrigatório. Digite novamente.");
                Console.ResetColor();

                if (contadorEnter >= 3)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"\nVocê pressionou ENTER {contadorEnter} vezes.");
                    Console.WriteLine("Digite novamente ou pressione ENTER para seguir em frente.");
                    Console.ResetColor();

                    Console.ReadLine(); // aguarda ENTER ou nova tentativa
                    return string.Empty;
                }
            }
        }

        //LerValorMonetario - Lê um valor decimal representando uma quantia financeira - usado em espacocontrolador / CadastrarEspaco
        public static decimal LerValorMonetario(string pergunta)
        {
            CultureInfo culturaBrasil = new CultureInfo("pt-BR");

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($"\n{pergunta} (Ex: 150,50): ");
                Console.ResetColor();

                string? entrada = Console.ReadLine()?.Trim();

                if (decimal.TryParse(entrada, NumberStyles.Number, culturaBrasil, out decimal valor))
                {
                    if (valor > 0)
                        return valor;

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nO valor deve ser maior que zero. Diárias gratuitas não são permitidas.");
                    Console.ResetColor();
                    continue;
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nValor inválido! Digite um número positivo com vírgula para centavos (Ex: 150,50).");
                Console.ResetColor();
            }
        }

        /*//LerValorMonetarioComEscape não é mais usado
        public static decimal LerValorMonetarioComEscape(string pergunta, ref int contadorVazio)
        {
            CultureInfo culturaBrasil = new CultureInfo("pt-BR");

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($"\n{pergunta} (Ex: 150,50): ");
                Console.ResetColor();

                string? entrada = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(entrada))
                {
                    contadorVazio++;

                    if (contadorVazio >= 3)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\nDeseja voltar ao menu anterior? (s/n)");
                        Console.ResetColor();

                        string? resposta = Console.ReadLine()?.Trim().ToLower();

                        if (resposta == "s")
                            throw new OperacaoCanceladaException();

                        contadorVazio = 0;
                        continue;
                    }

                    CentralSaida.Exibir(TipoMensagem.Erro, "Campo obrigatório. Digite um valor no formato correto.");
                    Thread.Sleep(1200);
                    continue;
                }

                contadorVazio = 0;

                if (decimal.TryParse(entrada, NumberStyles.Number, culturaBrasil, out decimal valor) && valor >= 0)
                    return valor;

                CentralSaida.Exibir(TipoMensagem.Erro, "Valor inválido! Digite um número decimal positivo (ex: 150,50).");
                Thread.Sleep(1200);
            }
        }
        */

        //LerDataObrigatoria - Lê uma data obrigatória no formato válido.
                public static DateTime LerDataObrigatoria(string mensagem)
        {
            int contadorVazio = 0;

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($"\n{mensagem} (formato: DD/MM/AAAA): ");
                Console.ResetColor();

                string? entrada = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(entrada))
                {
                    contadorVazio++;

                    if (contadorVazio >= 3)
                    {
                        bool desejaSair = CentralEntradaControladora.SimNao("Você pressionou ENTER 3 vezes.\nDeseja voltar ao menu anterior?");
                        if (desejaSair)
                            throw new OperacaoCanceladaException();

                        contadorVazio = 0; // Reinicia contador após resposta
                        continue;
                    }

                    CentralSaida.Exibir(TipoMensagem.Erro, "Campo obrigatório. Digite uma data no formato DD/MM/AAAA.");
                    Thread.Sleep(1200);
                    continue;
                }

                contadorVazio = 0;

                if (DateTime.TryParseExact(entrada, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime data))
                    return data;

                CentralSaida.Exibir(TipoMensagem.Erro, "Data inválida. Use o formato DD/MM/AAAA.");
                Thread.Sleep(1200);
            }
        }


        /*//LertextoComEscape não é mais usado
        public static string LerTextoComEscape(string pergunta, ref int contadorVazio)
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($"\n{pergunta}: ");
                Console.ResetColor();

                string? entrada = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(entrada))
                {
                    contadorVazio++;

                    if (contadorVazio >= 3)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\nVocê deseja voltar ao menu anterior? (s/n)");
                        Console.ResetColor();

                        string? resposta = Console.ReadLine()?.Trim().ToLower();
                        if (resposta == "s")
                            throw new OperacaoCanceladaException();

                        contadorVazio = 0;
                        continue;
                    }

                    CentralSaida.Exibir(TipoMensagem.Erro, "Campo obrigatório. Digite algum texto válido.");
                    Thread.Sleep(1200);
                    continue;
                }

                contadorVazio = 0;
                return entrada;
            }
        }
        */

        /*//LerNumeroInteiroComEscape não é mais usado
        public static int LerNumeroInteiroComEscape(string pergunta, ref int contadorVazio)
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($"\n{pergunta}: ");
                Console.ResetColor();

                string? entrada = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(entrada))
                {
                    contadorVazio++;

                    if (contadorVazio >= 3)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\nVocê deseja voltar ao menu anterior? (s/n)");
                        Console.ResetColor();

                        string? resposta = Console.ReadLine()?.Trim().ToLower();
                        if (resposta == "s")
                            throw new OperacaoCanceladaException();

                        contadorVazio = 0;
                        continue;
                    }

                    CentralSaida.Exibir(TipoMensagem.Erro, "Campo obrigatório. Digite um número inteiro válido.");
                    Thread.Sleep(1200);
                    continue;
                }

                contadorVazio = 0;

                if (int.TryParse(entrada, out int numero))
                    return numero;

                CentralSaida.Exibir(TipoMensagem.Erro, "Valor inválido! Digite apenas números inteiros.");
                Thread.Sleep(1200);
            }
        }
        */

        //acima usados em cadastrarespaco
        internal static void ExibirPerguntaOpcao(string mensagem, string[] opcoes)
        {
            CentralSaida.CabecalhoMensagem(mensagem);

            for (int i = 0; i < opcoes.Length; i++)
                Console.WriteLine($"{i + 1} - {opcoes[i]}");

            CentralSaida.LinhaSeparadora();
        }

    }

    // OperacaoCanceladaException - usada em cadastro de espacos
    public class OperacaoCanceladaException : Exception
    {
        public OperacaoCanceladaException() : base("Operação cancelada pelo usuário.") { }
    }

    //────────────────────────────────────────────
    //CentralEntradaInteira – números inteiros com validação e cancelamento
    //────────────────────────────────────────────
    public static class CentralEntradaInteira
    {
        // L
        public static int? LerComCancelamento(string titulo, int tentativas = 3)
        {
            for (int i = 0; i < tentativas; i++)
            {
                CentralSaida.Exibir(TipoMensagem.Instrucao, titulo);
                Console.Write("");
                string? entrada = Console.ReadLine();

                if (entrada?.Trim() == "")
                    return null;

                if (int.TryParse(entrada, out int resultado))
                    return resultado;

                CentralSaida.Exibir(TipoMensagem.Erro, "Digite um número inteiro válido.");
            }

            CentralSaida.MensagemDeCancelamento("Entrada inteira");
            return null;
        }

        public static DateTime? LerDataComCancelamento(string mensagem)
        {
            while (true)
            {
                Console.Write($"{mensagem}: ");
                string? entrada = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(entrada))
                    return null;

                if (DateTime.TryParseExact(entrada, "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out DateTime data))
                    return data;

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Data inválida. Use o formato DD/MM/AAAA.");
                Console.ResetColor();
            }
        }

        // U
        public static int UmAteLimite(
    string mensagem,
    int limiteSuperior,
    string tituloRetorno,
    Action aoVoltarParaSubmenu,
    Action aoVoltarMenuPrincipal)
        {
            int tentativasEnter = 0;

            while (true)
            {
                Console.WriteLine();
                CentralSaida.Exibir(
                    TipoMensagem.Instrucao,
                    $"{mensagem} (entre 1 e {limiteSuperior}):"
                );

                string? entrada = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(entrada))
                {
                    tentativasEnter++;

                    if (tentativasEnter >= 3)
                    {
                        CentralSaida.Exibir(TipoMensagem.Alerta, "Você pressionou ENTER 3 vezes seguidas.");
                        Console.Write("Deseja sair da página? (S/N): ");
                        string? resposta = Console.ReadLine()?.Trim().ToUpper();

                        if (resposta == "S")
                        {
                            MenuRodape.ExibirRodape(tituloRetorno, aoVoltarParaSubmenu, aoVoltarMenuPrincipal);
                            return 0;
                        }

                        tentativasEnter = 0;
                        CentralSaida.Exibir(
                            TipoMensagem.Instrucao,
                            "Tudo bem. Escolha um valor e continue."
                        );
                        continue;
                    }

                    CentralSaida.Exibir(
                        TipoMensagem.Erro,
                        "Campo obrigatório. Digite um número entre 1 e o limite permitido."
                    );
                    Thread.Sleep(1200);
                    continue;
                }

                tentativasEnter = 0;

                if (int.TryParse(entrada, out int valor))
                {
                    if (valor >= 1 && valor <= limiteSuperior)
                        return valor;

                    CentralSaida.ErroIntervalo(1, limiteSuperior);
                    Thread.Sleep(1000);
                }
                else
                {
                    CentralSaida.Exibir(
                        TipoMensagem.Erro,
                        "Entrada inválida. Digite um número inteiro."
                    );
                    Thread.Sleep(1000);
                }
            }
        }


        //────────────────────────────────────────────────────
        //CentralEntradaControladora – entrada flexivel de alto nível com lógica e controle
        //────────────────────────────────────────────────────
    }

    public static class CentralEntradaControladora
    {
        // L
        public static string? LerEmailComValidacao(string mensagem)
        {
            while (true)
            {
                Console.Write($"\n{mensagem}: ");
                string? email = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(email))
                    return null;

                if (email.Contains("@") && email.Contains(".") && email.Length >= 5)
                    return email;

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Formato de e-mail inválido. Tente novamente.");
                Console.ResetColor();
            }
        }

        /*//LerDecimalComCancelamento não é mais usado
        public static decimal? LerDecimalComCancelamento(string label)
        {
            var entrada = LerNumeroComoString(label, true, true);
            return decimal.TryParse(entrada, out decimal valor) ? valor : null;
        }
        */

        /*//LerInteiroComCancelamento não é mais usado
        public static int? LerInteiroComCancelamento(string label)
        {
            var entrada = LerNumeroComoString(label, true);
            return int.TryParse(entrada, out int valor) ? valor : null;
        }
        */

        // minmax 0 a 6
        public static int LerInteiroMinMax(string titulo, int minimo, int maximo)
        {
            int valor;

            do
            {
                if (!string.IsNullOrWhiteSpace(titulo))
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write($"{titulo}: ");
                    Console.ResetColor();
                }

                string? entrada = Console.ReadLine();

                if (int.TryParse(entrada, out valor) && valor >= minimo && valor <= maximo)
                    return valor;

                CentralSaida.ErroIntervalo(minimo, maximo);
            }
            while (true);
        }


        /*// como usar LerInteiroMinMaxSN:
        int indice = CentralEntradaControladora.LerInteiroMinMaxComSN(
        "Digite o número do cliente que deseja excluir",
        1,
        clientes.Count,
        "Menu – Clientes",
        () => MenuClientes.ExibirMenuClientes(clientes, reservas, new List<EspacoHospedagem>(), null),
        () => MenuPrincipal.Exibir(new List<EspacoHospedagem>(), clientes, reservas, null)
        );
        */
        public static int LerInteiroMinMaxComSN(
    string mensagem,
    int minimo,
    int maximo,
    string tituloRetorno,
    Action aoVoltarParaSubmenu,
    Action aoVoltarPrincipal)
        {
            int tentativasEnter = 0;

            while (true)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"{mensagem} ({minimo} a {maximo}): ");
                Console.ResetColor();

                string? entrada = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(entrada))
                {
                    tentativasEnter++;

                    if (tentativasEnter >= 3)
                    {
                        CentralSaida.Exibir(TipoMensagem.Alerta, "\nVocê pressionou ENTER 3 vezes. Deseja sair desta página? (S/N): ");
                        string? resposta = Console.ReadLine()?.Trim().ToUpper();

                        if (resposta == "S")
                        {
                            MenuRodape.ExibirRodape(tituloRetorno, aoVoltarParaSubmenu, aoVoltarPrincipal);
                            return 0;
                        }

                        tentativasEnter = 0;
                        CentralSaida.Exibir(TipoMensagem.Instrucao, "Tudo bem. Escolha um valor válido e continue.");
                        continue;
                    }

                    CentralSaida.Exibir(TipoMensagem.Erro, "Campo obrigatório. Digite um número válido.");
                    Thread.Sleep(1200);
                    continue;
                }

                tentativasEnter = 0;

                if (int.TryParse(entrada, out int valor) && valor >= minimo && valor <= maximo)
                    return valor;

                CentralSaida.ErroIntervalo(minimo, maximo);
                Thread.Sleep(1200);
            }
        }

        /*//como usar LerTextoSN
                    string novoEmail = CentralEntradaControladora.LerTextoComSN(
                "Novo e-mail",
                "Menu – Clientes",
                () => MenuClientes.ExibirMenuClientes(clientes, reservas, espacos, reservaSelecionada),
                () => MenuPrincipal.Exibir(espacos, clientes, reservas, reservaSelecionada)
                );  
            */
        public static string LerTextoComSN(
    string campo,
    string tituloRetorno,
    Action aoVoltarSubmenu,
    Action aoVoltarPrincipal)
        {
            int tentativasEnter = 0;

            while (true)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"→ {campo}: ");
                Console.ResetColor();

                string? entrada = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(entrada))
                {
                    tentativasEnter++;

                    if (tentativasEnter >= 3)
                    {
                        CentralSaida.Exibir(TipoMensagem.Alerta, "Você pressionou ENTER 3 vezes. Deseja sair desta página? (S/N): ");
                        string? resposta = Console.ReadLine()?.Trim().ToUpper();

                        if (resposta == "S")
                        {
                            MenuRodape.ExibirRodape(tituloRetorno, aoVoltarSubmenu, aoVoltarPrincipal);
                            return "";
                        }

                        tentativasEnter = 0;
                        CentralSaida.Exibir(TipoMensagem.Instrucao, "Tudo bem. Digite um valor para continuar.");
                        continue;
                    }

                    CentralSaida.Exibir(TipoMensagem.Erro, "Campo obrigatório. Digite um valor válido.");
                    Thread.Sleep(1200);
                    continue;
                }

                return entrada;
            }
        }

        /*//lerEmailSN. como usar:
                    string novoEmail = CentralEntradaControladora.LerEmailComSN(
            "Novo e-mail",
            "Menu – Clientes",
            () => MenuClientes.ExibirMenuClientes(clientes, reservas, espacos, reservaSelecionada),
            () => MenuPrincipal.Exibir(espacos, clientes, reservas, reservaSelecionada)
            );
        */
        /// <summary>
        /// Solicita um e-mail válido com lógica emocional de ENTERs.
        /// Após 3x ENTER, oferece saída via MiniRodape (S/N).
        /// </summary>
        public static string LerEmailComSN(
    string campo,
    string tituloRetorno,
    Action aoVoltarSubmenu,
    Action aoVoltarPrincipal)
        {
            int tentativasEnter = 0;

            while (true)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"→ {campo}: ");
                Console.ResetColor();

                string? email = Console.ReadLine()?.Trim();

                // Verifica se o campo está vazio
                if (string.IsNullOrWhiteSpace(email))
                {
                    tentativasEnter++;

                    if (tentativasEnter >= 3)
                    {
                        CentralSaida.Exibir(
                            TipoMensagem.Alerta,
                            "\nVocê pressionou ENTER 3 vezes. Deseja sair desta página? (S/N): "
                        );

                        string? resposta = Console.ReadLine()?.Trim().ToUpper();

                        if (resposta == "S")
                        {
                            MenuRodape.ExibirRodape(tituloRetorno, aoVoltarSubmenu, aoVoltarPrincipal);
                            return "";
                        }

                        tentativasEnter = 0;
                        CentralSaida.Exibir(
                            TipoMensagem.Instrucao,
                            "Tudo bem. Digite um e-mail válido para continuar."
                        );
                        continue;
                    }

                    CentralSaida.Exibir(
                        TipoMensagem.Erro,
                        "Campo obrigatório. Digite um e-mail válido."
                    );
                    Thread.Sleep(1200);
                    continue;
                }

                tentativasEnter = 0;

                // Validação básica de formato de e-mail
                if (email.Contains("@") && email.Contains(".") && email.Length >= 5)
                    return email;

                CentralSaida.Exibir(
                    TipoMensagem.Erro,
                    "Formato de e-mail inválido. Tente novamente."
                );
                Thread.Sleep(1000);
            }
        }

        public static string? LerNumeroComoString(string label, bool permiteCancelar = false, bool aceitarDecimal = false)
        {
            while (true)
            {
                Console.WriteLine(); // espaço visual acima
                Console.Write($"{label}: "); // pergunta e resposta na mesma linha

                string? entrada = Console.ReadLine()?.Trim();

                if (permiteCancelar && string.IsNullOrWhiteSpace(entrada))
                    return null;

                if (aceitarDecimal)
                {
                    if (decimal.TryParse(entrada, out _))
                        return entrada;
                }
                else
                {
                    if (int.TryParse(entrada, out _))
                        return entrada;
                }

                CentralSaida.Exibir(TipoMensagem.Erro, "Entrada inválida. Digite um número válido.");
            }
        }
        public static int LerNumeroInteiro(string mensagem)
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($"\n{mensagem}: ");
                Console.ResetColor();

                string? entrada = Console.ReadLine()?.Trim();

                if (!string.IsNullOrWhiteSpace(entrada) && int.TryParse(entrada, out int numero))
                    return numero;

                // Mensagem visual de erro
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nValor inválido! Digite um número inteiro válido.");
                Console.ResetColor();
                Thread.Sleep(1200);
            }
        }

        // usada, em alguns lugares de sim nao simples com retorno amigavel
        public static bool SimNao(string mensagem)
        {
            int enterVazio = 0;
            string? resposta;

            do
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"{mensagem} (S/N): ");
                Console.ResetColor();

                resposta = Console.ReadLine()?.Trim().ToUpper();

                if (string.IsNullOrWhiteSpace(resposta))
                {
                    enterVazio++;
                    if (enterVazio == 3)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("\nPressionou ENTER 3 vezes.");
                        Console.WriteLine("Digite S para confirmar ou N para cancelar.");
                        Console.ResetColor();
                        enterVazio = 0; // Reinicia após aviso
                    }
                    continue;
                }

                enterVazio = 0; // Reset se digitou algo

                if (resposta == "S") return true;
                if (resposta == "N") return false;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Entrada inválida. Digite apenas S ou N.");
                Console.ResetColor();
            }
            while (true);
        }

        // Adicione aqui métodos que lidam com limites de tentativas, ENTER x3, menus dinâmicos, etc.
    }
}




