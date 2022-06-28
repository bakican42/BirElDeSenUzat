using BirEldeSenUzat.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BirEldeSenUzat.Controllers
{
    public class KullanicilarController : Controller
    {
        IhtiyacDB context = new IhtiyacDB();
        // GET: Kullanicilar
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Kullanicilar(int sayfa=1) 
        {
            var kullanicilar = context.KullaniciRols.Where(x => x.Kullanici.Status == true && x.Rol.RolAdi != "Admin").OrderByDescending(x => x.Kullanici.KayitTarihi).ToList().ToPagedList(sayfa, 10);

            return View(kullanicilar);
        }

        public ActionResult KullaniciDetay(int? id, int sayfa = 1)
        {
            var kullanicilar = context.Sepets.Where(x => x.KullaniciId == id).ToList().OrderByDescending(x => x.Tarih).ToPagedList(sayfa , 6);
            //Paged list olduğundan dolayı her sayfada toplam yapılmış olan tutarı yazması yerine kullanıcı sepet tutarını viewbag ile sayfaya gönderiyoruz.
            var kullaniciSepetTutari = context.Sepets.Where(x => x.KullaniciId == id).Sum(i => i.Toplam);
            ViewBag.sepetTutari = kullaniciSepetTutari;

            return View(kullanicilar);
        }

        public void KullaniciSil(int id)
        {
            var silinicekKullanici = context.KullaniciRols.Where(x => x.KullaniciID == id).FirstOrDefault();

            silinicekKullanici.Kullanici.Status = false;
            context.SaveChanges();

        }
    }
}