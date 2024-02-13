using ProcessadorTarefas.Entidades;

namespace ProcessadorTarefas.Servicos
{
    public interface IProcessadorTarefas
    {
        Task Iniciar();
        Task Encerrar();
        List<ProgressoExecucaoDeTarefa> GetProgressoTarefas();
        Task Pausar(long id);
    }
}
