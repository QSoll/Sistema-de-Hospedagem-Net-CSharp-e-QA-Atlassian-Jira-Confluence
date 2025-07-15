using System;
using System.Collections.Generic;
using DesafioProjetoHospedagem.Models;
using DesafioProjetoHospedagem.Utils;
using DesafioProjetoHospedagem.Controladoras;
using DesafioProjetoHospedagem.Sessao;
using DesafioProjetoHospedagem.Menus;

namespace DesafioProjetoHospedagem.Menus
{
    public static class TelaClientes
    {
        public static List<Cliente> SelecionarMultiplos()
        {

            // Registrar para navegação reversa
            NavegacaoAtual.Registrar(() => MenuPrincipal.ExibirPrincipal());
            
            var clientesSelecionados = new List<Cliente>();

            if (SessaoAtual.Clientes.Count == 0)
            {
                CentralSaida.Exibir(TipoMensagem.Alerta, "Nenhum cliente disponível para seleção.");
                return clientesSelecionados;
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Clientes disponíveis:\n");
            Console.ResetColor();

            for (int i = 0; i < SessaoAtual.Clientes.Count; i++)
            {
                var cliente = SessaoAtual.Clientes[i];
                Console.WriteLine($"[{i + 1}] {cliente.Nome} – CPF: {cliente.Cpf}");
            }

            Console.WriteLine("\nDigite os números dos clientes que deseja selecionar (separados por vírgula):");
            string? entrada = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(entrada))
            {
                CentralSaida.Exibir(TipoMensagem.Alerta, "Nenhum cliente foi selecionado.");
                return clientesSelecionados;
            }

            var indices = entrada.Split(',')
                .Select(e => e.Trim())
                .Where(e => int.TryParse(e, out _))
                .Select(int.Parse)
                .Distinct()
                .ToList();

            Console.WriteLine();

            foreach (int indice in indices)
            {
                int posicao = indice - 1;
                if (posicao >= 0 && posicao < SessaoAtual.Clientes.Count)
                {
                    var clienteSelecionado = SessaoAtual.Clientes[posicao];

                    if (!clientesSelecionados.Contains(clienteSelecionado))
                    {
                        clientesSelecionados.Add(clienteSelecionado);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"{clienteSelecionado.Nome} selecionado.");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"{clienteSelecionado.Nome} já foi selecionado.");
                    }
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Número {indice} é inválido.");
                    Console.ResetColor();
                }
            }

            return clientesSelecionados;
        }
    }
}
