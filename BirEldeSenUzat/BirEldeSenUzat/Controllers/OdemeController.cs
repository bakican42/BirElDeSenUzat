using BirEldeSenUzat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace BirEldeSenUzat.Controllers
{
    public class OdemeController : Controller
    {
        IhtiyacDB context = new IhtiyacDB();
        // GET: Odeme
        public ActionResult Index()
        {
            string kullaniciAdSoyad = User.Identity.Name;
            var kullaniciBilgi = context.Sepets.Where(x => x.Kullanici.AdSoyad == kullaniciAdSoyad).ToList();

            return View(kullaniciBilgi);
        }

        [HttpPost]
        public ActionResult Index(Sepet sepet)
        {
            string kullaniciAdSoyad = User.Identity.Name;
            var kullanici = context.Sepets.Where(x => x.Kullanici.AdSoyad == kullaniciAdSoyad && x.OdemeTamamlandiMi == false).ToList();

            foreach (var item in kullanici)
            {
                item.OdemeTamamlandiMi = true;

                context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }

            return View();
        }

        [HttpGet]
        public ActionResult MailGonder(string kartNo, string tutar)
        {
            string sonDortSıra = kartNo.Substring(kartNo.Length - 4, 4);
            System.Web.HttpContext.Current.Session["KartNumarasi"] = "************"+ sonDortSıra;

            string kullaniciAdSoyad = User.Identity.Name;
            var kullanici = context.Kullanicis.Where(x => x.AdSoyad == kullaniciAdSoyad).FirstOrDefault();
            var kullaniciTelNo = context.Sepets.Where(x => x.Kullanici.AdSoyad == kullaniciAdSoyad).Select(x => x.Kullanici.TelNo).FirstOrDefault();
            string basdorttelNo = kullaniciTelNo.Substring(kullaniciTelNo.Length - 16 ,6);
            string ikitelNo = kullaniciTelNo.Substring(kullaniciTelNo.Length - 5, 2);
            string sonikittelNo = kullaniciTelNo.Substring(kullaniciTelNo.Length - 2, 2);

            System.Web.HttpContext.Current.Session["TelNo"] = basdorttelNo + "XXX" + ikitelNo + sonikittelNo;

            DateTime tarih = DateTime.Now;
            System.Web.HttpContext.Current.Session["Tarih"] = tarih;
            System.Web.HttpContext.Current.Session["Tutar"] = tutar;

            if (kullanici != null)
            {
                Random rnd = new Random();
                int kod = rnd.Next();
                System.Web.HttpContext.Current.Session["Kod"] = kod;

                MailMessage mail = new MailMessage();
                mail.IsBodyHtml = true;
                mail.To.Add(kullanici.Mail);
                mail.From = new MailAddress("bireldesenuzat@outlook.com", "Ödeme İşlemi", System.Text.Encoding.UTF8);
                mail.Subject = "Ödeme İşlemi Talebi";
                mail.Body = $"Ödeme İşlemi Doğrulama Kodunuz : " + kod;
                mail.IsBodyHtml = true;

                SmtpClient smp = new SmtpClient();

                smp.EnableSsl = true;
                smp.UseDefaultCredentials = false;
                smp.Credentials = new NetworkCredential("bireldesenuzat@outlook.com", "baki.1907");
                smp.Port = 587;
                smp.Host = "smtp.office365.com";
                smp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smp.Send(mail);

                return Redirect("/Odeme/OdemeEkranı");
            }
            else
            {
                ViewBag.Uyari = "Bir hata oluştu. Tekrar Deneyiniz!";
            }

            return View("Odeme/Index");
        }

        public ActionResult OdemeEkranı()
        {
            string kullaniciAdSoyad = User.Identity.Name;
            var kullanici = context.Sepets.Where(x => x.Kullanici.AdSoyad == kullaniciAdSoyad).FirstOrDefault();

            return View(kullanici);
        }

        [HttpPost]
        public ActionResult OdemeEkranı(int? kod)
        {
            if (kod == (int)Session["Kod"])
            {
                string kullaniciAdSoyad = User.Identity.Name;
                var kullanici = context.Sepets.Where(x => x.Kullanici.AdSoyad == kullaniciAdSoyad && x.OdemeTamamlandiMi == false).ToList();

                foreach (var item in kullanici)
                {
                    item.OdemeTamamlandiMi = true;
                    item.Tarih = DateTime.Now;

                    context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                }
                return Redirect("/Odeme/OdemeBasarili");
            }
            else
            {
                ViewBag.Uyari = "Bir hata oluştu. Kodunuzu, Mail Adresinizi kontrol ederek tekrar deneyiniz!";
            }

            return View();
        }

        public ActionResult OdemeBasarili()
        {
            return View();
        }
    }
}