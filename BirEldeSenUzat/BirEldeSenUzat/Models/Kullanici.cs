namespace BirEldeSenUzat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Kullanici")]
    public partial class Kullanici
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Kullanici()
        {
            Bagislars = new HashSet<Bagislar>();
            KullaniciRols = new HashSet<KullaniciRol>();
            Sepets = new HashSet<Sepet>();
        }

        public int KullaniciID { get; set; }

        [StringLength(100)]
        public string AdSoyad { get; set; }

        [StringLength(50)]
        public string TelNo { get; set; }

        [StringLength(50)]
        public string Mail { get; set; }

        [StringLength(100)]
        public string Parola { get; set; }

        public DateTime? KayitTarihi { get; set; }

        public bool? OnaylandiMi { get; set; }

        [StringLength(10)]
        public string Cinsiyet { get; set; }

        [StringLength(11)]
        public string TCNo { get; set; }

        public DateTime? DogumTarihi { get; set; }

        [StringLength(150)]
        public string AdresDetay { get; set; }

        public int? SehirId { get; set; }

        public int? ilceId { get; set; }

        public int? SemtMahId { get; set; }
        public bool Status { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Bagislar> Bagislars { get; set; }

        public virtual Ilceler Ilceler { get; set; }

        public virtual Sehirler Sehirler { get; set; }

        public virtual SemtMah SemtMah { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KullaniciRol> KullaniciRols { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sepet> Sepets { get; set; }
    }
}
