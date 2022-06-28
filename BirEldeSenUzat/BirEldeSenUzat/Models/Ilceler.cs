namespace BirEldeSenUzat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Ilceler")]
    public partial class Ilceler
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ilceler()
        {
            Kullanicis = new HashSet<Kullanici>();
            SemtMahs = new HashSet<SemtMah>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ilceId { get; set; }

        public int SehirId { get; set; }

        [Required]
        [StringLength(60)]
        public string IlceAdi { get; set; }

        public virtual Sehirler Sehirler { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Kullanici> Kullanicis { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SemtMah> SemtMahs { get; set; }
    }
}
