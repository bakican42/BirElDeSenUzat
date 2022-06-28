using BirEldeSenUzat.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BirEldeSenUzat.Controllers
{
    [Authorize]
    public class KategoriController : Controller
    {
        
        IhtiyacDB context = new IhtiyacDB();
        // GET: Kategori
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Kategoriler(int sayfa=1)
        {
            var kategoriler = context.Kategoris.ToList().ToPagedList(sayfa,10);

            return View(kategoriler);
        }

        public ActionResult KategoriEkle()
        {
            return View();
        }

        [HttpPost]
        public ActionResult KategoriEkle(Kategori kategori)
        {
            context.Kategoris.Add(kategori);
            context.SaveChanges();
            return RedirectToAction("Kategoriler");
        }

        public ActionResult KategoriSilme(int id)
        {
            var silinicekKategori = context.Kategoris.Where(x => x.KategoriID == id).FirstOrDefault();
            return View(silinicekKategori);
        }

        [HttpPost]
        public ActionResult KategoriSilme(int id, Kategori k)
        {
            var silinicekKategori = context.Kategoris.Where(x => x.KategoriID == id).FirstOrDefault();
            var bagis = context.Bagislars.Where(x => x.KategoriID == id).FirstOrDefault();

            if (bagis != null)
            {
                TempData["Mesaj"] = "Kategori kaydını silmeden o kategoriye bağlı bağışları silmelisiniz!!!!";
                return RedirectToAction("Kategoriler");
            }
            else
            {
                context.Entry(silinicekKategori).State = System.Data.Entity.EntityState.Deleted;
                context.SaveChanges();
                return RedirectToAction("Kategoriler");
            }
        }

        [HttpGet]
        public ActionResult KategoriGuncelle(int id)
        {
            var guncellenecekKategori = context.Kategoris.Where(x => x.KategoriID == id).FirstOrDefault(); 
            return View(guncellenecekKategori);
        }

        [HttpPost]
        public ActionResult KategoriGuncelle(Kategori k, int id)
        {
            var guncellenecekKategori = context.Kategoris.Where(x => x.KategoriID == id).FirstOrDefault(); // güncellenecek kategorinin idsinin yakalıyoruz
            guncellenecekKategori.KategoriID = id;// güncellecenk kategorinin id si değişmez idyi yapıştır
            guncellenecekKategori.kategori_ad = k.kategori_ad;// güncellecenek kategorinin adı.
            guncellenecekKategori.aciklama = k.aciklama; // güncellenecek kategorinin açıklaması

            context.Entry(guncellenecekKategori).State = System.Data.Entity.EntityState.Modified;// değişikleri kaydet.
            context.SaveChanges();
            return RedirectToAction("Kategoriler");//kategoriler sayfasına dön.
        }


    }
}