using System;
using System.IO;
using System.Collections.Generic;
using DesafioProjetoHospedagem.Models;
using DesafioProjetoHospedagem.Utils;
using DesafioProjetoHospedagem.Menus;
using DesafioProjetoHospedagem.Controladoras;
using DesafioProjetoHospedagem.Sessao;

namespace DesafioProjetoHospedagem.Servicos
{
    /// <summary>
    /// Serviço responsável por ações relacionadas à persistência de dados dos clientes.
    /// </summary>
    public static class ServicoClientes
    {
        private const string CaminhoArquivo = "clientes_salvos.txt";

        /// <summary>
        /// Salva os dados dos clientes em um arquivo de texto.
        /// </summary>
        /// 
        public static void SalvarClientes()
        /*public static void SalvarClientes(
            List<Cliente> clientes,
            List<EspacoHospedagem> espacos,
            List<Reserva> reservas,
            Reserva? reservaSelecionada)*/
        {
            Console.Clear();
            CentralSaida.CabecalhoMenus("Salvar Clientes");

                if (SessaoAtual.Clientes == null || SessaoAtual.Clientes.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Nenhum cliente para salvar.");
                Console.ResetColor();
                return;
            }

            using StreamWriter writer = new StreamWriter(CaminhoArquivo, append: false)
            {
                AutoFlush = true
            };

            foreach (var cliente in SessaoAtual.Clientes)
            {
                writer.WriteLine($"Nome completo: {cliente.NomeCompleto}");
                writer.WriteLine($"E-mail: {cliente.Email}");
                writer.WriteLine($"Nascimento: {cliente.DataNascimento:dd/MM/yyyy}");
                writer.WriteLine(new string('-', 30));
            }

            CentralSaida.Exibir(
            TipoMensagem.Sucesso,
            $"{SessaoAtual.Clientes.Count} cliente(s) salvo(s) com sucesso em:\n{Path.GetFullPath(CaminhoArquivo)}"
            );

            MenuRodape.ExibirRodape(
            "Salvar Clientes",
            aoVoltarParaSubmenu: () =>
            {
                // Aqui você pode reabrir o menu anterior, como ExibirSubMenuClientes(...) ou outro fluxo de edição
            },
            aoVoltarMenuPrincipal: () => MenuPrincipal.ExibirPrincipal()
            );


        }
    }
}
