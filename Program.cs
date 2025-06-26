using System;
using System.Text;
using System.Threading;
using DesafioProjetoHospedagem.Models;
using Menus;

// Listas principais
List<EspacoHospedagem> espacos = new();
List<Hospede> hospedes = new();
List<Hospede> hospedesDaReserva = new();
List<Reserva> reservas = new();

Reserva? reservaSelecionada = null;

// Configura UTF-8 para acentuação correta
Console.OutputEncoding = Encoding.UTF8;

Console.Clear();
Console.WriteLine("Iniciando sistema...");
Thread.Sleep(2000);

// Chamada central do sistema
MenuPrincipal.Exibir(espacos, hospedes, reservas, hospedesDaReserva, reservaSelecionada);
