using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Web.Data.Entities
{
    public class Country : IEntity
    {
        public int Id { get ; set ; }

        public string Name { get; set; }
    }
}
