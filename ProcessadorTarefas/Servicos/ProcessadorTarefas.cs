using ProcessadorTarefas.Entidades;
using SOLID_Example.Interfaces;
using System;
using System.Collections.Generic;
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
        public ProcessadorTarefas(IRepository<Tarefa> repositoryInstance)
        {
            repository = repositoryInstance;
        }

        public async Task Iniciar()
        {
            FillQueue();

            var act = new List<Task>();
            var taskToTarefaMap = new Dictionary<Task, Tarefa>();


            foreach (var tarefa in tarefasEmExecucao)
            {
                var task = Executar(tarefa);
                act.Add(task);
                taskToTarefaMap[task] = tarefa;
            }
                

            int counter = 0;
            while (act.Count > 0)
            {
                Console.WriteLine($"{counter} tarefas finalizadas");

                var completedTask = await Task.WhenAny(act);

                act.Remove(completedTask);

                var tarefaConcluida = taskToTarefaMap[completedTask];
                tarefasEmExecucao.Remove(tarefaConcluida);
                taskToTarefaMap.Remove(completedTask);

                if (tarefasNaFila.TryDequeue(out var nextTarefa))
                {
                    var task = Executar(nextTarefa);
                    taskToTarefaMap[task] = nextTarefa;
                    act.Add(task);
                    tarefasEmExecucao.Add(nextTarefa);
                }

                counter++;
            }
            tarefasEmExecucao.Clear();
            tarefasNaFila.Clear();
        }

        public List<ProgressoExecucaoDeTarefa> GetProgressoTarefas()
        {
            List<ProgressoExecucaoDeTarefa> lista = new List<ProgressoExecucaoDeTarefa>();
            foreach (var item in tarefasEmExecucao)
               lista.Add(item.VerificarProgresso());

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
    }
}
