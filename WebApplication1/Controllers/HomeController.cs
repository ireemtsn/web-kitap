using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using WebApplication1.Models.DataContext;
using PagedList;
using PagedList.Mvc;
using WebApplication1.Models.Model;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private KitapsesimDBContext db = new KitapsesimDBContext();
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.Hizmetler = db.Hizmet.ToList().OrderByDescending(x => x.HizmetId);

          

            return View();
        }
        public ActionResult SliderPartial()
        {
            return View(db.Slider.ToList().OrderByDescending(x => x.Sliderid));
        }
        public ActionResult HizmetPartial()
        {
            return View(db.Hizmet.ToList());
        }
        public ActionResult Hakkimizda()
        {
           

            ViewBag.Blog = db.Blog.ToList().OrderByDescending(x => x.BlogId);

            return View(db.Hakkimizda.SingleOrDefault());
        }
        public ActionResult Hizmetlerimiz()
        {
            return View(db.Hizmet.ToList().OrderByDescending(x => x.HizmetId));
        }
        public ActionResult Iletişim()
        {
            return View(db.Iletisim.SingleOrDefault());
        }
        [HttpPost]
        public ActionResult Iletişim(string adsoyad=null,string email=null,string konu=null,string mesaj=null)
        {
            //Mail Gönderme İşlemi!!!!

            if(adsoyad!=null && email!=null)
            {
                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.EnableSsl = true;
                WebMail.UserName = "ireem.tsn@gmail.com"; //güvenli bağlantı
                WebMail.Password= "12345";
                WebMail.SmtpPort = 587;
                WebMail.Send("tosun.1640@gmail.com", konu, email + "</br>" + mesaj);
                ViewBag.Uyari = "Mesajınız başarı ile gönderilmiştir.";
            }
            else
            {
                ViewBag.Uyari = "Hata oluştu tekar deneyiniz";
            }
            return View();
        } 
         public ActionResult BlogKategoriPartial()
        {
            return PartialView(db.Kategori.Include("Blogs").ToList().OrderBy(x =>x.KategoriAd)); //blog kayıtlarına göre getirir
        }
        public ActionResult BlogKayitPartial()
        {
            return PartialView(db.Blog.ToList().OrderByDescending(x=>x.BlogId)); //blog kayıtlarına göre getirir
        }

        public ActionResult Blog(int Sayfa = 1)
        {
            return View(db.Blog.Include("Kategori").OrderByDescending(x => x.BlogId).ToPagedList(Sayfa, 5));
        }
        public ActionResult KategoriBlog(int id,int sayfa=1)
        {
            var b = db.Blog.Include("kategori").OrderByDescending(x=>x.BlogId).Where(x => x.Kategori.KategoriId == id).ToPagedList(sayfa,5);
            return View(b);
        }
       
        public ActionResult BlogDetay(int id)
        {
            var b = db.Blog.Include("kategori").Include("yorums").Where(x => x.BlogId == id).SingleOrDefault();
            return View(b);
        }
        public JsonResult YorumYap(string adsoyad,string eposta, string icerik,int blogid)
        {
            if (icerik == null)
            {
              return Json(true, JsonRequestBehavior.AllowGet);
            }
            db.Yorum.Add(new Yorum{ AdSoyad = adsoyad, Eposta = eposta, Icerik = icerik, Blogid = blogid,Onay=false});
            db.SaveChanges();
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FooterPartial()
        {
            ViewBag.Hizmetler = db.Hizmet.ToList().OrderByDescending(x => x.HizmetId);

            ViewBag.Iletisim = db.Iletisim.SingleOrDefault();

            return PartialView();
        }
       
    }
}