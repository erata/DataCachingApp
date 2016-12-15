using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Script.Serialization;
using DataCachingApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DataCachingApp.DAL
{
    public class ContextInitializer : DropCreateDatabaseAlways<ProductContext>
    {
        protected override void Seed(ProductContext context)
        {
            InitializeData(context);
        }

        private void InitializeData(ProductContext context)
        {
            //Generate 500 object item to test
            for (int i = 1; i <= 500; i++)
            {
                context.Products.Add(new Product
                {
                    Id = i,
                    Name = string.Format("Product - {0}", i),
                    LastUpdateTime= DateTime.Now
                });
                
            }

            base.Seed(context);
        }                
    }
}
