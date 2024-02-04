using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ProcessadorTarefas.Entidades
{
    public static class ConfigurationValues
    {
        public static int QuantidadeDeTarefasExecutadas { get; private set; }
        public static int QuantidadeMaximaDeSubtarefas { get; private set; }
        public static TipoArmazenamento Armazenamento { get; private set; }

        public static void SetValues(int quantidadeDeTarefasExecutadas,
            int quantidadeMaximaDeSubtarefas, TipoArmazenamento armazenamento)
        {
            QuantidadeDeTarefasExecutadas = quantidadeDeTarefasExecutadas;
            QuantidadeMaximaDeSubtarefas = quantidadeMaximaDeSubtarefas;
            Armazenamento = armazenamento;
        }
    }
}
