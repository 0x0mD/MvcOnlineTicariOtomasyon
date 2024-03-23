using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcOnlineTicariOtomasyon.Models.Siniflar
{
    public class Admin
    {
        [Key]
        public int Adminid { get; set; }

        [Column(TypeName = "Varchar")]
        [StringLength(10, ErrorMessage = "En fazla 10 karakter yazabilirsin!")]
        [Required(ErrorMessage = "Bu alanı boş geçemezsin!")]
        public string KullaniciAd { get; set; }

        [Column(TypeName = "Varchar")]
        [StringLength(20, ErrorMessage = "En fazla 20 karakter yazabilirsin!")]
        [Required(ErrorMessage = "Bu alanı boş geçemezsin!")]
        public string Sifre { get; set; }

        [Column(TypeName = "Char")]
        [StringLength(1)]
        [Required(ErrorMessage = "Bu alanı boş geçemezsin!")]
        public string Yetki { get; set; }
    }
}