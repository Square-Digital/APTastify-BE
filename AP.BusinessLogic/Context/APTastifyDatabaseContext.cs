using AP.BusinessInterfaces.Data.User;
using Microsoft.EntityFrameworkCore;
namespace AP.BusinessLogic.Context;

public class APTastifyDatabaseContext(DbContextOptions<APTastifyDatabaseContext> opt) : DbContext(opt)
{
    public DbSet<UserSignup> UserSignup { get; set; }
    
}