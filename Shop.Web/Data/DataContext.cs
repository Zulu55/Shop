namespace Shop.Web.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Shop.Web.Data.Entities;

    public class DataContext : IdentityDbContext<User>

    {
        public DbSet<Product> Products { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
    }
}
