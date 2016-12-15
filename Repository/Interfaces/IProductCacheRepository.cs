using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataCachingApp.Models;

namespace DataCachingApp.Repository.Interfaces
{
    public interface IProductCacheRepository : ICacheRepository<Product>
    {
    }
}
