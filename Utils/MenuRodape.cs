using System;
using DesafioProjetoHospedagem.Menus;
using DesafioProjetoHospedagem.Models;

namespace DesafioProjetoHospedagem.Utils
{
    /// <summary>
    /// Exibe o rodapé horizontal com atalhos padrão (0, 8, 9) e interpreta entrada do usuário.
    /// Aceita números, letras ("S"/"N") e entrada inválida com mensagens emocionais.
    /// </summary>
    public static class MenuRodape
    {
        //menurodape para rotas em voids

        /*//como usar:
    MenuRodape.ExibirRodape(
    "Cadastro de Espaços",
    aoVoltarParaSubmenu: () => MenuEspacos.ExibirMenuEspacos(),
    aoVoltarMenuPrincipal: () => MenuPrincipal.ExibirPrincipal()
    );
    */
        public static void ExibirRodape(
        string nomeSubmenu,
        Action? aoVoltarParaSubmenu = null,
        Action? aoVoltarMenuPrincipal = null)
        {
            int enterCount = 0;

            while (true)
            {
                Console.WriteLine(); // Espaço antes do rodapé

                // Linha divisória superior
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(new string('─', Console.WindowWidth));
                Console.ResetColor();
                Console.WriteLine();

                // Frase "Digite aqui sua opção..." + entrada centralizada
                string frase = "8.voltar  9.Menu principal  0.Encerrar ? ";
                int largura = Console.WindowWidth;
                int margemFrase = (largura - frase.Length) / 2;

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(new string(' ', margemFrase) + frase);
                Console.ResetColor();

                string? entrada = Console.ReadLine()?.Trim();

                Console.WriteLine(); // Espaço

                // Rodapé com atalhos centralizado
                string rodapeTexto = "8.Voltar   9.Menu principal   0.Encerrar";
                int margemRodape = (largura - rodapeTexto.Length) / 2;

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(new string(' ', margemRodape) + rodapeTexto);
                Console.ResetColor();
                Console.WriteLine(); // Espaço final

                // Validação da entrada
                if (string.IsNullOrEmpty(entrada))
                {
                    enterCount++;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Use 8, 9 ou 0 para navegar.");
                    Console.ResetColor();

                    if (enterCount >= 3)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Use o menu do rodapé para navegar.");
                        Console.ResetColor();
                        Environment.Exit(0);
                        return;
                    }

                    continue;
                }

                enterCount = 0; // zera contagem

                if (int.TryParse(entrada, out int atalho))
                {
                    switch (atalho)
                    {
                        case 0:
                            Console.WriteLine("Saindo do sistema...");
                            Environment.Exit(0);
                            return;
                        case 8:
                            aoVoltarParaSubmenu?.Invoke();
                            return;
                        case 9:
                            aoVoltarMenuPrincipal?.Invoke();
                            return;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Opção inválida. Use 0, 8 ou 9.");
                            Console.ResetColor();
                            break;
                    }
                }
                else
                {
                    string escolha = entrada.ToUpper();
                    if (escolha == "S")
                    {
                        aoVoltarParaSubmenu?.Invoke();
                        return;
                    }
                    else if (escolha == "N")
                    {
                        aoVoltarMenuPrincipal?.Invoke();
                        return;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Ops! Digite um número válido ou S/N.");
                        Console.ResetColor();
                    }
                }
            }
        }



    }
}




