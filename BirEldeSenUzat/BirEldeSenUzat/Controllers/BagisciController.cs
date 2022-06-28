using BirEldeSenUzat.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using PagedList;
using Rotativa;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BirEldeSenUzat.Controllers
{
    
    public class BagisciController : Controller
    {
        IhtiyacDB context = new IhtiyacDB();
        // GET: Bagisci
        [Authorize(Roles = "Bagisci,Admin")]
        public ActionResult Index(int? id, int sayfa = 1)
        {
            string girisYapankullanici = User.Identity.Name;
            var kullanici = context.Kullanicis.Where(x => x.AdSoyad == girisYapankullanici).FirstOrDefault();

            var sepet = context.Sepets.Where(x => x.KullaniciId == kullanici.KullaniciID && x.OdemeTamamlandiMi == true).OrderByDescending(x => x.Tarih).ToList().ToPagedList(sayfa, 4);

            return View(sepet);
        }
        
        public ActionResult UyeOl()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UyeOl(Kullanici kl)
        {
            kl.KayitTarihi = DateTime.Now;

            kl.OnaylandiMi = true;
            kl.Status = true;
            context.Kullanicis.Add(kl);
            context.SaveChanges();

            Rol kullanici = context.Rols.FirstOrDefault(x => x.RolAdi == "Bagisci");

            KullaniciRol kr = new KullaniciRol();
            kr.RolID = kullanici.RolID;
            kr.KullaniciID = kl.KullaniciID;

            context.KullaniciRols.Add(kr);
            context.SaveChanges();

            TempData["Mesaj"] = "Üyelik kaydınız başarıyla gerçekleştirildi.";    

            return View();
        }

        public ActionResult TemelBilgiler()
        {
            string kullaniciAdSoyad = User.Identity.Name;
            var data = context.Kullanicis.Where(x => x.AdSoyad == kullaniciAdSoyad).FirstOrDefault();

            return View(data);
        }

        [HttpPost]
        public ActionResult TemelBilgiler(Kullanici kl, int id)
        {
            var guncellenecekKullanici = context.Kullanicis.Where(x => x.KullaniciID == id).FirstOrDefault();

            guncellenecekKullanici.AdSoyad = kl.AdSoyad;
            guncellenecekKullanici.Cinsiyet = kl.Cinsiyet;
            guncellenecekKullanici.TCNo = kl.TCNo;
            guncellenecekKullanici.DogumTarihi = kl.DogumTarihi;

            context.Entry(guncellenecekKullanici).State = System.Data.Entity.EntityState.Modified;// değişikleri kaydet.
            context.SaveChanges();

            TempData["Mesaj"] = "Bilgileriniz başarıyla kaydedilmiştir.";

            return RedirectToAction("Index","Bagisci");
        }

        public ActionResult iletisimBilgileri()
        {
            string kullaniciAdSoyad = User.Identity.Name;
            var kullaniciBilgi = context.Kullanicis.Where(x => x.AdSoyad == kullaniciAdSoyad).FirstOrDefault(); 

            return View(kullaniciBilgi);
        }

        [HttpPost]
        public ActionResult iletisimBilgileri(Kullanici kl, int id)
        {
            var guncellenecekKullanici = context.Kullanicis.Where(x => x.KullaniciID == id).FirstOrDefault();

            guncellenecekKullanici.KullaniciID = id;
            guncellenecekKullanici.TelNo = kl.TelNo;
            guncellenecekKullanici.Mail = kl.Mail;

            context.Entry(guncellenecekKullanici).State = System.Data.Entity.EntityState.Modified;// değişikleri kaydet.
            context.SaveChanges();

            TempData["Mesaj"] = "Bilgileriniz başarıyla kaydedilmiştir.";

            return RedirectToAction("Index", "Bagisci");
        }

        public ActionResult KonumBilgileri()
        {
            string kullaniciAdSoyad = User.Identity.Name;
            var kullaniciBilgi = context.Kullanicis.Where(x => x.AdSoyad == kullaniciAdSoyad).FirstOrDefault();
            List<Sehirler> SehirListesi = context.Sehirlers.OrderBy(x => x.SehirAdi).ToList();
            ViewBag.Sehirler = new SelectList(SehirListesi, "SehirId", "SehirAdi");

            if (kullaniciBilgi.SehirId != null && kullaniciBilgi.ilceId != null && kullaniciBilgi.SemtMahId != null)
            {
                List<Ilceler> İlceler = context.Ilcelers.Where(x => x.SehirId == kullaniciBilgi.SehirId).ToList();
                ViewBag.ilceler = new SelectList(İlceler, "ilceId", "IlceAdi");

                List<SemtMah> SempMah = context.SemtMahs.Where(x => x.ilceId == kullaniciBilgi.ilceId).ToList();
                ViewBag.SempMah = new SelectList(SempMah, "SemtMahId", "MahalleAdi");
            }

            return View(kullaniciBilgi);
        }

        public JsonResult İlceGetir(int SehirId)
        {
            context.Configuration.ProxyCreationEnabled = false;
            List<Ilceler> İlceListesi = context.Ilcelers.Where(x => x.SehirId == SehirId).OrderBy(x => x.IlceAdi).ToList();
            return Json(İlceListesi, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SemtGetir(int ilceId)
        {
            context.Configuration.ProxyCreationEnabled = false;
            List<SemtMah> SemtListesi = context.SemtMahs.Where(x => x.ilceId == ilceId).OrderBy(x => x.MahalleAdi).ToList();
            return Json(SemtListesi, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult KonumBilgileri(int SehirId, int ilceId, int SemptId, string AdresDetay, int id)
        {
            var guncellenecekKullanici = context.Kullanicis.Where(x => x.KullaniciID == id).FirstOrDefault();

            guncellenecekKullanici.SehirId = SehirId;
            guncellenecekKullanici.ilceId = ilceId;
            guncellenecekKullanici.SemtMahId = SemptId;
            guncellenecekKullanici.AdresDetay = AdresDetay;

            context.Entry(guncellenecekKullanici).State = System.Data.Entity.EntityState.Modified;// değişikleri kaydet.
            context.SaveChanges();

            TempData["Mesaj"] = "Bilgileriniz başarıyla kaydedilmiştir.";

            return RedirectToAction("Index", "Bagisci");
        }

        public ActionResult sifredegistir()
        {
            string kullaniciAdSoyad = User.Identity.Name;
            var kullaniciBilgi = context.Kullanicis.Where(x => x.AdSoyad == kullaniciAdSoyad).FirstOrDefault();

            return View(kullaniciBilgi);
        }

        [HttpPost]
        public ActionResult sifredegistir(Kullanici kl, int id, string EskiParola, string Parola, string YeniParola)
        {
            var guncellenecekKullanici = context.Kullanicis.Where(x => x.KullaniciID == id).FirstOrDefault();
            
            if(guncellenecekKullanici.Parola == EskiParola)
            {
                if (Parola == YeniParola)
                {
                    guncellenecekKullanici.Parola = Parola;
                    context.Entry(guncellenecekKullanici).State = System.Data.Entity.EntityState.Modified;// değişikleri kaydet.
                    context.SaveChanges();

                    TempData["Başarılı"] = "Parola değişimi başarıyla gerçekleştirildi.";

                    return RedirectToAction("sifredegistir", "Bagisci");
                }

                else
                {
                    TempData["Mesaj"] = "Parolalarınız uyuşmamaktadır!!!";
                }
            }

            else
            {
                TempData["Mesaj1"] = "Şifre değişimi için eski parolanızın doğru olması gereklidir!!!";
            }
            return RedirectToAction("sifredegistir", "Bagisci");
        }

        public ActionResult BagisYap()
        {
            var data = context.Bagislars.Where(x => x.Status == false).ToList();

            return View(data);
        }

        public ActionResult MakbuzDetay(int? id)
        {
            string kullaniciAdSoyad = User.Identity.Name;
            var kullaniciBilgi = context.Kullanicis.Where(x => x.AdSoyad == kullaniciAdSoyad).Select(x => x.KullaniciID).FirstOrDefault();
            
            var BagisDetay = context.Sepets.Where(x => x.KullaniciId == kullaniciBilgi && x.BagisId == id).FirstOrDefault();

            return View(BagisDetay);
        }
    }
}