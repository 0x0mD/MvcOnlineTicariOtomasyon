using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcOnlineTicariOtomasyon.Models.Siniflar
{
    public class Cariler
    {
        [Key]
        public int Cariid { get; set; }

        [Column(TypeName = "Varchar")]
        [StringLength(30,ErrorMessage ="En fazla 30 karakter yazabilirsin!")]
        [Required(ErrorMessage ="Bu alanı boş geçemezsin!")]
        public string CariAd { get; set; }

        [Column(TypeName = "Varchar")]
        [StringLength(30, ErrorMessage = "En fazla 30 karakter yazabilirsin!")]
        [Required(ErrorMessage = "Bu alanı boş geçemezsin!")]
        public string CariSoyad { get; set; }

        [Column(TypeName = "Varchar")]
        [StringLength(20, ErrorMessage = "En fazla 20 karakter yazabilirsin!")]
        public string CariSehir { get; set; }

        [Column(TypeName = "Varchar")]
        [StringLength(50, ErrorMessage = "En fazla 50 karakter yazabilirsin!")]
        public string CariMail { get; set; }
        public bool Durum { get; set; }
        public ICollection<SatisHareket> SatisHareketS { get; set; }
    }
}