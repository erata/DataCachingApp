using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;
using DataCachingApp.DAL;
using DataCachingApp.Models;
using DataCachingApp.Repository.Interfaces; 

namespace DataCachingApp.Repository.Implementation
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductContext _context = new ProductContext();

        public IEnumerable<Product> GetAll()
        {
            return _context.Products.Select(x => x); 
        }

        public Product GetById(int id)
        {
            return _context.Products.FirstOrDefault(x => x.Id == id);
        }

        public Product Get(System.Linq.Expressions.Expression<Func<Product, bool>> expression)
        {
            return _context.Products.FirstOrDefault(expression);
        }

        public IQueryable<Product> GetMany(System.Linq.Expressions.Expression<Func<Product, bool>> expression)
        {
            return _context.Products.Where(expression);
        }

        public void Insert(Product obj)
        {
            _context.Products.Add(obj);
        }

        public void Update(Product obj)
        {
            _context.Products.AddOrUpdate();
        }

        public void Delete(int id)
        {
            var Product = GetById(id);
            if (Product != null)
            {
                _context.Products.Remove(Product);
            }
        }

        public int Count()
        {
            return _context.Products.Count();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

    }
}
