using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using MvcOnlineTicariOtomasyon.Models.Siniflar;

namespace MvcOnlineTicariOtomasyon.Controllers
{
    public class PersonelController : Controller
    {
        Context db = new Context();
        public ActionResult Index()
        {
            var data = db.Personels.Where(x=>x.Durum==true).ToList();
            return View(data);
        }
        [HttpGet]
        private List<SelectListItem> GetDepartmanListesi()
        {
            var departman_data = (from x in db.Departmans.ToList()
                                 select new SelectListItem
                                 {
                                     Text = x.DepartmanAd,
                                     Value = x.Departmanid.ToString()
                                 }).ToList();
            return departman_data;
        }
        private List<SelectListItem> GetDepartmanData(int selectedDepartmanId)
        {
            return (from x in db.Departmans.ToList()
                    select new SelectListItem
                    {
                        Text = x.DepartmanAd,
                        Value = x.Departmanid.ToString(),
                        Selected = x.Departmanid == selectedDepartmanId // Seçili Departman
                    }).ToList();
        }
        public ActionResult PersonelEkle()
        {
            ViewBag.departman_data_ekle = GetDepartmanListesi();
            return View();
        }

        [HttpPost]
        public ActionResult PersonelEkle(Personel personel)
        {
            try
            {
                if (ViewBag.departman_data_ekle == null)
                    ViewBag.departman_data_ekle = GetDepartmanListesi(); // Departman listesi yüklü değilse tekrar yükle

                if (!ModelState.IsValid)
                    return View("PersonelEkle");

                personel.Durum = true;
                db.Personels.Add(personel);
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
                return View("PersonelEkle");
            }
        }
        public ActionResult PersonelSil(int id)
        {
            try
            {
                var sil = db.Personels.Find(id);
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
        public ActionResult PersonelGetir(int id)
        {
            List<SelectListItem> personel_departman_data = GetDepartmanData(id);
            ViewBag.personel_data_guncelle = personel_departman_data;

            var data = db.Personels.Find(id);
            return View("PersonelGetir", data);
        }
        public ActionResult PersonelGuncelle(Personel personel)
        {
            if (personel == null)
            {
                return HttpNotFound();
            }

            if (ViewBag.personel_data_guncelle == null)
            {
                List<SelectListItem> personel_departman_data = GetDepartmanData(personel.Personelid);
                ViewBag.personel_data_guncelle = personel_departman_data;
            }

            if (!ModelState.IsValid)
            {
                return View("PersonelGetir", personel);
            }

            try
            {
                var personel_id = db.Personels.Find(personel.Personelid);
                if (personel_id == null || personel_id.Personelid == 0)
                {
                    System.Diagnostics.Debug.WriteLine("Güncelleme işlemi sırasında bir hata oluştu.");
                    return RedirectToAction("Index");
                }

                personel_id.PersonelAd = personel.PersonelAd;
                personel_id.PersonelSoyad = personel.PersonelSoyad;
                personel_id.Departmanid = personel.Departmanid;
                personel_id.PersonelGorsel = personel.PersonelGorsel;
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
                return View("PersonelGetir", personel);
            }
        }
    }
}