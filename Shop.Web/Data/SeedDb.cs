namespace Shop.Web.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    using Microsoft.AspNetCore.Identity;

    /// <summary>
    /// This class seed the initial data to DB
    /// </summary>
    public class SeedDb
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly DataContext context;

        /// <summary>
        /// 
        /// </summary>
        private readonly UserManager<User> userManager;

        /// <summary>
        /// 
        /// </summary>
        private Random random;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userManager"></param>
        public SeedDb(DataContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.random = new Random();
        }

        public async Task SeedAsync()
        {
            await this.context.Database.EnsureCreatedAsync();

            // Add user
            var user = await this.userManager.FindByEmailAsync("jzuluaga55@gmail.com");
            if (user == null)
            {
                user = new User
                {
                    FirstName = "Juan",
                    LastName = "Zuluaga",
                    Email = "jzuluaga55@gmail.com",
                    UserName = "jzuluaga55@gmail.com",
                    PhoneNumber = "3506342747"
                };

                var result = await this.userManager.CreateAsync(user, "123456");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }
            }

            // Add products
            if (!this.context.Products.Any())
            {
                this.AddProduct("iPhone X", user);
                this.AddProduct("Magic Mouse", user);
                this.AddProduct("iWatch Series 4", user);
                await this.context.SaveChangesAsync();
            }
        }

        private void AddProduct(string name, User user)
        {
            this.context.Products.Add(new Product
            {
                Name = name,
                Price = this.random.Next(1000),
                IsAvailabe = true,
                Stock = this.random.Next(100),
                User = user
            });
        }
    }
}
