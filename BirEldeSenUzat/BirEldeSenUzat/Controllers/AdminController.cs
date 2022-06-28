using BirEldeSenUzat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BirEldeSenUzat.Controllers
{
    public class AdminController : Controller
    {
        IhtiyacDB context = new IhtiyacDB();
        // GET: Admin
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var kategoriSayisi = context.Kategoris.ToList().Count();
            var bagisSayisi = context.Bagislars.ToList().Count();
            var kullaniciSayisi = context.Kullanicis.ToList().Count();

            ViewBag.kategoriSayisi = kategoriSayisi;
            ViewBag.bagisSayisi = bagisSayisi;
            ViewBag.kullaniciSayisi = kullaniciSayisi;

            return View();
        }   
    }
}