using BirEldeSenUzat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BirEldeSenUzat.Controllers
{
    public class KullaniciController : Controller
    {
        IhtiyacDB context = new IhtiyacDB();
        // GET: Kullanici
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GirisYap()
        { 

            return View();
        }

        [HttpPost]
        public ActionResult GirisYap(Kullanici kl)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("GirisYap", "Kullanici");
            }

            var kullaniciVarmi = context.Kullanicis.FirstOrDefault(x => x.Mail == kl.Mail && x.Parola == kl.Parola && x.Status == true);
            var kullaniciRol = context.Kullanicis.Where(x => x.Mail == kl.Mail && x.Parola == kl.Parola).Select(x => x.KullaniciID).FirstOrDefault();
            var rol = context.KullaniciRols.Where(x => x.KullaniciID == kullaniciRol).Select(x => x.Rol.RolAdi).FirstOrDefault();            

            if (kullaniciVarmi != null)
            {
                if (rol == "Admin")
                {
                    Session["uyeid"] = kullaniciVarmi.KullaniciID;

                    FormsAuthentication.SetAuthCookie(kullaniciVarmi.AdSoyad, false);
                    return RedirectToAction("Index", "Admin");
                }

                else if(rol == "Bagisci")
                {
                    Session["uyeid"] = kullaniciVarmi.KullaniciID;

                    FormsAuthentication.SetAuthCookie(kullaniciVarmi.AdSoyad, false);
                    return RedirectToAction("Index", "Bagisci");
                }

                else if (rol == "Gonullu")
                {
                    Session["uyeid"] = kullaniciVarmi.KullaniciID;

                    FormsAuthentication.SetAuthCookie(kullaniciVarmi.AdSoyad, false);
                    return RedirectToAction("Index", "Gonullu");
                }
            }

            else
            {
                ViewBag.Mesaj = "Böyle bir kullanıcı bulunamadı. Lütfen mail ve parola bilgisini tekrardan kontrol ediniz.";
                return View();
            }

            return View();
        }

        
        public ActionResult CikisYap()
        {
            Session.Abandon();
            Response.Cookies.Clear();

            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Sepetim()
        {
            string girisYapankullanici = User.Identity.Name;
            var kullanici = context.Kullanicis.Where(x => x.AdSoyad == girisYapankullanici).FirstOrDefault();

            var sepet = context.Sepets.Where(x => x.KullaniciId == kullanici.KullaniciID && x.OdemeTamamlandiMi == false).ToList();

            ViewBag.BagisSayisi = sepet.Count();

            return View(sepet);
        }

        [HttpPost]
        public ActionResult Sepetim(int? bagisId, int? bagisMiktari)
        {
            string girisYapankullanici = User.Identity.Name;
            var kullanici = context.Kullanicis.Where(x => x.AdSoyad == girisYapankullanici).FirstOrDefault();

            Sepet spt = new Sepet();

            spt.KullaniciId = kullanici.KullaniciID;
            spt.BagisId = bagisId;
            spt.Miktar = bagisMiktari;
            spt.UrunSayisi = 1;
            spt.Toplam = bagisMiktari * spt.UrunSayisi;
            spt.Tarih = DateTime.Now;
            spt.OdemeTamamlandiMi = false;

            context.Sepets.Add(spt);
            context.SaveChanges();
            return RedirectToAction("BagisYap", "Bagisci");
        }

        public void MiktarArtir(int id)
        {
            var urun = context.Sepets.Where(x => x.Id == id).FirstOrDefault();

            urun.UrunSayisi++;

            urun.Toplam = urun.Miktar * urun.UrunSayisi;
           
            context.Entry(urun).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
        }

        public void MiktarAzalt(int id)
        {
            var urun = context.Sepets.Where(x => x.Id == id).FirstOrDefault();

            if(urun.UrunSayisi <= 1)
            {

            }

            else
            {
                urun.UrunSayisi--;
                urun.Toplam = urun.Miktar * urun.UrunSayisi;
            }
            
            context.Entry(urun).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
        }

        public void BagisiSepettenCikar(int id)
        {
            var silinecekBagis = context.Sepets.Where(x => x.Id == id).FirstOrDefault();
            context.Entry(silinecekBagis).State = System.Data.Entity.EntityState.Deleted;
            context.SaveChanges();
        }
        
        public ActionResult SifremiUnuttum()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SifremiUnuttum(string Mail)
        {
            var data = context.Kullanicis.Where(m => m.Mail == Mail).FirstOrDefault();
            var id = context.Kullanicis.Where(m => m.Mail == Mail).Select(x => x.KullaniciID).FirstOrDefault();

            if (data != null)
            {
                Random rnd = new Random();
                int kod = rnd.Next();
                System.Web.HttpContext.Current.Session["Kod"] = kod;

                MailMessage mail = new MailMessage();
                mail.IsBodyHtml = true;
                mail.To.Add(data.Mail);
                mail.From = new MailAddress("bireldesenuzat@outlook.com", "Şifre Güncelleme", System.Text.Encoding.UTF8);
                mail.Subject = "Şifre Güncelleme Talebi";
                mail.Body = $"Şifre Yenileme Doğrulama Kodunuz : " + kod;
                mail.IsBodyHtml = true;

                SmtpClient smp = new SmtpClient();

                smp.EnableSsl = true;
                smp.UseDefaultCredentials = false;
                smp.Credentials = new NetworkCredential("bireldesenuzat@outlook.com", "baki.1907");
                smp.Port = 587;
                smp.Host = "smtp.office365.com";
                smp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smp.Send(mail);

                return Redirect("/Kullanici/SifreYenile/" + id);
            }

            else
            {
                ViewBag.Mesaj = "Bir hata oluştu. Mail adresinizi kontrol ediniz!!!.";
                return View();
            }
        }

        public ActionResult SifreYenile(int? id)
        {
            if(id!= null)
            {
                var sifreunutan = context.Kullanicis.Where(m => m.KullaniciID == id).SingleOrDefault();
                return View(sifreunutan);
            }
            return View();
        }

        [HttpPost]
        public ActionResult SifreYenile(string sifre, int kod, string sifreTekrar, int? id)
        {
            var sifreunutan = context.Kullanicis.Where(m => m.KullaniciID == id).SingleOrDefault();

            if (kod == (int)Session["Kod"])
            {
                if (sifre == sifreTekrar)
                {
                    sifreunutan.Parola = sifre.ToString();
                    context.SaveChanges();
                    Session.Abandon();
                    ViewBag.Mesaj = "Şifre Değişimi Sağlandı! Giriş Yapabilirsiniz.";
                    return View("GirisYap", sifreunutan);
                }
                else
                {
                    ViewBag.Mesaj1 = "Şifreleriniz eşleşmemektedir. Lütfen yeni parolanızı tekrardan kontrol ediniz!";
                    return View(sifreunutan);
                }
            }

            else
            {
                ViewBag.Mesaj1 = "Bir hata oluştu. Kodunuzu, Mail Adresinizi kontrol ederek tekrar deneyiniz!";
                return View(sifreunutan);
            }
        }
    }

}