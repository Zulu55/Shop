namespace Shop.Web.Data.Repositories
{
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    using Microsoft.EntityFrameworkCore;
    using Shop.Web.Helpers;
    using Shop.Web.Models;

    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly DataContext context;
        private readonly IUserHelper userHelper;

        public OrderRepository(DataContext context, IUserHelper userHelper) : base(context)
        {
            this.context = context;
            this.userHelper = userHelper;
        }

        public async Task<IQueryable<Order>> GetOrdersAsync(string userName)
        {
            var user = await this.userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return null;
            }

            if (await this.userHelper.IsUserInRoleAsync(user, "Admin"))
            {
                return this.context.Orders
                    .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                    .OrderByDescending(o => o.OrderDate);
            }

            return this.context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Where(o => o.User == user)
                .OrderByDescending(o => o.OrderDate);
        }

        public async Task<IQueryable<OrderDetailTemp>> GetDetailTempsAsync(string userName)
        {
            var user = await this.userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return null;
            }

            return this.context.OrderDetailTemps
                .Include(o => o.Product)
                .Where(o => o.User == user)
                .OrderBy(o => o.Product.Name);
        }

        public async Task AddItemToOrderAsync(AddItemViewModel model, string userName)
        {
            var user = await this.userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return;
            }

            var product = await this.context.Products.FindAsync(model.ProductId);
            if (product == null)
            {
                return;
            }

            var orderDetailTemp = await this.context.OrderDetailTemps
                .Where(odt => odt.User == user && odt.Product == product)
                .FirstOrDefaultAsync();
            if (orderDetailTemp == null)
            {
                orderDetailTemp = new OrderDetailTemp
                {
                    Price = product.Price,
                    Product = product,
                    Quantity = model.Quantity,
                    User = user,
                };

                this.context.OrderDetailTemps.Add(orderDetailTemp);
            }
            else
            {
                orderDetailTemp.Quantity += model.Quantity;
                this.context.OrderDetailTemps.Update(orderDetailTemp);
            }

            await this.context.SaveChangesAsync();
        }

        public async Task ModifyOrderDetailTempQuantityAsync(int id, double quantity)
        {
            var orderDetailTemp = await this.context.OrderDetailTemps.FindAsync(id);
            if (orderDetailTemp == null)
            {
                return;
            }

            orderDetailTemp.Quantity += quantity;
            if (orderDetailTemp.Quantity > 0)
            {
                this.context.OrderDetailTemps.Update(orderDetailTemp);
                await this.context.SaveChangesAsync();
            }
        }

        public async Task DeleteDetailTempAsync(int id)
        {
            var orderDetailTemp = await this.context.OrderDetailTemps.FindAsync(id);
            if (orderDetailTemp == null)
            {
                return;
            }

            this.context.OrderDetailTemps.Remove(orderDetailTemp);
            await this.context.SaveChangesAsync();
        }
    }
}
