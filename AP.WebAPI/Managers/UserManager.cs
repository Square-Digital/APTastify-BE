using AP.BusinessInterfaces.Data.User;

namespace AP.WebAPI.Managers;

public class UserManager:IManager<User>
{
    public Task<User> Get(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<User> Create(User value)
    {
        throw new NotImplementedException();
    }

    public Task<User> Update(Guid id, User value)
    {
        throw new NotImplementedException();
    }

    public Task<User> Delete(Guid id)
    {
        throw new NotImplementedException();
    }
}