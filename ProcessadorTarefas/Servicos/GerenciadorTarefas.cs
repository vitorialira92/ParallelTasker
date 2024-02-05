using ProcessadorTarefas.Entidades;
using ProcessadorTarefas.Repositorios.TarefaRepositories;
using SOLID_Example.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessadorTarefas.Servicos
{
    public class GerenciadorTarefas : IGerenciadorTarefas
    {
        private readonly IRepository<Tarefa> repository;
        public GerenciadorTarefas(IRepository<Tarefa> repository)
        {
            this.repository = repository;
        }
        public Task Cancelar(long idTarefa)
        {
            var tarefa = repository.GetById(idTarefa);
            if(tarefa.Cancelar())
                repository.Update(tarefa);

            return Task.CompletedTask;
        }

        public Task<Tarefa> Consultar(long idTarefa)
        {
            return Task.FromResult(repository.GetById(idTarefa));
        }

        public Task<Tarefa> Criar()
        {
            long id = GetNewId();
            Tarefa tarefa = new Tarefa(id);

            return Task.FromResult(tarefa);
        }

        private long GetNewId()
        {
            Random random = new Random();
            long id = 0;
            do
            {
                id = random.Next(34, 89238283);
            } while (repository.GetById(id) != null);

            return id;
        }

        public Task<IEnumerable<Tarefa>> ListarAtivas()
        {
            IEnumerable<Tarefa> listaTarefas = repository.GetAll()
                .ToList().Where(tarefa => tarefa.EstaAtiva());
            return Task.FromResult(listaTarefas);
        }

        public Task<IEnumerable<Tarefa>> ListarInativas()
        {
            IEnumerable<Tarefa> listaTarefas = repository.GetAll()
                .ToList().Where(tarefa => !tarefa.EstaAtiva());
            return Task.FromResult(listaTarefas);
        }
    }
}
