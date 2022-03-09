using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models.DataContext;
using WebApplication1.Models.Model;

namespace WebApplication1.Controllers
{
    public class HakkimizdaController : Controller
    {
        KitapsesimDBContext db = new KitapsesimDBContext();
        // GET: Hakkimizda
        public ActionResult Index()
        {
            var h = db.Hakkimizda.ToList();
            return View(h);
        }
        public ActionResult Edit(int id)
        {
            var h = db.Hakkimizda.Where(x => x.HakkmizdaId == id).FirstOrDefault();
            return View(h);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Hakkimizda h)
        {
            if(ModelState.IsValid)
            {
                var hakkimizda = db.Hakkimizda.Where(x => x.HakkmizdaId == id).SingleOrDefault();
                hakkimizda.Aciklama =h.Aciklama;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(h);
        }
       
    
    }
}