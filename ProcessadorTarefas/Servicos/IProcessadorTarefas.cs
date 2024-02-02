namespace ProcessadorTarefas.Servicos
{
    internal interface IProcessadorTarefas
    {
        Task Iniciar();
        Task CancelarTarefa(int idTarefa);
        Task Encerrar();
    }
}
