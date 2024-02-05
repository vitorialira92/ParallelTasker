using ProcessadorTarefas.Entidades;

namespace SOLID_Example.Interfaces
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T? GetById(long id);
        void Add(T entity);
        void Update(T entity);
        IEnumerable<T> GetTheFirstNExecutable(int n);
    }
}
