namespace BirEldeSenUzat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Bagislar")]
    public partial class Bagislar
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Bagislar()
        {
            Sepets = new HashSet<Sepet>();
            UlasanEllerinizs = new HashSet<UlasanElleriniz>();
        }

        public int id { get; set; }

        [StringLength(100)]
        public string resimadi { get; set; }

        [StringLength(200)]
        public string aciklama { get; set; }

        public int? KullaniciID { get; set; }

        public int? KategoriID { get; set; }

        [StringLength(100)]
        public string Baslik { get; set; }
        public bool Status { get; set; }

        public DateTime? OlusturulmaTarihi { get; set; }

        public virtual Kategori Kategori { get; set; }

        public virtual Kullanici Kullanici { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sepet> Sepets { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UlasanElleriniz> UlasanEllerinizs { get; set; }
    }
}
