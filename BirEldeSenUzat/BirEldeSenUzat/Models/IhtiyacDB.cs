using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace BirEldeSenUzat.Models
{
    public partial class IhtiyacDB : DbContext
    {
        public IhtiyacDB()
            : base("name=IhtiyacDB3")
        {
        }

        public virtual DbSet<Bagislar> Bagislars { get; set; }
        public virtual DbSet<Ilceler> Ilcelers { get; set; }
        public virtual DbSet<Iletisim> Iletisims { get; set; }
        public virtual DbSet<Kategori> Kategoris { get; set; }
        public virtual DbSet<Kullanici> Kullanicis { get; set; }
        public virtual DbSet<KullaniciRol> KullaniciRols { get; set; }
        public virtual DbSet<Rol> Rols { get; set; }
        public virtual DbSet<Sehirler> Sehirlers { get; set; }
        public virtual DbSet<SemtMah> SemtMahs { get; set; }
        public virtual DbSet<Sepet> Sepets { get; set; }
        public virtual DbSet<UlasanElleriniz> UlasanEllerinizs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bagislar>()
                .HasMany(e => e.Sepets)
                .WithOptional(e => e.Bagislar)
                .HasForeignKey(e => e.BagisId);

            modelBuilder.Entity<Bagislar>()
                .HasMany(e => e.UlasanEllerinizs)
                .WithOptional(e => e.Bagislar)
                .HasForeignKey(e => e.BagisID);

            modelBuilder.Entity<Ilceler>()
                .HasMany(e => e.SemtMahs)
                .WithRequired(e => e.Ilceler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Kullanici>()
                .HasMany(e => e.KullaniciRols)
                .WithRequired(e => e.Kullanici)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Rol>()
                .HasMany(e => e.KullaniciRols)
                .WithRequired(e => e.Rol)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sehirler>()
                .HasMany(e => e.Ilcelers)
                .WithRequired(e => e.Sehirler)
                .WillCascadeOnDelete(false);
        }
    }
}
