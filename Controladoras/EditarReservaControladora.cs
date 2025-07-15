using System;
using System.Collections.Generic;
using DesafioProjetoHospedagem.Models;
using DesafioProjetoHospedagem.Utils;
using DesafioProjetoHospedagem.Controladoras;
using DesafioProjetoHospedagem.Sessao;
using DesafioProjetoHospedagem.Menus;

namespace DesafioProjetoHospedagem.Controladoras
{
    public static class EditarReservaControladora
    {
        public static void ContinuarEdicao(Reserva reserva)
        {
            CentralSaida.LimparTela("Editar Reserva");
            CentralSaida.CabecalhoMenus("Editar Reserva");

            if (reserva == null)
            {
                CentralSaida.Exibir(TipoMensagem.Alerta, "Reserva não encontrada.");
                MenuRodapeGlobal.ExibirRodapeGlobal("EditarReserva");
                return;
            }

            Console.WriteLine($"Reserva atual: {reserva.Espaco.NomeFantasia} | Check-in: {reserva.DataCheckIn:dd/MM/yyyy}");

            Console.WriteLine("\nDeseja editar o espaço? (s/n)");
            string? editarEspaco = Console.ReadLine()?.ToLower();

            if (editarEspaco == "s")
            {
                var novoEspaco = MenuEspacos.SelecionarEspaco(); // método que retorna um espaço válido
                reserva.Espaco = novoEspaco;
                CentralSaida.Exibir(TipoMensagem.Sucesso, "Espaço atualizado com sucesso!");
            }

            Console.WriteLine("\nDeseja editar a data de check-in? (s/n)");
            string? editarData = Console.ReadLine()?.ToLower();

            if (editarData == "s")
            {
                reserva.DataCheckIn = CentralEntradaData.LerData("Digite a nova data de check-in:");
                CentralSaida.Exibir(TipoMensagem.Sucesso, "Data de check-in atualizada com sucesso!");
            }

            Console.WriteLine("\nDeseja vincular novos clientes à reserva? (s/n)");
            string? editarClientes = Console.ReadLine()?.ToLower();

            if (editarClientes == "s")
            {
                var novosClientes = TelaClientes.SelecionarMultiplos();
                reserva.VincularClientes(novosClientes);
                CentralSaida.Exibir(TipoMensagem.Sucesso, "Clientes atualizados com sucesso!");
            }

            Console.WriteLine("\nEdição concluída.");

            MenuRodapeGlobal.ExibirRodapeGlobal("EditarReserva");
        }
    }
}
