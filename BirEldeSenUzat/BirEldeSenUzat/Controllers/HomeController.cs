using BirEldeSenUzat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BirEldeSenUzat.Controllers
{
    public class HomeController : Controller
    {
        IhtiyacDB context = new IhtiyacDB();

        public ActionResult Index()
        {
            var bagislar = context.Bagislars.Where(x => x.Status == false).ToList();

            return View(bagislar);
        }

        public PartialViewResult UlasanEllerWidget()
        {
            var data = context.UlasanEllerinizs.Where(x => x.Bagislar.Status == false && x.OnaylandiMi == true).ToList();

            return PartialView(data);
        }

        [HttpPost]
        public ActionResult Iletisim(Iletisim ilt)
        {
            Iletisim iletisim = new Iletisim();

            iletisim.AdSoyad = ilt.AdSoyad;
            iletisim.MailAdresi = ilt.MailAdresi;
            iletisim.Konu = ilt.Konu;
            iletisim.MesajIcerigi = ilt.MesajIcerigi;
            iletisim.Tarih = DateTime.Now;

            context.Iletisims.Add(iletisim);
            context.SaveChanges();

            TempData["SuccessMessage"] = "Mesajınız başarıyla iletildi.";
            return Redirect("/Home/Index");
        }
    }
}