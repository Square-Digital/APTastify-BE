using AP.BusinessInterfaces.Data.User;
using AP.BusinessLogic.Context;
using AP.BusinessLogic.Services.Email;

namespace AP.BusinessLogic.Services;

public class UserService(APTastifyDatabaseContext context, IEmailService emailService) : IService<User>
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

    public async Task<UserSignup> Signup(SignupRequest model)
    {
        UserSignup userSignup;
        
        // Handle database operations in transaction
        await using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            userSignup = CreateSignup(model);
            context.UserSignup.Add(userSignup);
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw;
        }

        // Handle email sending outside of transaction
        try
        {
            await emailService.SendTemplatedEmailAsync(
                model.Email,
                "SignupEmail",
                model
            );
        }
        catch (Exception ex)
        {
            // Log email sending error but don't fail the signup
            // You might want to add proper logging here
            // For now, we'll just continue
        }

        return userSignup;
    }

    public UserSignup CreateSignup(SignupRequest model)
    {
        return new UserSignup
        {
            Email = model.Email,
        };
    }
}