using Dystopian_Civil_Office.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dystopian_Civil_Office.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Address> Addresses { get; set; }
    public DbSet<Person> Persons { get; set; }
    public DbSet<BirthRecord> BirthRecords { get; set; }
    public DbSet<MarriageRecord> MarriageRecords { get; set; }
    public DbSet<DeathRecord> DeathRecords { get; set; }
    public DbSet<Document> Documents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Document>(entity =>
        {
            entity.ToTable("documents");

            entity.HasKey(e => e.DocumentId);

            entity.Property(e => e.DocumentId)
                .HasColumnName("document_id")
                .UseIdentityByDefaultColumn();

            entity.Property(e => e.Name)
                .HasColumnName("name")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.Category)
                .HasColumnName("category")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.ImportDate)
                .HasColumnName("import_date")
                .IsRequired();
        });

        modelBuilder.Entity<Address>(entity =>
        {
            entity.ToTable("addresses");

            entity.HasKey(e => e.AddressId);

            entity.Property(e => e.AddressId)
                .HasColumnName("address_id")
                .UseIdentityByDefaultColumn();

            entity.Property(e => e.City)
                .HasColumnName("city")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.Street)
                .HasColumnName("street")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.HouseNumber)
                .HasColumnName("house_number")
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(e => e.ApartmentNumber)
                .HasColumnName("apartment_number")
                .HasMaxLength(20);

            entity.Property(e => e.PostalCode)
                .HasColumnName("postal_code")
                .HasMaxLength(15)
                .IsRequired();

            entity.Property(e => e.Country)
                .HasColumnName("country")
                .HasMaxLength(60)
                .IsRequired();

            entity.Property(e => e.DocumentId)
                .HasColumnName("document_id");

            entity.HasIndex(e => e.DocumentId)
                .IsUnique();

            entity.HasOne(e => e.Document)
                .WithOne(d => d.Address)
                .HasForeignKey<Address>(e => e.DocumentId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.ToTable("persons");

            entity.HasKey(e => e.PersonId);

            entity.Property(e => e.PersonId)
                .HasColumnName("person_id")
                .UseIdentityByDefaultColumn();

            entity.Property(e => e.Pesel)
                .HasColumnName("pesel")
                .HasMaxLength(11)
                .IsRequired();

            entity.HasIndex(e => e.Pesel)
                .IsUnique();

            entity.Property(e => e.FirstName)
                .HasColumnName("first_name")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.MiddleName)
                .HasColumnName("middle_name")
                .HasMaxLength(100);

            entity.Property(e => e.LastName)
                .HasColumnName("last_name")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.Gender)
                .HasColumnName("gender")
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(e => e.BirthDate)
                .HasColumnName("birth_date")
                .HasColumnType("date")
                .IsRequired();

            entity.Property(e => e.BirthPlace)
                .HasColumnName("birth_place")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.AddressId)
                .HasColumnName("address_id")
                .IsRequired();

            entity.Property(e => e.DocumentId)
                .HasColumnName("document_id");

            entity.HasIndex(e => e.DocumentId)
                .IsUnique();

            entity.HasOne(e => e.Address)
                .WithMany(a => a.Persons)
                .HasForeignKey(e => e.AddressId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Document)
                .WithOne(d => d.Person)
                .HasForeignKey<Person>(e => e.DocumentId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasCheckConstraint("ck_persons_gender", "\"gender\" IN ('Male', 'Female', 'Other')");
        });

        modelBuilder.Entity<BirthRecord>(entity =>
        {
            entity.ToTable("birth_records");

            entity.HasKey(e => e.BirthRecordId);

            entity.Property(e => e.BirthRecordId)
                .HasColumnName("birth_record_id")
                .UseIdentityByDefaultColumn();

            entity.Property(e => e.RegistryNumber)
                .HasColumnName("registry_number")
                .HasMaxLength(50)
                .IsRequired();

            entity.HasIndex(e => e.RegistryNumber)
                .IsUnique();

            entity.Property(e => e.PersonId)
                .HasColumnName("person_id")
                .IsRequired();

            entity.HasIndex(e => e.PersonId)
                .IsUnique();

            entity.Property(e => e.MotherId)
                .HasColumnName("mother_id");

            entity.Property(e => e.FatherId)
                .HasColumnName("father_id");

            entity.Property(e => e.RegistryDate)
                .HasColumnName("registry_date")
                .HasColumnType("date")
                .IsRequired();

            entity.Property(e => e.DocumentId)
                .HasColumnName("document_id");

            entity.HasIndex(e => e.DocumentId)
                .IsUnique();

            entity.HasOne(e => e.Person)
                .WithOne(p => p.BirthRecord)
                .HasForeignKey<BirthRecord>(e => e.PersonId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Mother)
                .WithMany(p => p.BirthRecordsAsMother)
                .HasForeignKey(e => e.MotherId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Father)
                .WithMany(p => p.BirthRecordsAsFather)
                .HasForeignKey(e => e.FatherId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Document)
                .WithOne(d => d.BirthRecord)
                .HasForeignKey<BirthRecord>(e => e.DocumentId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<DeathRecord>(entity =>
        {
            entity.ToTable("death_records");

            entity.HasKey(e => e.DeathRecordId);

            entity.Property(e => e.DeathRecordId)
                .HasColumnName("death_record_id")
                .UseIdentityByDefaultColumn();

            entity.Property(e => e.RegistryNumber)
                .HasColumnName("registry_number")
                .HasMaxLength(50)
                .IsRequired();

            entity.HasIndex(e => e.RegistryNumber)
                .IsUnique();

            entity.Property(e => e.PersonId)
                .HasColumnName("person_id")
                .IsRequired();

            entity.HasIndex(e => e.PersonId)
                .IsUnique();

            entity.Property(e => e.DeathDate)
                .HasColumnName("death_date")
                .HasColumnType("date")
                .IsRequired();

            entity.Property(e => e.DeathPlace)
                .HasColumnName("death_place")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.RegistryDate)
                .HasColumnName("registry_date")
                .HasColumnType("date")
                .IsRequired();

            entity.Property(e => e.CauseOfDeath)
                .HasColumnName("cause_of_death")
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(e => e.DocumentId)
                .HasColumnName("document_id");

            entity.HasIndex(e => e.DocumentId)
                .IsUnique();

            entity.HasOne(e => e.Person)
                .WithOne(p => p.DeathRecord)
                .HasForeignKey<DeathRecord>(e => e.PersonId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Document)
                .WithOne(d => d.DeathRecord)
                .HasForeignKey<DeathRecord>(e => e.DocumentId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<MarriageRecord>(entity =>
        {
            entity.ToTable("marriage_records");

            entity.HasKey(e => e.MarriageRecordId);

            entity.Property(e => e.MarriageRecordId)
                .HasColumnName("marriage_record_id")
                .UseIdentityByDefaultColumn();

            entity.Property(e => e.RegistryNumber)
                .HasColumnName("registry_number")
                .HasMaxLength(50)
                .IsRequired();

            entity.HasIndex(e => e.RegistryNumber)
                .IsUnique();

            entity.Property(e => e.Spouse1Id)
                .HasColumnName("spouse1_id")
                .IsRequired();

            entity.Property(e => e.Spouse2Id)
                .HasColumnName("spouse2_id")
                .IsRequired();

            entity.Property(e => e.MarriageDate)
                .HasColumnName("marriage_date")
                .HasColumnType("date")
                .IsRequired();

            entity.Property(e => e.MarriagePlace)
                .HasColumnName("marriage_place")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.RegistryDate)
                .HasColumnName("registry_date")
                .HasColumnType("date")
                .IsRequired();

            entity.Property(e => e.DocumentId)
                .HasColumnName("document_id");

            entity.HasIndex(e => e.DocumentId)
                .IsUnique();

            entity.HasOne(e => e.Spouse1)
                .WithMany(p => p.MarriagesAsSpouse1)
                .HasForeignKey(e => e.Spouse1Id)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Spouse2)
                .WithMany(p => p.MarriagesAsSpouse2)
                .HasForeignKey(e => e.Spouse2Id)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Document)
                .WithOne(d => d.MarriageRecord)
                .HasForeignKey<MarriageRecord>(e => e.DocumentId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasCheckConstraint("ck_marriage_records_spouses_different", "\"spouse1_id\" <> \"spouse2_id\"");
        });
    }
}