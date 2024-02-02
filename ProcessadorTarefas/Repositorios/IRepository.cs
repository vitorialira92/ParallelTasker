namespace SOLID_Example.Interfaces
{
    internal interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T? GetById(int id);
        void Add(T entity);
        void Update(T entity);
    }
}
