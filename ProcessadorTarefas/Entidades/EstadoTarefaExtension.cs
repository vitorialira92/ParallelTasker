using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessadorTarefas.Entidades
{
    public static class EstadoTarefaExtension
    {
        private static List<EstadoTarefa> estadosNaoAtivos = new List<EstadoTarefa>()
            {
                EstadoTarefa.Concluida,
                EstadoTarefa.Cancelada
            };

        private static List<EstadoTarefa> possiveisParaCancelar = new List<EstadoTarefa>()
            {
                EstadoTarefa.Criada,
                EstadoTarefa.Agendada,
                EstadoTarefa.EmExecucao
            };
        
        private static List<EstadoTarefa> possiveisParaExecutar = new List<EstadoTarefa>()
            {
                EstadoTarefa.Agendada,
                EstadoTarefa.EmPausa
            };

        private static Dictionary<EstadoTarefa, List<EstadoTarefa>> PossibilidadesTransicaoEstadosAtivos = new Dictionary<EstadoTarefa, List<EstadoTarefa>>()
        {
            { EstadoTarefa.Criada, new List<EstadoTarefa>() { EstadoTarefa.Agendada, EstadoTarefa.Cancelada } },
            { EstadoTarefa.Agendada, new List<EstadoTarefa>() { EstadoTarefa.EmExecucao, EstadoTarefa.Cancelada } },
            { EstadoTarefa.EmExecucao, new List<EstadoTarefa>() { EstadoTarefa.EmPausa, EstadoTarefa.Concluida, EstadoTarefa.Cancelada } },
            { EstadoTarefa.EmPausa, new List<EstadoTarefa>() { EstadoTarefa.EmExecucao } }

        };

        public static bool EhEstadoAtivo(this EstadoTarefa estado)
        {
            return !estadosNaoAtivos.Contains(estado);
        }
        public static bool EPossivelCancelar(this EstadoTarefa estado)
        {
            return possiveisParaCancelar.Contains(estado);
        }

        public static bool EPossivelExecutar(this EstadoTarefa estado)
        {
            return possiveisParaExecutar.Contains(estado);
        }
        
        public static bool EPossivelConcluir(this EstadoTarefa estado)
        {
            return estado == EstadoTarefa.EmExecucao;
        }


        public static bool EhPossivelFazerTransicao(this EstadoTarefa estado,
                EstadoTarefa novoEstado)
        {
            if(!EhEstadoAtivo(estado)) return false;

            if (!PossibilidadesTransicaoEstadosAtivos[estado].Contains(novoEstado))
                return false;

            return true;
        }
    }
}
