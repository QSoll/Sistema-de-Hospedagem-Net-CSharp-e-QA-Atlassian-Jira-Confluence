using System;
using System.Linq;
using System.Collections.Generic;
using DesafioProjetoHospedagem.Models;
using DesafioProjetoHospedagem.Utils;
using DesafioProjetoHospedagem.Controladoras;
using DesafioProjetoHospedagem.Sessao;
using DesafioProjetoHospedagem.Menus;

namespace DesafioProjetoHospedagem.Menus
{
    public static class MenuRelatorios
    {        public static void Exibir()
        {
            Console.Clear();
            CentralSaida.CabecalhoMenus("Relatórios e Estatísticas");

            int totalEspacos = SessaoAtual.Espacos.Count;
            int espacosOcupados = SessaoAtual.Reservas.Count(r => r.Ativa);
            int espacosDisponiveis = totalEspacos - espacosOcupados;

            int totalReservas = SessaoAtual.Reservas.Count;
            int reservasAtivas = SessaoAtual.Reservas.Count(r => r.Ativa);
            int reservasEncerradas = SessaoAtual.Reservas.Count(r => !r.Ativa);

            int totalClientesUnicos = SessaoAtual.Clientes.DistinctBy(c => c.NomeCompleto).Count();

            decimal totalEstimado = SessaoAtual.Reservas
            .Where(r => r.Ativa)
            .Sum(r => r.CalcularResumoFinanceiro().ValorTotalEstimado);


            // ESPAÇOS
            CentralSaida.Subtitulo("Espaços");
            Console.WriteLine($"Cadastrados: {totalEspacos}");
            CentralSaida.Exibir(TipoMensagem.Sucesso, $"Disponíveis: {espacosDisponiveis}");
            CentralSaida.Exibir(TipoMensagem.Erro, $"Ocupados: {espacosOcupados}");

            // RESERVAS
            CentralSaida.Subtitulo("Reservas");
            Console.WriteLine($"Total: {totalReservas}");
            CentralSaida.Exibir(TipoMensagem.Sucesso, $"Ativas: {reservasAtivas}");
            CentralSaida.Exibir(TipoMensagem.Alerta, $"Encerradas: {reservasEncerradas}");

            // CLIENTES & FINANCEIRO
            CentralSaida.Subtitulo("Clientes & Financeiro");
            Console.WriteLine($"Clientes únicos: {totalClientesUnicos}");
            CentralSaida.ExibirDestaque("valor importante");


            MenuRodape.ExibirRodape(
            "Menu – Relatórios",
            aoVoltarParaSubmenu: () => MenuReservas.ExibirMenuReservas(),
            aoVoltarMenuPrincipal: () => MenuPrincipal.ExibirPrincipal()
            );

        }
    }
}
