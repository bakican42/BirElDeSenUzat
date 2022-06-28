namespace BirEldeSenUzat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UlasanElleriniz")]
    public partial class UlasanElleriniz
    {
        public int ID { get; set; }

        public int? BagisID { get; set; }

        public string BagisUlasimSayisi { get; set; }

        [StringLength(250)]
        public string Aciklama { get; set; }

        public virtual Bagislar Bagislar { get; set; }
        public bool OnaylandiMi { get; set; }
        public int? BagisAdetMiktari { get; set; }
    }
}
