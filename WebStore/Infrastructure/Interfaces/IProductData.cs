using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entities;

namespace WebStore.Infrastructure.Interfaces
{
    interface IProductData
    {
        IEnumerable<Section> GetSections();

        IEnumerable<Brand> GetBrands();
    }
}
