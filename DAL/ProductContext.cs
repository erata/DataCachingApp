using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.IO;
using System.Reflection;
using System.Web;
using DataCachingApp.Models;
using Newtonsoft.Json.Linq;

namespace DataCachingApp.DAL
{
    public class ProductContext : DbContext
    {
        public ProductContext() : base("SampleDB") 
        {
          Database.SetInitializer(new ContextInitializer());
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
       
    }
}