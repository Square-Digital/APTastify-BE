namespace AP.BusinessLogic.Services;

public interface IService<T>
{
    T Get(Guid id);
    T Update(Guid id, T model);
    bool Delete(Guid id);
    T Create(T entity);
}