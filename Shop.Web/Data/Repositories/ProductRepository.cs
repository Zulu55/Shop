namespace Shop.Web.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly DataContext context;

        public ProductRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        public IQueryable GetAllWithUsers()
        {
            return this.context.Products.Include(p => p.User); 
        }

        public IEnumerable<SelectListItem> GetComboProducts()
        {
            var list = this.context.Products.Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Id.ToString()
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a product...)",
                Value = "0"
            });

            return list;
        }
    }
}
