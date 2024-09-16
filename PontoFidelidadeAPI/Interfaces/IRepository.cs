namespace PontoFidelidadeAPI.Interfaces
{
    public interface IRepository<T, t>
    {
        IEnumerable<T> GetAll();
        T Get(t id);
        T Save(T entity);
        void Update(T entity);
        void Delete(t id);
    }
}
