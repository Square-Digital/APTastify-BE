namespace AP.BusinessLogic.Services;

public interface IService<T>
{
    Task<T> Get(Guid id);
    Task<T> Update(Guid id, T model);
    Task<bool> Delete(Guid id);
    Task<T> Create(T entity);
}