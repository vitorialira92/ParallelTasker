using ProcessadorTarefas.Entidades;

namespace ProcessadorTarefas.Servicos
{
    public interface IGerenciadorTarefas
    {
        Task<Tarefa> Criar();
        Task<Tarefa> Consultar(long idTarefa);
        Task Cancelar(long idTarefa);
        Task<IEnumerable<Tarefa>> ListarAtivas();
        Task<IEnumerable<Tarefa>> ListarInativas();
    }
}
