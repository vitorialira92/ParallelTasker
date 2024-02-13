using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Text;

namespace ProcessadorTarefas.Entidades
{
    public interface ITarefa
    {
        long Id { get; }
        EstadoTarefa Estado { get; }
        DateTime IniciadaEm { get; }
        DateTime EncerradaEm { get; }
        IEnumerable<Subtarefa> SubtarefasPendentes { get; set; }
        IEnumerable<Subtarefa> SubtarefasExecutadas { get; set; }
    }

    public class Tarefa : ITarefa
    {
        public long Id { get; set; }
        public EstadoTarefa Estado { get; set; }
        public DateTime IniciadaEm { get; set; }
        public DateTime EncerradaEm { get; set; }

        private List<Subtarefa> subtarefasPendentesInternas = new List<Subtarefa>();

        private List<Subtarefa> subtarefasExecutadasInternas = new List<Subtarefa>();


        public IEnumerable<Subtarefa> SubtarefasPendentes
        {
            get => subtarefasPendentesInternas.AsReadOnly(); 
            set => subtarefasPendentesInternas = new List<Subtarefa>(value);
        }
    
        public IEnumerable<Subtarefa> SubtarefasExecutadas
        {
            get => subtarefasExecutadasInternas.AsReadOnly();
            set => subtarefasExecutadasInternas = new List<Subtarefa>(value);
        }
        

        private bool StopExecution { get; set; }

        internal Tarefa()
        {

        }
        public Tarefa(long Id)
        {
            this.Id = Id;

            this.Estado = EstadoTarefa.Criada;

            CriarSubtarefas();

            this.IniciadaEm = default;

            this.EncerradaEm = default;
        }

        public Tarefa(long Id, EstadoTarefa estado, List<Subtarefa> subtarefasPendentes, List<Subtarefa> subtarefasExecutadas,
            DateTime IniciadaEm = default, DateTime EncerradaEm = default)
        {
            this.Id = Id;
            this.subtarefasPendentesInternas = subtarefasPendentes;
            this.subtarefasExecutadasInternas = new List<Subtarefa>();

            this.IniciadaEm = IniciadaEm;
            this.EncerradaEm = EncerradaEm;
        }

        public bool EstaAtiva()
        {
            return this.Estado.EhEstadoAtivo();
        }

        public bool AtualizarEstado(EstadoTarefa novoEstado)
        {
            if(!this.Estado.EhPossivelFazerTransicao(novoEstado))
                return false;

            this.Estado = novoEstado; return true;
        }

        public async Task Executar()
        {
            if (this.Estado.EPossivelExecutar())
            {
                StopExecution = false;
                this.Estado = EstadoTarefa.EmExecucao;
                
                if(IniciadaEm == default)
                    IniciadaEm = DateTime.Now;

                List<Subtarefa> subtarefasEmExecucao = new List<Subtarefa>(subtarefasPendentesInternas);

                foreach (var subtarefa in subtarefasEmExecucao)
                {
                    if (StopExecution)
                        return;

                    await Task.Delay(subtarefa.Duracao);

                    Console.WriteLine($"TAREFA ID {Id} - SUBTAREFA DE {subtarefa.Duracao} FINALIZADA");

                    subtarefasExecutadasInternas.Add(subtarefa);

                    subtarefasPendentesInternas.Remove(subtarefa);
                }

                Concluir();
            }

        }

        private void Concluir()
        {
            this.EncerradaEm = DateTime.Now;
            this.Estado = EstadoTarefa.Concluida;
        }

        public bool Pausar()
        {

            if(this.Estado != EstadoTarefa.EmExecucao) return false;

            StopExecution = true;

            this.Estado = EstadoTarefa.EmPausa;
            
            return true;
        }

        public bool Encerrar(DateTime dateTime)
        {
            if (!this.Estado.EPossivelConcluir() || dateTime < this.IniciadaEm)
                return false;

            this.EncerradaEm = dateTime;

            return true;
        }

        public bool Cancelar()
        {
            if (!this.Estado.EPossivelCancelar())
                return false;

            StopExecution = true;
            this.Estado = EstadoTarefa.Cancelada;

            return true;
        }

        public ProgressoExecucaoDeTarefa VerificarProgresso()
        {
            int executadas = 0;

            foreach (var item in this.SubtarefasExecutadas)
                executadas += (int) item.Duracao.TotalSeconds;
            
            int total = executadas;
            foreach (var item in this.SubtarefasPendentes)
                total += (int) item.Duracao.TotalSeconds;

            return new ProgressoExecucaoDeTarefa(
                    Id,
                    executadas,
                    total
                );
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($" >> Tarefa de ID {Id}\n");
            sb.Append($" >>>> Estado: {this.Estado.GetName()}");
            int count = 1;
            foreach (var item in subtarefasExecutadasInternas)
            {
                sb.Append($"\n    > Subtarefa {count} | {(int) item.Duracao.TotalSeconds} segundos | Status: EXECUTADA");
                count++;
            }
            
            foreach (var item in subtarefasPendentesInternas)
            {
                sb.Append($"\n    > Subtarefa {count} | {(int) item.Duracao.TotalSeconds} segundos | Status: PENDENTE");
                count++;
            }
                
            sb.Append($"\n >>>> Total de {subtarefasExecutadasInternas.Count + subtarefasPendentesInternas.Count} Subtarefas \n" +
                $" >>>> Tempo total para executar esta tarefa: {VerificarProgresso().TempoDeTotalDeSubtarefas} segundos\n\n");

            return sb.ToString();

        }

        private void CriarSubtarefas()
        {
            Random random = new Random();

            List<Subtarefa> subtarefas = new List<Subtarefa>();


            for (int i = 0; i < random.Next(10, ConfigurationValues.QuantidadeMaximaDeSubtarefas + 1); i++)
            {
                Subtarefa subtarefa = new Subtarefa();
                subtarefa.Duracao = TimeSpan.FromSeconds(random.Next(3, 61));
                subtarefas.Add(subtarefa);
            }

            this.SubtarefasPendentes = subtarefas;
        }

    }

}
