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

    public class CariHareketleriController : Controller
    {

        //private DasProjeDBEntities1 db = new DasProjeDBEntities1();

        //DasProjeDbEntities db = new DasProjeDbEntities();

        DasProjeDBEntities db = new DasProjeDBEntities();

        //Cari listeleme viewine veri göndermeyi sağlar. Listeleme viewine bağlı. tblCariHareketleri olarak nesne oluşturup veritabanına bağlanıyor. Oradan carihareketleri tablosuna ve verileri çekiyor.
        //Verileri çekerken Include metodu ile carideki  uyuşan verilerle çekiyor. viewe bu nesneyi list olarak gönderiyor.

        public ActionResult Index(string p)
        {
            //if (!string.IsNullOrEmpty(CariUnvan))
            //{
            //    return View(db.tblCariHareketleri.Where(x => x.tblCari.Unvan.ToLower() == CariUnvan.ToLower()).ToList());
            //}
            List<tblCariHareketleri> CariHareketleriList = db.tblCariHareketleri.ToList();
            ViewBag.cariHareketleriList = CariHareketleriList.Reverse<tblCariHareketleri>().ToList();

            return View();
        }

        //List oluşturuyor ve liste veri eklemeleri yapıyor. Viewbag ile viewe gönderiyor. Cariyi de Id ve unvanla gönderiyor. Bunlar dropdownlist de gözükmesi için yapılıyor.
        //Nesne oluşturup nesneye tablodaki kolonlar ekleniyor. Model doğrulanırsa eğer nesne veritabanına ekleniyor ve veritabanı kaydediliyor. Ardından Index actionuna gidiliyor.
        public ActionResult Create()
        {
            List<string> IslemTipleri = new List<string>();
            IslemTipleri.Add("Nakit"); IslemTipleri.Add("Kredi"); IslemTipleri.Add("Masraf"); IslemTipleri.Add("Tahakkuk");

            List<string> IslemTurleri = new List<string>();
            IslemTurleri.Add("Tahsilat"); IslemTurleri.Add("Ödeme");

            ViewBag.CariId = new SelectList(db.tblCari, "CariId", "Unvan");
            ViewBag.IslemTipi = new SelectList(IslemTipleri);
            ViewBag.IslemTuru = new SelectList(IslemTurleri);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CariId,Tarih,IslemTipi,IslemTuru,Borc,Alacak,Aciklama")] tblCariHareketleri tblCariHareketleri)
        {
            if (ModelState.IsValid)
            {
                db.tblCariHareketleri.Add(tblCariHareketleri);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            List<string> IslemTipleri = new List<string>();
            IslemTipleri.Add("Nakit"); IslemTipleri.Add("Kredi"); IslemTipleri.Add("Masraf"); IslemTipleri.Add("Tahakkuk");

            List<string> IslemTurleri = new List<string>();
            IslemTurleri.Add("Ödeme"); IslemTurleri.Add("Tahsilat");
            ViewBag.CariId = new SelectList(db.tblCari, "CariId", "Unvan", tblCariHareketleri.CariId);
            ViewBag.IslemTipi = new SelectList(IslemTipleri);
            ViewBag.IslemTuru = new SelectList(IslemTurleri);
            tblCariHareketleri.IslemTipi = null;
            tblCariHareketleri.IslemTuru = null;
            return View(tblCariHareketleri);
        }


        //Burada id parametresi alınıyor boş mu diye kontrol edilıyor. boşsa hata veriyor. değilse neyse oluşturuluyor ve nesneye veritabanından o idli değer aranıp getiriliyor.
        //O ide ait değer bulunamazsa null değeri döner. nullsa hata veriyor. değilse viewe gönderiliyor nesne.
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCariHareketleri tblCariHareketleri = db.tblCariHareketleri.Find(id);
            if (tblCariHareketleri == null)
            {
                return HttpNotFound();
            }
            ViewBag.cariHareketId = id;
            return View(tblCariHareketleri);
        }



        //Güncellemede id parametresi alınıyor. id boş ise hata veriyor. değilse nesne oluşturuluyor ve ide sahip veri nesneye atanıyor. bulamazsa veriyi nesne null olur. null ise hata veriyor.
        //null değilse cari id ve unvanı viewbag ile viewe gönderiliyor.
        //altta carihareketi nesnesi oluşturuluyor ve nesneye kolonlar ekleniyor. model eğer doğrulanırsa güncelleme yapılıyor ve veritabanı kaydediliyor. sonrada ındex actionuna geçiliyor.
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCariHareketleri tblCariHareketleri = db.tblCariHareketleri.Find(id);

            if (tblCariHareketleri == null)
            {
                return HttpNotFound();
            }

            List<string> IslemTipleri = new List<string>();
            List<string> IslemTurleri = new List<string>();

            #region Sorgular
            if (tblCariHareketleri.IslemTipi == "Nakit")
            {
                IslemTipleri.Add("Nakit"); IslemTipleri.Add("Kredi"); IslemTipleri.Add("Masraf"); IslemTipleri.Add("Tahakkuk");
            }
            else if (tblCariHareketleri.IslemTipi == "Kredi")
            {
                IslemTipleri.Add("Kredi"); IslemTipleri.Add("Nakit"); IslemTipleri.Add("Masraf"); IslemTipleri.Add("Tahakkuk");
            }
            else if (tblCariHareketleri.IslemTipi == "Masraf")
            {
                IslemTipleri.Add("Masraf"); IslemTipleri.Add("Kredi"); IslemTipleri.Add("Nakit"); IslemTipleri.Add("Tahakkuk");
            }
            else if (tblCariHareketleri.IslemTipi == "Tahakkuk")
            {
                IslemTipleri.Add("Tahakkuk"); IslemTipleri.Add("Masraf"); IslemTipleri.Add("Kredi"); IslemTipleri.Add("Nakit");
            }

            if (tblCariHareketleri.IslemTuru == "Ödeme")
            {
                IslemTurleri.Add("Ödeme"); IslemTurleri.Add("Tahsilat");
            }
            else if (tblCariHareketleri.IslemTuru == "Tahsilat")
            {
                IslemTurleri.Add("Tahsilat"); IslemTurleri.Add("Ödeme");
            }
            #endregion

            ViewBag.CariId = new SelectList(db.tblCari, "CariId", "Unvan", tblCariHareketleri.CariId);
            ViewBag.IslemTipi = new SelectList(IslemTipleri);
            ViewBag.IslemTuru = new SelectList(IslemTurleri);
        

            return View(tblCariHareketleri);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CariId,Tarih,IslemTipi,IslemTuru,Borc,Alacak,Aciklama")] tblCariHareketleri tblCariHareketleri)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblCariHareketleri).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CariId = new SelectList(db.tblCari, "CariId", "Unvan", tblCariHareketleri.CariId);
            return View(tblCariHareketleri);
        }



        //İd parametresi alıyor boş mu diye kontrole diliyor boşsa hata veriyor. değilse nesne oluşturuluyor ve veritabanında ide sahip değer nesneye atanıyor. eğer veri bulunamazsa nesne boş olur ve hata verir.
        //boş değilse viewe nesne gönderilir.
        //Silme onayı olunca nesneye idli eleman veritabanından çekiliyor ve veritabanından remove ediliyor. sonrada veritabanı kaydediliyor. ardından ındex actionuna geçiş yapılıyor.
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCariHareketleri tblCariHareketleri = db.tblCariHareketleri.Find(id);
            if (tblCariHareketleri == null)
            {
                return HttpNotFound();
            }
            return View(tblCariHareketleri);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblCariHareketleri tblCariHareketleri = db.tblCariHareketleri.Find(id);
            db.tblCariHareketleri.Remove(tblCariHareketleri);
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
