using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using MiniMuhasebeci.Models;

namespace MiniMuhasebeci.Controllers
{
    public class PDFController : Controller
    {
        DasProjeDBEntities db = new DasProjeDBEntities();

        public ActionResult Index(int? id)
        {
            if (id != null)
            {
                var degerler = db.tblCariHareketleri.Where(x => x.Id == id).ToList();
                ViewBag.cariId = (db.tblCariHareketleri.FirstOrDefault(x => x.Id == id)).CariId;
                return View(degerler); 
            }
            return HttpNotFound();
        }

    }
}