using System;
using DesafioProjetoHospedagem.Models;
using DesafioProjetoHospedagem.Utils;
using DesafioProjetoHospedagem.Controladoras;
using DesafioProjetoHospedagem.Menus;
using System.Text;

namespace DesafioProjetoHospedagem.Utils
{
    /*  
    //Como usar: em paginas que vão acessar metodos globais, como listarEspacoGlobal
        Dentro do menu que está chamando uma tela como ListarEspacosGlobal, você faz:
        NavegacaoAtual.Registrar(() => MenuReservas.ExibirCriarReserva());
        ListarEspacosGlobal();
        //Isso informa ao sistema: “se o usuário pressionar 8 depois, volte para Criar Reserva”.
    */
    public static class MenuRodapeGlobal
    {
        public static void ExibirRodapeGlobal(string nomeSubmenu)
        {
            int enterCount = 0;

            while (true)
            {
                Console.WriteLine(); // espaço após conteúdo principal

                // Linha divisória horizontal
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(new string('─', Console.WindowWidth));
                Console.ResetColor();
                Console.WriteLine();

                // Frase centralizada: "Digite aqui sua opção do rodapé:"
                string fraseRodape = "8.Voltar  9.Menu principal  0.Encerrar";
                int largura = Console.WindowWidth;
                int margemFrase = (largura - fraseRodape.Length) / 2;

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(new string(' ', margemFrase) + fraseRodape + " ");
                Console.ResetColor();

                // Entrada do usuário
                Console.ForegroundColor = ConsoleColor.Cyan;
                string? entrada = Console.ReadLine()?.Trim();
                Console.ResetColor();

                Console.WriteLine(); // espaço após entrada

                // Rodapé com atalhos centralizado
                string rodapeTexto = "8.Voltar  9.Menu principal  0.Encerrar";
                int margemRodape = (largura - rodapeTexto.Length) / 2;

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(new string(' ', margemRodape) + rodapeTexto);
                Console.ResetColor();

                Console.WriteLine(); // espaço extra final

                if (string.IsNullOrEmpty(entrada))
                {
                    enterCount++;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Ops. Escolha: 8.Voltar  9.Menu principal  0.Encerrar");
                    Console.ResetColor();

                    if (enterCount >= 3)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Ops... Se deseja sair escolha:8.Voltar  9.Menu principal  0.Encerrar");
                        Console.ResetColor();
                        enterCount = 0; // zera sem encerrar
                        continue;

                    }

                    continue;
                }

                enterCount = 0; // zera contador
                Console.Clear();
                CentralSaida.CabecalhoMenus(nomeSubmenu.ToUpper());

                switch (entrada)
                {
                    case "8":
                        NavegacaoAtual.Voltar();
                        return;
                    case "9":
                        NavegacaoAtual.Limpar();
                        MenuPrincipal.ExibirPrincipal();
                        return;
                    case "0":
                        Console.WriteLine("\nSistema encerrado. Até breve !!!");
                        Environment.Exit(0);
                        return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Ops. Escolha: 8.Voltar  9.Menu principal  0.Encerrar");
                        Console.ResetColor();
                        break;
                }

            }

        }




    }
}

public static class NavegacaoAtual
{
    private static Stack<Action> PilhaMenus = new();

    public static void Registrar(Action menuAnterior)
    {
        PilhaMenus.Push(menuAnterior);
    }
    public static void Voltar()
    {
        if (PilhaMenus.Count > 0)
        {
            var voltarPara = PilhaMenus.Pop();
            voltarPara?.Invoke();
        }
        else
        {
            Console.WriteLine("\nNenhum menu anterior registrado. Voltando ao principal...");
            MenuPrincipal.ExibirPrincipal(); // Certifique-se que está acessível
        }
    }

    public static void Limpar()
    {
        PilhaMenus.Clear();
    }
}








