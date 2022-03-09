using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using WebApplication1.Models.DataContext;
using WebApplication1.Models.Model;

namespace WebApplication1.Controllers
{
    public class BlogController : Controller
    {
        private KitapsesimDBContext db = new KitapsesimDBContext();
        // GET: Blog
        public ActionResult Index()
        {
            return View(db.Blog.ToList().OrderByDescending(x=>x.BlogId));
        }
        public ActionResult Create()
        {
            ViewBag.KategoriId = new SelectList(db.Kategori, "KategoriId", "KategoriAd");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Blog blog,HttpPostedFileBase ResimURL)
        {
            if (ResimURL!= null)
            {
                
                WebImage img = new WebImage(ResimURL.InputStream);
                FileInfo imginfo = new FileInfo(ResimURL.FileName);

                string blogimgname = Guid.NewGuid().ToString() + imginfo.Extension;
                img.Resize(600,400);
                img.Save("~/Uploads/Blog/" + blogimgname);

                blog.ResimURL= "/Uploads/Blog/" + blogimgname;
            }
            db.Blog.Add(blog);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
          public ActionResult Edit(int id)
          {
            if (id == null)
            {
                return HttpNotFound();
            }
            var b = db.Blog.Where(x => x.BlogId == id).SingleOrDefault();
            if (b == null)
            {
                return HttpNotFound();
            }
            ViewBag.KategoriId = new SelectList(db.Kategori, "KategoriId", "KategoriAd",b.KategoriId);
            return View(b);
          }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit (int id,Blog blog,HttpPostedFileBase ResimURL)
        {
            if (ModelState.IsValid)
            {
                var b = db.Blog.Where(x => x.BlogId == id).SingleOrDefault();
                if (ResimURL != null)
                {

                    if (System.IO.File.Exists(Server.MapPath(b.ResimURL)))
                    {
                        System.IO.File.Delete(Server.MapPath(b.ResimURL));
                    }
                    WebImage img = new WebImage(ResimURL.InputStream);
                    FileInfo imginfo = new FileInfo(ResimURL.FileName);

                    string blogimgname = Guid.NewGuid().ToString() + imginfo.Extension;
                    img.Resize(600,400);
                    img.Save("~/Uploads/Blog/" + blogimgname);

                    b.ResimURL = "/Uploads/Blog/" + blogimgname;
                }
                b.Baslik = blog.Baslik;
                b.Icerik = blog.Icerik;
                b.KategoriId = blog.KategoriId;
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(blog);
        }
       
          public ActionResult Delete(int id)
          {
            var b = db.Blog.Find(id);
            if(b==null)
            {
                return HttpNotFound();
            }
            if (System.IO.File.Exists(Server.MapPath(b.ResimURL))) //gelen kayıt null değilse bu işlemleri yapar
            {
                System.IO.File.Delete(Server.MapPath(b.ResimURL));//alana gider kayıt var mı diye bakar varsa siler
            }
            db.Blog.Remove(b);  //bu kaydı burdan kaldır. 
            db.SaveChanges(); //veri tabanına kaydet.

            return RedirectToAction("Index"); //index metoduna dön.
            
          }
    }
}