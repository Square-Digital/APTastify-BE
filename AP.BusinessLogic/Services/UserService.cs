using AP.BusinessInterfaces.Data.User;

namespace AP.BusinessLogic.Services;

public class UserService : IService<User>
{
    public Task<User> Get(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<User> Update(Guid id, User model)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<User> Create(User entity)
    {
        throw new NotImplementedException();
    }
}