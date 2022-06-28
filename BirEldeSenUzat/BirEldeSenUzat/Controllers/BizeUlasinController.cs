using BirEldeSenUzat.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BirEldeSenUzat.Controllers
{
    public class BizeUlasinController : Controller
    {
        IhtiyacDB context = new IhtiyacDB();
        // GET: BizeUlasin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BizeUlas(int sayfa = 1)
        {
            var data = context.Iletisims.OrderByDescending(x => x.Tarih).ToList().ToPagedList(sayfa, 10);

            return View(data);
        }
        
        public ActionResult BizeUlasDetay(int? id)
        {
            var data = context.Iletisims.Where(x => x.ID == id).FirstOrDefault();

            return View(data);
        }

        public void MesajSil(int id)
        {
            var silinicekMesaj = context.Iletisims.Where(x => x.ID == id).FirstOrDefault();
            context.Entry(silinicekMesaj).State = System.Data.Entity.EntityState.Deleted;
            context.SaveChanges();
        }



    }
}