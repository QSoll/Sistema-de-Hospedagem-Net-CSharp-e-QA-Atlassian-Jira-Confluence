
using System;
using System.Collections.Generic;
using DesafioProjetoHospedagem.Models;
using DesafioProjetoHospedagem.Utils;
using DesafioProjetoHospedagem.Menus;
using DesafioProjetoHospedagem.Controladoras;

namespace DesafioProjetoHospedagem.Sessao
{
    public static class SessaoAtual
    {
        public static List<EspacoHospedagem> Espacos { get; set; } = new();
        public static EspacoHospedagem? EspacoSelecionado { get; set; }


        public static List<Cliente> Clientes { get; set; } = new();
        public static Cliente? ClienteSelecionado { get; set; }
        public static List<Cliente> ClientesSelecionados { get; set; } = new();


        public static List<Reserva> Reservas { get; set; } = new();

        public static Reserva? ReservaSelecionada { get; set; }

        //SanitizarDados, usar antes de menus: SessaoAtual.SanitizarDados();

        public static void SanitizarDados()
        {
            foreach (var espaco in Espacos)
            {
                if (!string.IsNullOrWhiteSpace(espaco.TipoEspaco))
                    espaco.TipoEspaco = espaco.TipoEspaco.Trim().ToUpper();

                if (!string.IsNullOrWhiteSpace(espaco.Status))
                    espaco.Status = espaco.Status.Trim().ToUpper();

                if (!string.IsNullOrWhiteSpace(espaco.NomeFantasia))
                    espaco.NomeFantasia = espaco.NomeFantasia.Trim();
            }

            foreach (var cliente in Clientes)
            {
                if (!string.IsNullOrWhiteSpace(cliente.Nome))
                    cliente.Nome = cliente.Nome.Trim();

                if (!string.IsNullOrWhiteSpace(cliente.Sobrenome))
                    cliente.Sobrenome = cliente.Sobrenome.Trim();

                if (!string.IsNullOrWhiteSpace(cliente.Email))
                    cliente.Email = cliente.Email.Trim().ToLower();
            }
        }

        /*//VerificarSessaoGlobal, usar antes de menus: SessaoAtual.SanitizarDados();
        // Você pode colocar isso dentro de:
        MenuPrincipal.ExibirPrincipal()
        MenuReservas.ExibirMenuReservas()
        MenuClientes.ExibirMenuClientes()
                */
        public static void VerificarSessaoGlobal()
        {
            Console.WriteLine(); // espaço visual

            if (!Espacos.Any())
                CentralSaida.Exibir(TipoMensagem.Alerta, "Nenhum espaço está cadastrado no sistema.");

            if (!Clientes.Any())
                CentralSaida.Exibir(TipoMensagem.Alerta, "Nenhum cliente disponível para vincular ou reservar.");

            if (!Reservas.Any())
                CentralSaida.Exibir(TipoMensagem.Informacao, "ℹAinda não existem reservas cadastradas.");
        }






    }




}

