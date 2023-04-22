using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        //options have the connection string.
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AppUser> Users { get; set; } //Table name will be Users. i.e whichever variable name you give

        //For photos DBSet is not needed because we will not query it directly but via Users.
        // So table name will be picked up from classname or we can override in entity with decorator
    }
}