using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcOnlineTicariOtomasyon.Models.Siniflar;

namespace MvcOnlineTicariOtomasyon.Controllers
{
    public class KategoriController : Controller
    {
        Context db = new Context();

        public ActionResult Index()
        {
            var data = db.Kategoris.Where(x => x.Durum == true).ToList();
            return View(data);
        }
        [HttpGet]
        public ActionResult KategoriEkle()
        {
            return View();
        }

        [HttpPost]
        public ActionResult KategoriEkle(Kategori kategori)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("KategoriEkle");
                }

                kategori.Durum = true;
                db.Kategoris.Add(kategori);
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
                return View("KategoriEkle");
            }
        }
        public ActionResult KategoriSil(int id)
        {
            try
            {
                var sil = db.Kategoris.Find(id);
                if (sil == null)
                {
                    ModelState.AddModelError("", "Silme işlemi sırasında bir hata oluştu.");
                    System.Diagnostics.Debug.WriteLine("Silme işlemi sırasında bir hata oluştu.");
                    // Silmeye çalışılan öğe bulunamadı, burada kullanıcıya bir hata mesajı gösterir
                    return RedirectToAction("Index"); // Başka bir sayfaya yönlendirme
                }

                //db.Kategoris.Remove(sil);
                sil.Durum = false;
                db.SaveChanges();
                return RedirectToAction("Index"); // Başka bir sayfaya yönlendirme
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Silme işlemi sırasında bir hata oluştu: " + ex.Message);
                ModelState.AddModelError("", "Silme işlemi sırasında bir hata oluştu: " + ex.Message);
                return View("Index"); // Hata olduğunda aynı sayfaya geri dönme ve hata mesajını gösterme
            }
        }
        public ActionResult KategoriGetir(int id)
        {
            var kategori = db.Kategoris.Find(id);
            return View("KategoriGetir", kategori);
        }
        public ActionResult KategoriGuncelle(Kategori kategori)
        {
            if (!ModelState.IsValid)
            {
                return View("KategoriGetir");
            }
            var kategori_id = db.Kategoris.Find(kategori.KategoriID);
            try
            {
                if (kategori_id == null || kategori_id.KategoriID == 0)
                {

                    System.Diagnostics.Debug.WriteLine("Güncelleme işlemi sırasında bir hata oluştu.");
                    return RedirectToAction("Index");
                }
                // KategoriName alanının uzunluğunu kontrol edin
                if (string.IsNullOrEmpty(kategori.KategoriName) || kategori.KategoriName.Length < 3 || !ModelState.IsValid)
                {
                    // KategoriName alanı uygun uzunlukta değilse, uygun bir hata mesajı ekleyin
                    ModelState.AddModelError("", "Kategori adı en az 3 karakter olmalıdır.");
                    return View("KategoriGetir", kategori_id);
                }

                kategori_id.KategoriName = kategori.KategoriName;
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
                return View("KategoriGetir", kategori_id); // Doğrulama hatası varsa aynı sayfaya geri dönme
            }
        }
    }
}