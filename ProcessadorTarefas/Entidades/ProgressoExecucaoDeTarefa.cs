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
        public int SubtarefasExecutadas { get; }
        public int TotalDeSubtarefas { get; }

        public ProgressoExecucaoDeTarefa(long id, int subtarefasExecutadas, int totalDeSubtarefas)
        {
            Id = id;
            SubtarefasExecutadas = subtarefasExecutadas;
            TotalDeSubtarefas = totalDeSubtarefas;
        }
    }
}
