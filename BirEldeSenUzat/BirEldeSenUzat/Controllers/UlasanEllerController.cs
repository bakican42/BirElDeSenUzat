using BirEldeSenUzat.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BirEldeSenUzat.Controllers
{
    public class UlasanEllerController : Controller
    {
        IhtiyacDB context = new IhtiyacDB();
        // GET: UlasanEller
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UlasanElleriniz(int? id)
        {
            if(id != null)
            {
                var data = context.UlasanEllerinizs.Where(x => x.BagisID == id).FirstOrDefault();               
                return View(data);
            }
            return View();
        }
        [HttpPost]
        public ActionResult UlasanElleriniz(UlasanElleriniz veri, int? id, int bagisID)
        {
            var data = context.UlasanEllerinizs.Where(x => x.ID == id).FirstOrDefault();
            //Bagis Tutarımızı bulmak için
            var toplamBagiTutari = context.UlasanEllerinizs.Where(x => x.BagisID == id).FirstOrDefault();
            var sepetBagisTutar = context.Sepets.Where(x => x.BagisId == data.BagisID).Sum(i => i.Toplam);
            var bagisUlasimSayisi = sepetBagisTutar / veri.BagisAdetMiktari;
            var deger = String.Join(",", bagisUlasimSayisi);

            data.BagisID = data.BagisID;
            data.BagisAdetMiktari = veri.BagisAdetMiktari;
            data.Aciklama = veri.Aciklama;
            data.BagisUlasimSayisi = deger;
            data.OnaylandiMi = true;

            context.Entry(data).State = System.Data.Entity.EntityState.Modified;// değişikleri kaydet.
            context.SaveChanges();

            return View("Index", data);
        }
        public JsonResult BagisBilgiGetir(int? id)
        {
            var bagislar = context.UlasanEllerinizs.Where(m => m.BagisID == id).ToList();
            //Bagis Tutarımızı bulmak için
            var data = context.UlasanEllerinizs.Where(x => x.BagisID == id).FirstOrDefault();
            var sepetBagisTutar = context.Sepets.Where(x => x.BagisId == data.BagisID).Sum(i => i.Toplam);
            

            var AllUsers = from e in bagislar
                           select new
                           {
                               resim_adi = e.Bagislar.resimadi,
                               bagis_aciklama = e.Bagislar.Baslik,
                               aciklama = e.Aciklama,
                               bagisTutarSayisi = sepetBagisTutar,
                               bagisUlasimSayisi = e.BagisUlasimSayisi,
                               bagisAdetMiktari = e.BagisAdetMiktari,
                               id = e.ID,
                               bagisID = e.BagisID,
                           };

            return Json(AllUsers);
        }
    }
}