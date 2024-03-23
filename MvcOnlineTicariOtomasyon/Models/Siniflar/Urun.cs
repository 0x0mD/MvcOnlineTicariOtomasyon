using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcOnlineTicariOtomasyon.Models.Siniflar
{
    public class Urun
    {
        [Key]
        public int Urunid { get; set; }

        [Column(TypeName ="Varchar")]
        [StringLength(30, ErrorMessage = "En fazla 30 karakter yazabilirsin!")]
        [Required(ErrorMessage = "Bu alanı boş geçemezsin!")]
        public string UrunAd { get; set; }

        [Column(TypeName = "Varchar")]
        [StringLength(30, ErrorMessage = "En fazla 30 karakter yazabilirsin!")]
        [Required(ErrorMessage = "Bu alanı boş geçemezsin!")]
        public string Marka { get; set;}
        [Required(ErrorMessage = "Bu alanı boş geçemezsin!")]
        public short Stok { get; set; }
        [Required(ErrorMessage = "Bu alanı boş geçemezsin!")]
        public decimal AlisFiyat { get; set; }
        [Required(ErrorMessage = "Bu alanı boş geçemezsin!")]
        public decimal SatisFiyat { get; set; }
        public bool Durum { get; set; }

        [Column(TypeName = "Varchar")]
        [StringLength(250)]
        public string UrunGorsel { get; set; }
        public int Kategoriid { get; set; }
        public virtual Kategori Kategori { get; set; }

        public ICollection<SatisHareket> SatisHareketS { get; set; }
    }
}