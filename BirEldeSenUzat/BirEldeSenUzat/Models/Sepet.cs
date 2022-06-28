namespace BirEldeSenUzat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Sepet")]
    public partial class Sepet
    {
        public int Id { get; set; }

        public int? BagisId { get; set; }

        public int? KullaniciId { get; set; }

        public int? Miktar { get; set; }

        public int? Toplam { get; set; }

        public int? UrunSayisi { get; set; }

        public bool? OdemeTamamlandiMi { get; set; }

        public DateTime? Tarih { get; set; }

        public virtual Bagislar Bagislar { get; set; }

        public virtual Kullanici Kullanici { get; set; }
    }
}
