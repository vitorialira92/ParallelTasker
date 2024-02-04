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
        public static int QuantidadeDeTarefasExecutadas { get; set; }
        public static int QuantidadeMaximaDeSubtarefas { get; set; }
        public static TipoArmazenamento Armazenamento { get; set; }

        public static void SetValues(int quantidadeDeTarefasExecutadas,
            int quantidadeMaximaDeSubtarefas, TipoArmazenamento armazenamento)
        {
            QuantidadeDeTarefasExecutadas = quantidadeDeTarefasExecutadas;
            QuantidadeMaximaDeSubtarefas = quantidadeMaximaDeSubtarefas;
            Armazenamento = armazenamento;
        }
    }
}
