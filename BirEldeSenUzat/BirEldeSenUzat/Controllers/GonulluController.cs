using BirEldeSenUzat.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BirEldeSenUzat.Controllers
{

    public class GonulluController : Controller
    {
        IhtiyacDB context = new IhtiyacDB();
        // GET: Gonullu
        [Authorize(Roles = "Gonullu")]
        public ActionResult Index()
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

        [HttpPost]
        public ActionResult Index(int? SehirId, int? ilceId, int? SemptId, string AdresDetay, int? id, string EskiParola, string Parola, string YeniParola, string AdSoyad, string Cinsiyet, DateTime? DogumTarihi, string TCNo, string TelNo, string Mail)
        {
            var guncellenecekKullanici = context.Kullanicis.Where(x => x.KullaniciID == id).FirstOrDefault();

            if(SehirId != null || ilceId != null || SemptId != null)
            {
                guncellenecekKullanici.SehirId = SehirId;
                guncellenecekKullanici.ilceId = ilceId;
                guncellenecekKullanici.SemtMahId = SemptId;
                guncellenecekKullanici.AdresDetay = AdresDetay;
                guncellenecekKullanici.TelNo = TelNo;
                guncellenecekKullanici.Mail = Mail;

                context.Entry(guncellenecekKullanici).State = System.Data.Entity.EntityState.Modified;// değişikleri kaydet.
                context.SaveChanges();
            }

            else
            {
                guncellenecekKullanici.AdSoyad = AdSoyad;
                guncellenecekKullanici.Cinsiyet = Cinsiyet;
                guncellenecekKullanici.TCNo = TCNo;
                guncellenecekKullanici.DogumTarihi = DogumTarihi;
                guncellenecekKullanici.TelNo = TelNo;
                guncellenecekKullanici.Mail = Mail;

                context.Entry(guncellenecekKullanici).State = System.Data.Entity.EntityState.Modified;// değişikleri kaydet.
                context.SaveChanges();
            }

            //Şifre
            if (EskiParola != "" && Parola != "" && YeniParola != "")
            {
                if (guncellenecekKullanici.Parola == EskiParola)
                {
                    if (Parola == YeniParola)
                    {
                        guncellenecekKullanici.Parola = Parola;
                        context.Entry(guncellenecekKullanici).State = System.Data.Entity.EntityState.Modified;// değişikleri kaydet.
                        context.SaveChanges();

                        TempData["Mesaj"] = "Parola değişimi başarıyla gerçekleştirildi.";

                        return RedirectToAction("Index", "Gonullu");
                    }

                    else
                    {
                        TempData["Mesaj"] = "Parolalarınız uyuşmamaktadır!!!";
                        return RedirectToAction("Index", "Gonullu");
                    }
                }

                else
                {
                    TempData["Mesaj"] = "Şifre değişimi için eski parolanızın doğru olması gereklidir!!!";
                    return RedirectToAction("Index", "Gonullu");
                }
            }

            TempData["Mesaj"] = "Bilgileriniz başarıyla kaydedilmiştir.";

            return RedirectToAction("Index", "Gonullu");
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

            Rol kullanici = context.Rols.FirstOrDefault(x => x.RolAdi == "Gonullu");

            KullaniciRol kr = new KullaniciRol();
            kr.RolID = kullanici.RolID;
            kr.KullaniciID = kl.KullaniciID;

            context.KullaniciRols.Add(kr);
            context.SaveChanges();

            TempData["Mesaj"] = "Üyelik kaydınız başarıyla gerçekleştirildi.";

            return View();
        }
    }
}