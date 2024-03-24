using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcOnlineTicariOtomasyon.Models.Siniflar
{
    public class Personel
    {
        [Key]
        public int Personelid { get; set; }

        [Column(TypeName = "Varchar")]
        [StringLength(30, ErrorMessage = "En fazla 30 karakter yazabilirsin!")]
        [Required(ErrorMessage = "Bu alanı boş geçemezsin!")]
        public string PersonelAd { get; set; }

        [Column(TypeName = "Varchar")]
        [StringLength(30, ErrorMessage = "En fazla 30 karakter yazabilirsin!")]
        [Required(ErrorMessage = "Bu alanı boş geçemezsin!")]
        public string PersonelSoyad { get; set; }

        [Column(TypeName = "Varchar")]
        [StringLength(250)]
        public string PersonelGorsel { get; set; }
        public ICollection<SatisHareket> SatisHareketS { get; set; }
        public int Departmanid { get; set; }
        public virtual Departman Departman { get; set; }
        public bool Durum {  get; set; }
    }
}