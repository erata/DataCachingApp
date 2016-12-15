using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using CacheRepository;
using DataCachingApp.DAL;
using DataCachingApp.Models;
using DataCachingApp.Repository;
using DataCachingApp.Repository.Interfaces;
using PagedList;

namespace DataCachingApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly IProductCacheRepository cacheRepository;

        public HomeController(IProductRepository _productRepository, IProductCacheRepository _cacheRepository)
        {            
            productRepository = _productRepository;
            cacheRepository = _cacheRepository;
        }


        // GET: Home
        public ActionResult Index(int Sayfa = 1)
        {
          //  var products = cacheService.GetOrSet("product", () => productRepository.GetAll());
            if (cacheRepository.Count() > 0)
            {
                var products = cacheRepository.GetList();
                return View(products.OrderByDescending(x => x.Id).ToPagedList(Sayfa, 20));
            }
            else
            {
                var products = productRepository.GetAll().ToList(); 

                //will be cache as "product" + item.id
                cacheRepository.AddList(products); 

                return View(products.OrderByDescending(x => x.Id).ToPagedList(Sayfa, 20));
            }
                          
        }

        // GET: Home/Details/5
        public ActionResult Details(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //if exist get from cache, else get from db
            Product product = cacheRepository.GetOrSet(id, () => productRepository.GetById(id));
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Home/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Home/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,LastUpdateTime")] Product product)
        {
            if (ModelState.IsValid)
            {
                productRepository.Insert(product);
                productRepository.Save();

                //insert cache as well
                cacheRepository.Add(product);

                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Home/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = productRepository.GetById(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Home/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,LastUpdateTime")] Product product)
        {
            if (ModelState.IsValid)
            {
                productRepository.Update(product);
                productRepository.Save();

                //update cache
                cacheRepository.Update(product);

                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Home/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = productRepository.GetById(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Home/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            productRepository.Delete(id);
            productRepository.Save();

            //delete cache item
            cacheRepository.Delete(id);

            return RedirectToAction("Index");
        }

    }
}
