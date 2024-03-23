using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcOnlineTicariOtomasyon.Models.Siniflar;

namespace MvcOnlineTicariOtomasyon.Controllers
{
    public class CariController : Controller
    {
        Context db = new Context();
        public ActionResult Index()
        {
            var data = db.Carilers.Where(x=>x.Durum==true).ToList();
            return View(data);
        }

        [HttpGet]
        public ActionResult CariEkle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CariEkle(Cariler cariler)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("CariEkle");
                }

                cariler.Durum= true;
                db.Carilers.Add(cariler);
                db.SaveChanges();
                return RedirectToAction("Index"); //Kaydet ana indexe dön
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                    }
                }
                return View("CariEkle"); // Doğrulama hatası varsa aynı sayfaya geri dönme
            }
        }
        public ActionResult CariSil(int id)
        {
            try
            {
                var sil = db.Carilers.Find(id);
                if (sil == null)
                {
                    ModelState.AddModelError("", "Silme işlemi sırasında bir hata oluştu.");
                    System.Diagnostics.Debug.WriteLine("Silme işlemi sırasında bir hata oluştu.");
                    return RedirectToAction("Index"); // Başka bir sayfaya yönlendirme
                }

                //db.Carilers.Remove(sil);
                sil.Durum = false;
                db.SaveChanges();
                return RedirectToAction("Index"); // Başka bir sayfaya yönlendirme
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Silme işlemi sırasında bir hata oluştu: " + ex.Message);
                ModelState.AddModelError("", "Silme işlemi sırasında bir hata oluştu: " + ex.Message);
                return View("Index"); 
            }
        }
        public ActionResult CariGetir(int id)
        {
            var data = db.Carilers.Find(id);
            return View("CariGetir", data);
        }
        public ActionResult CariGuncelle(Cariler cariler)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("CariGetir");
                }

                var cari_id = db.Carilers.Find(cariler.Cariid);
                if (cari_id == null)
                {
                    System.Diagnostics.Debug.WriteLine("Güncelleme işlemi sırasında bir hata oluştu.");
                    return RedirectToAction("Index");
                }

                cari_id.CariAd = cariler.CariAd;
                cari_id.CariSoyad = cariler.CariSoyad;
                cari_id.CariSehir = cariler.CariSehir;
                cari_id.CariMail = cariler.CariMail;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                    }
                }
                return View("CariGetir");
            }
        }
        public ActionResult MusteriSatis(int id)
        {
            var data = db.SatisHarekets.Where(x => x.Cariid == id).ToList();
            var databag = db.Carilers.Where(a => a.Cariid == id).Select(y => y.CariAd + " " + y.CariSoyad).FirstOrDefault();
            ViewBag.cariinfo = databag;
            return View(data);
        }
    }
}