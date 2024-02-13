using ProcessadorTarefas.Entidades;
using SOLID_Example.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessadorTarefas.Servicos
{
    public class ProcessadorTarefas : IProcessadorTarefas
    {
        private readonly IRepository<Tarefa> repository;

        private List<Tarefa> tarefasEmExecucao = new List<Tarefa>();
        private Queue<Tarefa> tarefasNaFila = new Queue<Tarefa>();

        Dictionary<Tarefa, Stopwatch> tarefaStopwatchMap = new Dictionary<Tarefa, Stopwatch>();

        public ProcessadorTarefas(IRepository<Tarefa> repositoryInstance)
        {
            repository = repositoryInstance;
        }

        public async Task Iniciar()
        {
            FillQueue();

            var act = new List<Task>();

            int counter = 0;

            var taskToTarefaMap = new Dictionary<Task, Tarefa>();

            foreach (var tarefa in tarefasEmExecucao.Where(x => x.Estado ==  EstadoTarefa.EmPausa))
            {
                Console.WriteLine($"tAREFA ID {tarefa.Id} em pausa: {tarefa.Estado.ToString()}");    
                var task = Executar(tarefa);
                act.Add(task);
                taskToTarefaMap[task] = tarefa;

                Stopwatch stopwatch = Stopwatch.StartNew();
                tarefaStopwatchMap[tarefa] = stopwatch;
            }
                
            foreach (var tarefa in tarefasEmExecucao.Where(x => x.Estado != EstadoTarefa.EmExecucao))
            {
                Console.WriteLine($"tAREFA ID {tarefa.Id} NAO em pausa: {tarefa.Estado.ToString()}");

                var task = Executar(tarefa);
                act.Add(task);
                taskToTarefaMap[task] = tarefa;

                Stopwatch stopwatch = Stopwatch.StartNew();
                tarefaStopwatchMap[tarefa] = stopwatch;
            }
                

            while (act.Count > 0)
            {

                var completedTask = await Task.WhenAny(act);

                act.Remove(completedTask);

                var tarefaConcluida = taskToTarefaMap[completedTask];

                Console.WriteLine($"Tarefa {tarefaConcluida.Id} encerrada");

                tarefaStopwatchMap.Remove(tarefaConcluida);

                tarefasEmExecucao.Remove(tarefaConcluida);

                taskToTarefaMap.Remove(completedTask);

                if (tarefasNaFila.TryDequeue(out var nextTarefa))
                {
                    var task = Executar(nextTarefa);

                    taskToTarefaMap[task] = nextTarefa;

                    act.Add(task);

                    tarefasEmExecucao.Add(nextTarefa);

                    tarefaStopwatchMap[nextTarefa] = Stopwatch.StartNew();
                }

            }
            tarefasEmExecucao.Clear();

            tarefasNaFila.Clear();
        }

        public List<ProgressoExecucaoDeTarefa> GetProgressoTarefas()
        {
            List<ProgressoExecucaoDeTarefa> lista = new List<ProgressoExecucaoDeTarefa>();

            foreach (var tarefa in tarefasEmExecucao)
            {
                var progresso = tarefa.VerificarProgresso();

                if (tarefaStopwatchMap.TryGetValue(tarefa, out var stopwatch) && stopwatch.IsRunning)
                {
                    int tempoDecorrido = (int)stopwatch.Elapsed.TotalSeconds - progresso.TempoDeSubtarefasExecutadas;
                    progresso.TempoDeSubtarefasExecutadas += tempoDecorrido;
                }
                
                lista.Add(progresso);
            }
               

            return lista;
        }
        public Task Encerrar()
        {
            throw new NotImplementedException();
        }

        private void FillQueue()
        {
            int counter = 0;

            foreach (var item in repository.GetTheFirstNExecutable(3 * ConfigurationValues.QuantidadeDeTarefasExecutadas))
            {
                if (item.Estado != EstadoTarefa.EmPausa)
                    item.Estado = EstadoTarefa.Agendada;

                if (counter < ConfigurationValues.QuantidadeDeTarefasExecutadas)
                    tarefasEmExecucao.Add(item);
                    
                else
                    tarefasNaFila.Enqueue(item);

                counter++;
            }

        }
        private async Task Executar(Tarefa tarefa)
        {
            await tarefa.Executar();
        }

        public Task Pausar(long id)
        {
            var tarefa = repository.GetById(id);
            tarefa.Pausar();
            repository.Update(tarefa);
            return Task.CompletedTask;
        }
    }
}
