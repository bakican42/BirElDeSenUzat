using BirEldeSenUzat.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BirEldeSenUzat.Controllers
{
    [Authorize]
    public class BagisController : Controller
    {
        IhtiyacDB context = new IhtiyacDB();
        // GET: Bagislar
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Bagislar(int sayfa = 1)
        {
            var bagislar = context.Bagislars.Where(x => x.Status == false).ToList().ToPagedList(sayfa, 5);

            return View(bagislar);
        }

        [HttpGet]
        public ActionResult BagisOlusturma()
        {
            var Kategoriler = context.Kategoris.Select(x => x.kategori_ad).ToList();
            ViewBag.Kategoriler = Kategoriler;

            return View();
        }

        [HttpPost]
        public ActionResult BagisOlusturma(HttpPostedFileBase resimadi, string KategoriAdi, string aciklama, string Baslik, UlasanElleriniz ulasanElleriniz)
        {
            var Kategoriler = context.Kategoris.Select(x => x.kategori_ad).ToList();// Kategori adlarını sayfamıza çekiyoruz
            ViewBag.Kategoriler = Kategoriler;

            string kullanici = User.Identity.Name; // oturum açan kullanıcının kim olduğunu belirliyoruz
            ViewBag.KullaniciAdSoyad = kullanici;

            Bagislar bagis = new Bagislar();
            int kategoriID = context.Kategoris.Where(x => x.kategori_ad == KategoriAdi).Select(x => x.KategoriID).FirstOrDefault();// seçmiş olduğumuz kategori adını göre id bilgisini kategori tablosundan yakalıyoruz
            int KullaniciID = context.Kullanicis.Where(x => x.AdSoyad == kullanici).Select(x => x.KullaniciID).FirstOrDefault();// oturum açan kullanıcının kim olduğunu id bilgisine göre kullanıcı tablosundan yakalıyoruz
            string gd = Guid.NewGuid().ToString().Substring(0, 8); // resim adlarının karışmasının önüne geçilmesi için resime bir guid atamaıs yapıyoruz.
            var originalDirectory = new DirectoryInfo(string.Format("{0}BagisResim\\", Server.MapPath(@"\"))); // resmin hangi klasöre gideceğini belirliyoruz
            string pathString = System.IO.Path.Combine(originalDirectory.ToString(), "Resimler"); // resmin klasördeki adına belirliyoruz.
            var fileName = gd + "_" + Path.GetFileName(resimadi.FileName); // resmi belirtilen konumdaki klasöre kaydediyoruz.
            bool isExists = System.IO.Directory.Exists(pathString);// istediğimiz klasör açıldımı açılmadıysa 

            if (!isExists)
                System.IO.Directory.CreateDirectory(pathString); // klasörü açıyoruz.

            var path = string.Format("{0}\\{1}", pathString, fileName); // resmin yol ve ad bilgisine göre
            resimadi.SaveAs(path); // resmi klasöre kaydediyoruz.

            bagis.resimadi = fileName; // resim adı database bölümü kaydet.
            bagis.aciklama = aciklama; // resim açıklama database bölümü kaydet.
            bagis.KullaniciID = KullaniciID; // resim kullanici id database bölümü kaydet.
            bagis.KategoriID = kategoriID; // resim kategori id database bölümü kaydet.
            bagis.Baslik = Baslik; // resim başlık database bölümü kaydet.
            bagis.OlusturulmaTarihi = DateTime.Now; // resim oluşturulma database bölümü kaydet.
            bagis.Status = false;

            context.Bagislars.Add(bagis); // veritabanına ekle
            context.SaveChanges(); // değişiklikleri kaydet.
            ulasanElleriniz.BagisID = bagis.id;
            ulasanElleriniz.OnaylandiMi = false;
            context.UlasanEllerinizs.Add(ulasanElleriniz);
            context.SaveChanges();

            return RedirectToAction("Bagislar");
        }

        public ActionResult BagisDetayGoster(int id)
        {
            var Kategoriler = context.Kategoris.Select(x => x.kategori_ad).ToList();// Kategori adlarını sayfamıza çekiyoruz
            ViewBag.Kategori = Kategoriler;

            var bagis = context.Bagislars.Where(x => x.id == id).FirstOrDefault();
            return View(bagis);
        }

        [HttpPost]
        public ActionResult BagisDetayGoster(HttpPostedFileBase yeniresimadi,int id, string baslik, string aciklama, string KategoriAdi)
        {
            var bagis = context.Bagislars.Where(x => x.id == id).FirstOrDefault(); // güncellenecek kategorinin idsinin yakalıyoruz
            int kategoriID = context.Kategoris.Where(x => x.kategori_ad == KategoriAdi).Select(x => x.KategoriID).FirstOrDefault();// seçmiş olduğumuz kategori adını göre id bilgisini kategori tablosundan yakalıyoruz

            var Kategoriler = context.Kategoris.Select(x => x.kategori_ad).ToList();// Kategori adlarını sayfamıza çekiyoruz
            ViewBag.Kategori = Kategoriler;

            if (yeniresimadi != null) // eğer resim güncellemek istiyorsak buraya giricek
            {
                var resimAdi = context.Bagislars.Where(m => m.id == id).Select(x => x.resimadi).FirstOrDefault(); // eski resmin adını yakalıcaz.

                if (System.IO.File.Exists(Server.MapPath("~/BagisResim/Resimler/" + resimAdi)))
                {
                    System.IO.File.Delete(Server.MapPath("~/BagisResim/Resimler/" + resimAdi)); // eski resmi klasörden silicez.
                }

                string gd = Guid.NewGuid().ToString().Substring(0, 8); // resim adlarının karışmasının önüne geçilmesi için resime bir guid atamaıs yapıyoruz.
                var originalDirectory = new DirectoryInfo(string.Format("{0}BagisResim\\", Server.MapPath(@"\"))); // resmin hangi klasöre gideceğini belirliyoruz
                string pathString = System.IO.Path.Combine(originalDirectory.ToString(), "Resimler"); // resmin klasördeki adına belirliyoruz.
                var fileName = gd + "_" + Path.GetFileName(yeniresimadi.FileName); // resmi belirtilen konumdaki klasöre kaydediyoruz.

                var path = string.Format("{0}\\{1}", pathString, fileName); // resmin yol ve ad bilgisine göre
                yeniresimadi.SaveAs(path); // resmi klasöre kaydediyoruz.

                bagis.resimadi = fileName; // yeni resmin veritabanında kayda ait bölümünü güncelliyoruz
                bagis.KategoriID = kategoriID;
                bagis.aciklama = aciklama;
                bagis.Baslik = baslik;

                context.Entry(bagis).State = System.Data.Entity.EntityState.Modified;// değişikleri kaydet.
                context.SaveChanges();
                return RedirectToAction("Bagislar");
            }

            else
            {
                bagis.resimadi = bagis.resimadi;
                bagis.KategoriID = kategoriID;
                bagis.aciklama = aciklama;
                bagis.Baslik = baslik;
                context.Entry(bagis).State = System.Data.Entity.EntityState.Modified;// değişikleri kaydet.
                context.SaveChanges();
                return RedirectToAction("Bagislar");
            }
        }

        public void BagisSil(int id)
        {
            var silinicekBagis = context.Bagislars.Where(x => x.id == id).FirstOrDefault();
            silinicekBagis.Status = true;
            context.Entry(silinicekBagis).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
        }

    }
}