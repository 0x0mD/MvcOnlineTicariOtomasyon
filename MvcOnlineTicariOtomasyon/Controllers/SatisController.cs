using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcOnlineTicariOtomasyon.Models.Siniflar;

namespace MvcOnlineTicariOtomasyon.Controllers
{
    public class SatisController : Controller
    {
        Context db = new Context();
        public ActionResult Index()
        {
            var data = db.SatisHarekets.ToList();
            return View(data);
        }
        [HttpGet]
        private List<SelectListItem> GetUrunListesi(int id)
        {
            if (id == 0)
            {
                var urun_data = (from x in db.Uruns.ToList()
                                 select new SelectListItem
                                 {
                                     Text = x.UrunAd,
                                     Value = x.Urunid.ToString()
                                 }).ToList();
                return urun_data;
            }
            else
            {
                var urun_data = (from x in db.Uruns.ToList()
                                 select new SelectListItem
                                 {
                                     Text = x.UrunAd,
                                     Value = x.Urunid.ToString(),
                                     Selected = x.Urunid == id
                                 }).ToList();
                return urun_data;
            }
        }
        private List<SelectListItem> GetCariListesi(int id)
        {
            if (id == 0)
            {
                var cari_data = (from x in db.Carilers.ToList()
                                 select new SelectListItem
                                 {
                                     Text = x.CariAd + " " + x.CariSoyad,
                                     Value = x.Cariid.ToString()
                                 }).ToList();
                return cari_data;
            }
            else
            {
                var cari_data = (from x in db.Carilers.ToList()
                                 select new SelectListItem
                                 {
                                     Text = x.CariAd + " " + x.CariSoyad,
                                     Value = x.Cariid.ToString(),
                                     Selected = x.Cariid == id
                                 }).ToList();
                return cari_data;
            }
        }
        private List<SelectListItem> GetPersonelListesi(int id)
        {
            if (id == 0)
            {
                var personel_data = (from x in db.Personels.ToList()
                                     select new SelectListItem
                                     {
                                         Text = x.PersonelAd + " " + x.PersonelSoyad,
                                         Value = x.Personelid.ToString()
                                     }).ToList();
                return personel_data;
            }
            else
            {
                var personel_data = (from x in db.Personels.ToList()
                                     select new SelectListItem
                                     {
                                         Text = x.PersonelAd + " " + x.PersonelSoyad,
                                         Value = x.Personelid.ToString(),
                                         Selected = x.Personelid == id
                                     }).ToList();
                return personel_data;
            }
        }
        public ActionResult YeniSatis()
        {
            ViewBag.urun_data_ekle = GetUrunListesi(0);
            ViewBag.cari_data_ekle = GetCariListesi(0);
            ViewBag.personel_data_ekle = GetPersonelListesi(0);
            return View();
        }
        [HttpPost]
        public ActionResult YeniSatis(SatisHareket satis)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("YeniSatis");
                }

                string formatTarih = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                DateTime tarih;

                if (DateTime.TryParseExact(formatTarih, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out tarih))
                {
                    satis.Tarih = tarih;
                }
                else
                {
                    satis.Tarih = DateTime.Parse(DateTime.Now.ToShortDateString());
                }

                satis.Durum = true;
                db.SatisHarekets.Add(satis);
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
                return View("YeniSatis"); // Doğrulama hatası varsa aynı sayfaya geri dönme
            }
        }

        public ActionResult SatisSil(int id)
        {
            try
            {
                var sil = db.SatisHarekets.Find(id);
                if (sil == null)
                {
                    ModelState.AddModelError("", "Silme işlemi sırasında bir hata oluştu.");
                    System.Diagnostics.Debug.WriteLine("Silme işlemi sırasında bir hata oluştu.");
                    return RedirectToAction("Index");
                }

                sil.Durum = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Silme işlemi sırasında bir hata oluştu: " + ex.Message);
                ModelState.AddModelError("", "Silme işlemi sırasında bir hata oluştu: " + ex.Message);
                return View("Index");
            }
        }

        public ActionResult SatisGetir(int id)
        {
            List<SelectListItem> urun_data = GetUrunListesi(id);
            List<SelectListItem> cari_data = GetCariListesi(id);
            List<SelectListItem> personel_data = GetPersonelListesi(id);
            ViewBag.urun_data_guncelle = urun_data;
            ViewBag.cari_data_guncelle = cari_data;
            ViewBag.personel_data_guncelle = personel_data;

            var data = db.SatisHarekets.Find(id);
            return View("SatisGetir", data);
        }
        public ActionResult SatisGuncelle(SatisHareket satis)
        {
            if (satis == null)
            {
                return HttpNotFound();
            }

            if (ViewBag.urun_data_guncelle == null)
            {
                List<SelectListItem> urun_data = GetUrunListesi(satis.Urunid);
                ViewBag.urun_data_guncelle = urun_data;
            }
            if (ViewBag.cari_data_guncelle == null)
            {
                List<SelectListItem> cari_data = GetCariListesi(satis.Cariid);
                ViewBag.cari_data_guncelle = cari_data;
            }
            if (ViewBag.personel_data_guncelle == null)
            {
                List<SelectListItem> personel_data = GetPersonelListesi(satis.Personelid);
                ViewBag.personel_data_guncelle = personel_data;
            }

            if (!ModelState.IsValid)
            {
                return View("SatisGetir", satis);
            }

            try
            {
                var satis_id = db.SatisHarekets.Find(satis.Satisid);
                if (satis_id == null || satis_id.Satisid == 0)
                {
                    System.Diagnostics.Debug.WriteLine("Güncelleme işlemi sırasında bir hata oluştu.");
                    return RedirectToAction("Index");
                }

                satis_id.Urunid = satis.Urunid;
                satis_id.Cariid = satis.Cariid;
                satis_id.Personelid = satis.Personelid;
                satis_id.Adet = satis.Adet;
                satis_id.Fiyat = satis.Fiyat;
                satis_id.ToplamTutar = satis.ToplamTutar;

                string formatTarih = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                DateTime tarih;

                if (DateTime.TryParseExact(formatTarih, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out tarih))
                {
                    satis_id.Tarih = tarih;
                }
                else
                {
                    satis_id.Tarih = DateTime.Parse(DateTime.Now.ToShortDateString());
                }

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
                return View("SatisGetir", satis);
            }
        }
        public ActionResult SatisDetay(int id)
        {
            var data = db.SatisHarekets.Where(x => x.Satisid == id).ToList();
            return View(data);
        }
    }
}