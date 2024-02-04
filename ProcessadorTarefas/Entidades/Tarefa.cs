﻿using System.Diagnostics.Metrics;

namespace ProcessadorTarefas.Entidades
{
    public interface ITarefa
    {
        long Id { get; }
        EstadoTarefa Estado { get; }
        DateTime IniciadaEm { get; }
        DateTime EncerradaEm { get; }
        IEnumerable<Subtarefa> SubtarefasPendentes { get; }
        IEnumerable<Subtarefa> SubtarefasExecutadas { get; }
    }

    public class Tarefa : ITarefa
    {
        public long Id { get; set; }
        public EstadoTarefa Estado { get; set; }
        public DateTime IniciadaEm { get; set; }
        public DateTime EncerradaEm { get; set; }
        public IEnumerable<Subtarefa> SubtarefasPendentes { get; set; }
        public IEnumerable<Subtarefa> SubtarefasExecutadas { get; set; }

        private bool StopExecution { get; set; }

        internal Tarefa()
        {

        }
        //new task
        public Tarefa(long Id)
        {
            this.Id = Id;

            this.Estado = EstadoTarefa.Criada;

            CriarSubtarefas();

            this.SubtarefasExecutadas = new List<Subtarefa>();

            this.IniciadaEm = default;

            this.EncerradaEm = default;
        }

        public Tarefa(long Id, EstadoTarefa estado, List<Subtarefa> subtarefasPendentes, List<Subtarefa> subtarefasExecutadas,
            DateTime IniciadaEm = default, DateTime EncerradaEm = default)
        {
            this.Id = Id;
            this.SubtarefasPendentes = subtarefasPendentes;
            this.SubtarefasExecutadas = new List<Subtarefa>();

            this.IniciadaEm = IniciadaEm;
            this.EncerradaEm = EncerradaEm;
        }

        public bool EstaAtiva()
        {
            return this.Estado.EhEstadoAtivo();
        }

        public async Task Executar()
        {
            if (!this.Estado.EPossivelExecutar())
            {
                StopExecution = true;
                this.Estado = EstadoTarefa.EmExecucao;

                IniciadaEm = DateTime.Now;

                List<Subtarefa> tarefasExecutadas = this.SubtarefasPendentes.ToList();
                int i = 0;
                foreach (var subtarefa in tarefasExecutadas)
                {
                    if (!StopExecution)
                        return;

                    await Task.Delay(subtarefa.Duracao);

                    this.SubtarefasExecutadas.ToList().Add(subtarefa);

                    this.SubtarefasPendentes.ToList().Remove(subtarefa);
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

            StopExecution = false;

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

            this.Estado = EstadoTarefa.Cancelada;

            return true;
        }

        public ProgressoExecucaoDeTarefa VerificarProgresso()
        {
            int executadas = this.SubtarefasExecutadas.ToList().Count;
            int total = this.SubtarefasPendentes.ToList().Count + executadas;

            return new ProgressoExecucaoDeTarefa(
                    executadas,
                    total
                );
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
