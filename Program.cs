using System;
using System.Text;
using System.Threading;
using System.Globalization;
using DesafioProjetoHospedagem.Models;
using DesafioProjetoHospedagem.Utils;
using DesafioProjetoHospedagem.Menus;


// Cultura e acentuação
Console.OutputEncoding = Encoding.UTF8;
Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-BR");

// Animação inicial
ExibirTelaDeCarregamento();

/*// Listas principais
List<EspacoHospedagem> espacos = new();
List<Cliente> clientes = new();
List<Cliente> clientesDaReserva = new(); // caso use no futuro
List<Reserva> reservas = new();
Reserva? reservaSelecionada = null;
*/

// Chamada principal do sistema
MenuPrincipal.ExibirPrincipal();

// Método auxiliar
static void ExibirTelaDeCarregamento()
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Yellow;
    InterfaceHelper.EscreverComAnimacao("Iniciando sistema . . . ");
    Console.ResetColor();

    Thread.Sleep(300);
    Console.WriteLine("\n");
}
