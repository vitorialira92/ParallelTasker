using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessadorTarefas.Entidades
{
    public class ProgressoExecucaoDeTarefa
    {
        public int SubtarefasExecutadas { get; }
        public int TotalDeSubtarefas { get; }

        public ProgressoExecucaoDeTarefa(int subtarefasExecutadas, int totalDeSubtarefas)
        {
            SubtarefasExecutadas = subtarefasExecutadas;
            TotalDeSubtarefas = totalDeSubtarefas;
        }
    }
}
