namespace BirEldeSenUzat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SemtMah")]
    public partial class SemtMah
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SemtMah()
        {
            Kullanicis = new HashSet<Kullanici>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SemtMahId { get; set; }

        [Required]
        [StringLength(100)]
        public string MahalleAdi { get; set; }

        public int ilceId { get; set; }

        public virtual Ilceler Ilceler { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Kullanici> Kullanicis { get; set; }
    }
}
