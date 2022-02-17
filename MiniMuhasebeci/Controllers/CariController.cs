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
    public class CariController : Controller
    {
        //private DasProjeDBEntities1 db = new DasProjeDBEntities1();

        //DasProjeDbEntities db = new DasProjeDbEntities();

        DasProjeDBEntities db = new DasProjeDBEntities();

        public ActionResult AnaSayfa()
        {
            return View();
        }



        //Cari listeleme viewine veri göndermeyi sağlar. Listeleme viewine bağlı
        //db veri tabanı nesnesi. veritabanındaki cari tablosuna bağlanıyor ve verileri tolist ile listeliyor.
        public ActionResult Index(string Cari)
        {
            //if (!string.IsNullOrEmpty(Cari))
            //{
            //    return View(db.tblCari.Where(x => x.Ad.ToLower() == Cari.ToLower() || x.Soyad.ToLower() == Cari.ToLower()).ToList());
            //}
            
            List<tblCari> CariList = db.tblCari.ToList();
            ViewBag.cariList = CariList.Reverse<tblCari>().ToList();
            return View();
        }

        //Ekleme ekranına geçiş yapıyor. Veri gönderiyor ve veri alıyor. Parametre olarak tblCarinin nesnesini alıyor ve tablonun alacağı değerleri bu nesneye ekliyor.
        //if içerisinde kontrol yapılıyor. Doğrulama olursa yapılacak işlemler if içerisinde yazıyor. Gelen nesneyi cari tablosuna ekleyeceği ve veritabanında olan değişiklikleri kaydedeceği yazıyor. SaveChanges değişiklikleri
        //kaydeder. Ardından Index actionuna geçiş yapıyor.
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CariId,Unvan,Ad,Soyad,Adres,Telefon")] tblCari tblCari)
        {
            if (ModelState.IsValid)
            {
                db.tblCari.Add(tblCari);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblCari);
        }
        


        //Güncellemek için id parametresi alıyor. Soru işaretiyle boş olup olmadığını kontrol ediyor. Id null ise hata ekranına geçiyor. Değilse tblCari nesnesi oluşturuyor ve veritabanından o idli olan veriyi getiriyor.
        //Eğer ki nesne içi boşsa hata veriyor.
        //Parametre olarak tblCarinin nesnesini alıyor ve tablonun alacağı değerleri bu nesneye ekliyor. İf içerisinde onaylanırsa veriyi güncelleyeceğini yani modifie edeceğini belirtiyor.
        //Veritabanındaki değişiklikleri kaydediyor. Ve Index actionuna geçiyor.
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCari tblCari = db.tblCari.Find(id);
            if (tblCari == null)
            {
                return HttpNotFound();
            }
            return View(tblCari);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CariId,Unvan,Ad,Soyad,Adres,Telefon")] tblCari tblCari)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblCari).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblCari);
        }


        //Parametre olarak id alıyor ve boş mu diye kontrol ediyor. Id null ise hata ekranına geçiyor. Değilse tblCari nesnesi oluşturuyor ve veritabanından o idli olan veriyi getiriyor.
        //Eğer ki nesne içi boşsa hata veriyor.
        //Silmek onaylanırsa nesneye veriyi bulup getiriyor. Ve veritabanına bağlanıp tablodan siliyor. Sonrada veritabanını kaydedip Index actionuna geçiyor.
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCari tblCari = db.tblCari.Find(id);
            if (tblCari == null)
            {
                return HttpNotFound();
            }
            return View(tblCari);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblCari tblCari = db.tblCari.Find(id);
            db.tblCari.Remove(tblCari);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
