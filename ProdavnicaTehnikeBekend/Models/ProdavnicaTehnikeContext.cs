using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ProdavnicaTehnikeBekend.Models;

public partial class ProdavnicaTehnikeContext : DbContext
{
    public ProdavnicaTehnikeContext()
    {
    }

    public ProdavnicaTehnikeContext(DbContextOptions<ProdavnicaTehnikeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Kupac> Kupacs { get; set; }

    public virtual DbSet<Porudzbina> Porudzbinas { get; set; }

    public virtual DbSet<PorudzbinaProizvod> PorudzbinaProizvods { get; set; }

    public virtual DbSet<Proizvod> Proizvods { get; set; }

    public virtual DbSet<Zaposleni> Zaposlenis { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\MARKO;Database=ProdavnicaTehnike;Integrated Security=True;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Kupac>(entity =>
        {
            entity.ToTable("Kupac", "ProdavnicaTehnike");

            entity.Property(e => e.KupacId).HasColumnName("kupacID");
            entity.Property(e => e.AdresaKupca)
                .HasMaxLength(255)
                .HasColumnName("adresaKupca");
            entity.Property(e => e.BrojTelefonaKupca)
                .HasMaxLength(20)
                .HasColumnName("brojTelefonaKupca");
            entity.Property(e => e.DatumRodjenjaKupca).HasColumnName("datumRodjenjaKupca");
            entity.Property(e => e.GradKupca)
                .HasMaxLength(100)
                .HasColumnName("gradKupca");
            entity.Property(e => e.KontaktKupca)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("kontaktKupca");
            entity.Property(e => e.KorisnickoImeKupca)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("korisnickoImeKupca");
            entity.Property(e => e.PorudzbinaId).HasColumnName("porudzbinaID");
            entity.Property(e => e.SifraKupca)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("sifraKupca");

            entity.HasOne(d => d.Porudzbina).WithMany(p => p.Kupacs)
                .HasForeignKey(d => d.PorudzbinaId)
                .HasConstraintName("FK_Kupac_Porudzbina");
        });

        modelBuilder.Entity<Porudzbina>(entity =>
        {
            entity.ToTable("Porudzbina", "ProdavnicaTehnike");

            entity.Property(e => e.PorudzbinaId).HasColumnName("porudzbinaID");
            entity.Property(e => e.AdresaPorudzbine)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("adresaPorudzbine");
            entity.Property(e => e.DatumPlacanja).HasColumnName("datumPlacanja");
            entity.Property(e => e.DatumPorudzbine).HasColumnName("datumPorudzbine");
        });

        modelBuilder.Entity<PorudzbinaProizvod>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PorudzbinaProizvod", "ProdavnicaTehnike");

            entity.Property(e => e.PorudzbinaId).HasColumnName("porudzbinaID");
            entity.Property(e => e.ProizvodId).HasColumnName("proizvodID");

            entity.HasOne(d => d.Porudzbina).WithMany()
                .HasForeignKey(d => d.PorudzbinaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PorudzbinaProizvod_Porudzbina");

            entity.HasOne(d => d.Proizvod).WithMany()
                .HasForeignKey(d => d.ProizvodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PorudzbinaProizvod_Proizvod");
        });

        modelBuilder.Entity<Proizvod>(entity =>
        {
            entity.ToTable("Proizvod", "ProdavnicaTehnike", tb => tb.HasTrigger("UpdateStatusProizvoda"));

            entity.Property(e => e.ProizvodId).HasColumnName("proizvodID");
            entity.Property(e => e.CenaProizvoda)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("cenaProizvoda");
            entity.Property(e => e.Kolicina).HasColumnName("kolicina");
            entity.Property(e => e.NazivProizvoda)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("nazivProizvoda");
            entity.Property(e => e.OpisProizvoda)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("opisProizvoda");
            entity.Property(e => e.StatusProizvoda)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("Dostupan")
                .HasColumnName("statusProizvoda");
            entity.Property(e => e.TipProizvoda)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("tipProizvoda");
        });

        modelBuilder.Entity<Zaposleni>(entity =>
        {
            entity.ToTable("Zaposleni", "ProdavnicaTehnike");

            entity.Property(e => e.ZaposleniId).HasColumnName("zaposleniID");
            entity.Property(e => e.KontaktZaposlenog)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("kontaktZaposlenog");
            entity.Property(e => e.KorisnickoImeZaposlenog)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("korisnickoImeZaposlenog");
            entity.Property(e => e.PorudzbinaId).HasColumnName("porudzbinaID");
            entity.Property(e => e.SifraZaposlenog)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("sifraZaposlenog");

            entity.HasOne(d => d.Porudzbina).WithMany(p => p.Zaposlenis)
                .HasForeignKey(d => d.PorudzbinaId)
                .HasConstraintName("FK_Zaposleni_Porudzbina");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    public static implicit operator ProdavnicaTehnikeContext(Repositories.ProizvodRepository v)
    {
        throw new NotImplementedException();
    }
}
