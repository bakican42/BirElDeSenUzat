namespace BirEldeSenUzat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Iletisim")]
    public partial class Iletisim
    {
        public int ID { get; set; }

        [StringLength(100)]
        public string AdSoyad { get; set; }

        [StringLength(70)]
        public string Konu { get; set; }

        [StringLength(50)]
        public string MailAdresi { get; set; }

        public string MesajIcerigi { get; set; }
        public DateTime? Tarih { get; set; }
    }
}
