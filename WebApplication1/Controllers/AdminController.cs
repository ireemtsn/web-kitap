using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.DataContext;
using WebApplication1.Models.Model;

namespace WebApplication1.Controllers
{
    public class AdminController : Controller
    {

        KitapsesimDBContext db = new KitapsesimDBContext();
        // GET: Admin
        public ActionResult Index()
        {
            ViewBag.BlogSay = db.Blog.Count();
            ViewBag.KategoriSay = db.Kategori.Count();
            ViewBag.HizmetSay = db.Hizmet.Count();
            ViewBag.YorumSay = db.Yorum.Count();
            var sorgu = db.Kategori.ToList();

            return View(sorgu);
        }
        public ActionResult Login()
        {
        return View();
        }
        [HttpPost]
        public ActionResult Login(Admin admin)
        {
            var login = db.Admin.Where(x => x.Eposta == admin.Eposta).SingleOrDefault();
            if(login.Eposta==admin.Eposta&&login.Sifre==admin.Sifre)
            {
                Session["adminid"] = login.AdminId;
                Session["eposta"] = login.Eposta;
                return RedirectToAction("Index", "Admin");
            }
            ViewBag.Uyari  = "Kullanıcı adı ya da şifre yanlış";
           
            return View(admin);
        }
    }
}