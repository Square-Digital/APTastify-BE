using AP.BusinessInterfaces.Data.User;
using AP.BusinessLogic.Services;

namespace AP.WebAPI.Managers;

public class UserManager(UserService service) : IManager<User>
{
    public Task<User> Get(Guid id)
    {
        return service.Get(id);
    }

    public Task<User> Create(User value)
    {
        return service.Create(value);
    }

    public Task<User> Update(Guid id, User value)
    {
        return service.Update(id, value);
    }

    public Task<bool> Delete(Guid id)
    {
        return service.Delete(id);
    }
}