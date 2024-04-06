using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcOnlineTicariOtomasyon.Models.Siniflar;

namespace MvcOnlineTicariOtomasyon.Controllers
{
    public class UrunController : Controller
    {
        Context db = new Context();
        public ActionResult Index()
        {
            var data = db.Uruns.Where(x=>x.Durum == true).ToList();
            return View(data);
        }

        //Get istek türü
        [HttpGet]
        private List<SelectListItem> GetKategoriListesi()
        {
            var kategori_data = (from x in db.Kategoris.ToList()
                                 select new SelectListItem
                                 {
                                     Text = x.KategoriName,
                                     Value = x.KategoriID.ToString()
                                 }).ToList();
            return kategori_data;
        }
        private List<SelectListItem> GetKategoriData(int selectedCategoryId)
        {
            return (from x in db.Kategoris.ToList()
                    select new SelectListItem
                    {
                        Text = x.KategoriName,
                        Value = x.KategoriID.ToString(),
                        Selected = x.KategoriID == selectedCategoryId // Seçili kategoriyi
                    }).ToList();
        }
        public ActionResult UrunEkle()
        {
            ViewBag.kategori_data_ekle = GetKategoriListesi();
            return View();
        }

        //Post istek türü
        [HttpPost]
        public ActionResult UrunEkle(Urun urun)
        {
            if (ViewBag.kategori_data_ekle == null)
            {
                ViewBag.kategori_data_ekle = GetKategoriListesi(); // Kategori listesi yüklü değilse tekrar yükle
            }

            try
            {
                if (!ModelState.IsValid)
                {
                    return View("UrunEkle", urun);
                }

                urun.Durum = true;
                db.Uruns.Add(urun);
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
                return View("UrunEkle", urun);
            }
        }
        public ActionResult UrunSil(int id)
        {
            try
            {
                var sil = db.Uruns.Find(id);
                if (sil == null)
                {
                    ModelState.AddModelError("", "Silme işlemi sırasında bir hata oluştu.");
                    System.Diagnostics.Debug.WriteLine("Silme işlemi sırasında bir hata oluştu.");
                    return RedirectToAction("Index");
                }

                //db.Uruns.Remove(sil); //silmeyip pasife alalım
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

        public ActionResult UrunGetir(int id)
        {
            List<SelectListItem> kategori_data = GetKategoriData(id);
            ViewBag.kategori_data_guncelle = kategori_data;

            var data = db.Uruns.Find(id);
            return View("UrunGetir", data);
        }
        public ActionResult UrunGuncelle(Urun urun)
        {
            if (urun == null)
            {
                return HttpNotFound();
            }

            if (ViewBag.kategori_data_guncelle == null)
            {
                List<SelectListItem> kategori_data = GetKategoriData(urun.Kategoriid);
                ViewBag.kategori_data_guncelle = kategori_data;
            }

            if (!ModelState.IsValid)
            {
                return View("UrunGetir", urun);
            }

            try
            {
                var urun_id = db.Uruns.Find(urun.Urunid);
                if (urun_id == null || urun_id.Urunid == 0)
                {
                    System.Diagnostics.Debug.WriteLine("Güncelleme işlemi sırasında bir hata oluştu.");
                    return RedirectToAction("Index");
                }

                urun_id.UrunAd = urun.UrunAd;
                urun_id.Marka = urun.Marka;
                urun_id.Stok = urun.Stok;
                urun_id.AlisFiyat = urun.AlisFiyat;
                urun_id.SatisFiyat = urun.SatisFiyat;
                urun_id.Kategoriid = urun.Kategoriid;
                urun_id.UrunGorsel = urun.UrunGorsel;
                urun_id.Durum = urun.Durum;
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
                return View("UrunGetir", urun);
            }
        }
        public ActionResult UrunListesi()
        {
            var data = db.Uruns.ToList();
            return View(data);
        }
    }
}