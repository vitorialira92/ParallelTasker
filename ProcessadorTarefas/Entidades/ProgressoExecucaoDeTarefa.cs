using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessadorTarefas.Entidades
{
    public class ProgressoExecucaoDeTarefa
    {
        public long Id { get; }
        public int TempoDeSubtarefasExecutadas { get; set; }
        public int TempoDeTotalDeSubtarefas { get; set; }

        public ProgressoExecucaoDeTarefa(long id, int tempoDeSubtarefasExecutadas, int tempoTotalDeSubtarefas)
        {
            Id = id;
            TempoDeSubtarefasExecutadas = tempoDeSubtarefasExecutadas;
            TempoDeTotalDeSubtarefas = tempoTotalDeSubtarefas;
        }
    }
}
