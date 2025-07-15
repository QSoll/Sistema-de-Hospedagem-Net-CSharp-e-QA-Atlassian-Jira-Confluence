using System;
using DesafioProjetoHospedagem.Sessao;
using DesafioProjetoHospedagem.Models;
using DesafioProjetoHospedagem.Utils;
using DesafioProjetoHospedagem.Controladoras;
using DesafioProjetoHospedagem.Menus;

namespace DesafioProjetoHospedagem.Controladoras
{
    public static class CriarReservaControladora
    {
        public static void ContinuarCriacao(Reserva reservaParcial)
        {
            // Etapa 1: registrar origem da navegação
            NavegacaoAtual.Registrar(() => MenuReservas.ExibirMenuReservas());

            // Etapa 2: instrução visual
            CentralSaida.Exibir(TipoMensagem.Instrucao, "Vamos selecionar os clientes para sua reserva!");

            // Etapa 3: seleção dos clientes
            List<Cliente> clientesSelecionados = TelaClientes.SelecionarMultiplos();

            // Etapa 4: vincular clientes à reserva
            reservaParcial.VincularClientes(clientesSelecionados);

            // Etapa 5: finalizar reserva
            reservaParcial.FinalizarReserva();

            // Etapa 6: salvar na lista global (SessaoAtual ou persistência)
            SessaoAtual.Reservas.Add(reservaParcial);

            // Etapa 7: feedback de sucesso
            //CentralSaida.Exibir(TipoMensagem.Sucesso, "Reserva criada com sucesso e clientes vinculados!");
            string mensagemFinal = reservaParcial.Clientes.Any()
            ? "Reserva criada com sucesso e clientes vinculados!"
            : "Reserva criada, ainda sem clientes vinculados. Você pode adicioná-los separadamente.";

            CentralSaida.Exibir(TipoMensagem.Sucesso, mensagemFinal);



            // Etapa 8: rodapé emocional com base na origem
            MenuRodapeGlobal.ExibirRodapeGlobal("CriarReserva");

        }
        public static void IniciarComEspacoVinculado(EspacoHospedagem espacoSelecionado)
        {
            CentralSaida.Exibir(TipoMensagem.Informativo, $"Iniciando reserva com espaço \"{espacoSelecionado.NomeFantasia}\" vinculado...");

            // Coleta os dados restantes
            int dias = CentralEntradaControladora.LerInteiroMinMax("Quantidade de dias reservados", 1, 30);
            DateTime dataCheckIn = CentralEntrada.LerDataObrigatoria("Data do check-in");

            // Monta reserva parcial
            var reservaParcial = new Reserva(SessaoAtual.ClientesSelecionados, espacoSelecionado, dias)

            {
                DataCheckIn = dataCheckIn,
                DataCheckOut = dataCheckIn.AddDays(dias)
            };

            SessaoAtual.ReservaSelecionada = reservaParcial;

            // Continua o fluxo de criação
            ContinuarCriacao(reservaParcial);
        }



    }
}
