using Microsoft.EntityFrameworkCore;

namespace CivilServiceViewer.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<VMarriage> VMarriages { get; set; }

    public virtual DbSet<VPersonAddressStatus> VPersonAddressStatuses { get; set; }

    public virtual DbSet<VPersonBirth> VPersonBirths { get; set; }

    public virtual DbSet<VPersonDeath> VPersonDeaths { get; set; }

    public virtual DbSet<VPersonFamily> VPersonFamilies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VMarriage>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_marriages");

            entity.Property(e => e.MarriageDate).HasColumnName("marriage_date");
            entity.Property(e => e.MarriagePlace)
                .HasMaxLength(100)
                .HasColumnName("marriage_place");
            entity.Property(e => e.RegistryDate).HasColumnName("registry_date");
            entity.Property(e => e.RegistryNumber).HasColumnName("registry_number");
            entity.Property(e => e.Spouse1Pesel).HasColumnName("spouse1_pesel");
            entity.Property(e => e.Spouse2Pesel).HasColumnName("spouse2_pesel");
        });

        modelBuilder.Entity<VPersonAddressStatus>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_person_address_status");

            entity.Property(e => e.BirthDate).HasColumnName("birth_date");
            entity.Property(e => e.BirthPlace)
                .HasMaxLength(100)
                .HasColumnName("birth_place");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.Country)
                .HasMaxLength(60)
                .HasColumnName("country");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.Gender)
                .HasMaxLength(9)
                .HasColumnName("gender");
            entity.Property(e => e.HouseNumber).HasColumnName("house_number");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(100)
                .HasColumnName("middle_name");
            entity.Property(e => e.Pesel).HasColumnName("pesel");
            entity.Property(e => e.PostalCode)
                .HasMaxLength(15)
                .HasColumnName("postal_code");
            entity.Property(e => e.StatusZycia).HasColumnName("status_zycia");
            entity.Property(e => e.Street)
                .HasMaxLength(100)
                .HasColumnName("street");
        });

        modelBuilder.Entity<VPersonBirth>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_person_birth");

            entity.Property(e => e.BirthDate).HasColumnName("birth_date");
            entity.Property(e => e.BirthPlace)
                .HasMaxLength(100)
                .HasColumnName("birth_place");
            entity.Property(e => e.Pesel).HasColumnName("pesel");
            entity.Property(e => e.PeselMatki).HasColumnName("pesel_matki");
            entity.Property(e => e.PeselOjca).HasColumnName("pesel_ojca");
            entity.Property(e => e.RegistryDate).HasColumnName("registry_date");
            entity.Property(e => e.RegistryNumber).HasColumnName("registry_number");
        });

        modelBuilder.Entity<VPersonDeath>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_person_death");

            entity.Property(e => e.CauseOfDeath)
                .HasMaxLength(100)
                .HasColumnName("cause_of_death");
            entity.Property(e => e.DeathDate).HasColumnName("death_date");
            entity.Property(e => e.DeathPlace)
                .HasMaxLength(100)
                .HasColumnName("death_place");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.Pesel).HasColumnName("pesel");
            entity.Property(e => e.RegistryDate).HasColumnName("registry_date");
            entity.Property(e => e.RegistryNumber).HasColumnName("registry_number");
        });

        modelBuilder.Entity<VPersonFamily>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_person_family");

            entity.Property(e => e.PeselMalzonka).HasColumnName("pesel_malzonka");
            entity.Property(e => e.PeselMatki).HasColumnName("pesel_matki");
            entity.Property(e => e.PeselOjca).HasColumnName("pesel_ojca");
            entity.Property(e => e.PeselOsoby).HasColumnName("pesel_osoby");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
