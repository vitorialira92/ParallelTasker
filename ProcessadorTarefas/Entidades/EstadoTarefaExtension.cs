using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessadorTarefas.Entidades
{
    public static class EstadoTarefaExtension
    {
        public static bool EhEstadoAtivo(this EstadoTarefa estado)
        {
            List<EstadoTarefa> estadosNaoAtivos = new List<EstadoTarefa>()
            {
                EstadoTarefa.Concluida,
                EstadoTarefa.Cancelada
            };

            return !estadosNaoAtivos.Contains(estado);
        }
        public static bool EPossivelCancelar(this EstadoTarefa estado)
        {
            List<EstadoTarefa> possiveisParaCancelar = new List<EstadoTarefa>()
            {
                EstadoTarefa.Criada,
                EstadoTarefa.Agendada,
                EstadoTarefa.EmExecucao
            };

            return possiveisParaCancelar.Contains(estado);
        }

        public static bool EPossivelExecutar(this EstadoTarefa estado)
        {
            List<EstadoTarefa> possiveisParaExecutar = new List<EstadoTarefa>()
            {
                EstadoTarefa.Agendada,
                EstadoTarefa.EmPausa
            };
            return possiveisParaExecutar.Contains(estado);
        }
        
        public static bool EPossivelConcluir(this EstadoTarefa estado)
        {
            return estado == EstadoTarefa.EmExecucao;
        }
    }
}
