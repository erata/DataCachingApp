using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Caching;
using DataCachingApp.Models;
using DataCachingApp.Repository.Interfaces;

namespace DataCachingApp.Repository.Implementation
{
    public class ProductCacheRepository : IProductCacheRepository
    {        
        public Product Get(int id)
        {
            ObjectCache cache = MemoryCache.Default;

            string cacheKey = "product_" + id;

            Product item = cache.Get(cacheKey) as Product;
            
            return item;
        }

        public List<Product> GetList()
        {
             ObjectCache cache = MemoryCache.Default;

             var productList = cache.ToDictionary(c => c.Key, c => c.Value as Product)
                                    .Where(c=>c.Key.Contains("product"))
                                    .Select(x => x.Value).ToList();
                        
             return productList;
        }


        //sample imp. var products = cacheService.GetOrSet(id, () => productRepository.GetAll());
        public Product GetOrSet(int id, Func<Product> getItemCallback)
        {
            ObjectCache cache = MemoryCache.Default;

            string cacheKey = "product_" + id;

            Product item = cache.Get(cacheKey) as Product;
                        
            if (item == null)
            {
                item = getItemCallback();                
                MemoryCache.Default.Add("product_" + item.Id, item, DateTime.Now.AddMinutes(10));
            }

            return item;
        }


        public void Add(Product item)
        {
            ObjectCache cache = MemoryCache.Default;
 
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(10);
            
            cache.Add("product" + "_" + item.Id, item, policy);
         }

        public void AddList(List<Product> list)
        {
            ObjectCache cache = MemoryCache.Default;

            CacheItemPolicy policy = new CacheItemPolicy();

            // gets data from db after 10 minutes later. You can give bigger (such as day, or year) value and process with cache value
            policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(10);

            foreach (var item in list)
            {
                cache.Add("product" + "_" + item.Id, item, policy);                
            }            
        }

        public void Update(Product item)
        {
            ObjectCache cache = MemoryCache.Default;

            cache.Remove("product_" + item.Id);

            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(10);
           
            cache.Add("product" + "_" + item.Id, item, policy);

        }
        
        public void Delete(int id)
        {
            ObjectCache cache = MemoryCache.Default;
            cache.Remove("product_" + id);
        }

        public int Count()
        {
            ObjectCache cache = MemoryCache.Default;
            var count = cache.Where(x => x.Key.Contains("product")).Count();            
           
            return count;
        }       
        
    }
}