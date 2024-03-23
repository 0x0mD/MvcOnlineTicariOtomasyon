using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcOnlineTicariOtomasyon.Models.Siniflar;

namespace MvcOnlineTicariOtomasyon.Controllers
{
    public class DepartmanController : Controller
    {
        Context db = new Context();
        public ActionResult Index()
        {
            var data = db.Departmans.Where(x => x.Durum == true).ToList();
            return View(data);
        }
        [HttpGet]
        public ActionResult DepartmanEkle()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DepartmanEkle(Departman departman)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("DepartmanEkle");
                }

                if (string.IsNullOrEmpty(departman.DepartmanAd) || departman.DepartmanAd.Length < 3 || !ModelState.IsValid)
                {
                    ModelState.AddModelError("", "Kategori adı en az 3 karakter olmalıdır.");
                    return View("DepartmanEkle");
                }

                departman.Durum = true;
                db.Departmans.Add(departman);
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
                return View("DepartmanEkle");
            }

        }
        public ActionResult DepartmanSil(int id)
        {
            try
            {
                var sil = db.Departmans.Find(id);
                if (sil == null)
                {
                    ModelState.AddModelError("", "Silme işlemi sırasında bir hata oluştu.");
                    System.Diagnostics.Debug.WriteLine("Silme işlemi sırasında bir hata oluştu.");
                    return RedirectToAction("Index"); // Başka bir sayfaya yönlendirme
                }

                //db.Departmans.Remove(sil);
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
        public ActionResult DepartmanGetir(int id)
        {
            var data = db.Departmans.Find(id);
            return View("DepartmanGetir", data);
        }
        public ActionResult DepartmanGuncelle(Departman departman)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("DepartmanGetir");
                }

                var departman_id = db.Departmans.Find(departman.Departmanid);
                if (departman_id == null)
                {
                    System.Diagnostics.Debug.WriteLine("Güncelleme işlemi sırasında bir hata oluştu.");
                    return RedirectToAction("Index");
                }

                departman_id.DepartmanAd = departman.DepartmanAd;
                departman_id.Durum = departman.Durum;
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
                return View("DepartmanGetir");
            }
        }
        public ActionResult DepartmanDetay(int id)
        {
            var detay = db.Personels.Where(x => x.Departmanid == id).ToList();
            var dpt = db.Departmans.Where(x => x.Departmanid == id).Select(y => y.DepartmanAd).FirstOrDefault();
            ViewBag.departmanadi = dpt;
            return View(detay);
        }
        public ActionResult DepartmanPersonelSatis(int id)
        {
            var data = db.SatisHarekets.Where(x => x.Personelid == id).ToList();
            var databag = db.Personels.Where(a => a.Personelid == id).Select(y => y.PersonelAd + " " + y.PersonelSoyad).FirstOrDefault();
            ViewBag.personelinfo = databag;
            return View(data);
        }
    }
}