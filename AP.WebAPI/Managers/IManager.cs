using Microsoft.AspNetCore.Mvc;

namespace AP.WebAPI.Managers;

public interface IManager<T>
{
    Task<T> Get(Guid id);
    Task<T> Create(T value);
    Task<T> Update(Guid id, T value);
    Task<bool> Delete(Guid id);
}