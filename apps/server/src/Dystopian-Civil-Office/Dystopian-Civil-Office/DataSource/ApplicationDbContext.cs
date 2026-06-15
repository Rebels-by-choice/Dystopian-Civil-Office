using Dystopian_Civil_Office.Models.Archives;
using Dystopian_Civil_Office.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dystopian_Civil_Office.DataSource;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // ORM to main data tables
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Person> Persons { get; set; }
    public DbSet<BirthRecord> BirthRecords { get; set; }
    public DbSet<MarriageRecord> MarriageRecords { get; set; }
    public DbSet<DeathRecord> DeathRecords { get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<Case> Cases { get; set; }

    // ORM to archive tables
    public DbSet<DocumentArchive> DocumentArchives { get; set; }
    public DbSet<PersonArchive> PersonArchives { get; set; }
    public DbSet<BirthRecordArchive> BirthRecordArchives { get; set; }
    public DbSet<DeathRecordArchive> DeathRecordArchives { get; set; }
    public DbSet<MarriageRecordArchive> MarriageRecordArchives { get; set; }
    public DbSet<AddressArchive> AddressArchives { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Case>(entity =>
        {
            entity.ToTable("cases");

            entity.HasKey(e => e.CaseId);
            
            entity.Property(e => e.CaseId)
                .HasColumnName("case_id")
                .UseIdentityByDefaultColumn();

            entity.Property(e => e.Status)
                .HasColumnName("status")
                .IsRequired();
            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();
            entity.Property(e => e.ClosedAt)
                .HasColumnName("closed_at");
            entity.Property(e => e.InitiatorId)
                .HasColumnName("initiator_id")
                .IsRequired();
            entity.Property(e => e.ResponderId)
                .HasColumnName("responder_id");
            
            entity.HasOne(e => e.Initiator)
                .WithMany() // Leave empty if Person doesn't have an "InitiatedCases" collection property
                .HasForeignKey(e => e.InitiatorId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(e => e.Responder)
                .WithMany() // Leave empty if Person doesn't have a "RespondedCases" collection property
                .HasForeignKey(e => e.ResponderId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        
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
        
        modelBuilder.Entity<DocumentArchive>(entity =>
        {
            entity.ToTable("document_archives");
            entity.HasKey(e => e.DocumentArchiveId);
            entity.Property(e => e.Name).HasMaxLength(255).IsRequired();
            entity.Property(e => e.Category).HasMaxLength(100).IsRequired();
            entity.Property(e => e.ImportDate).IsRequired();
            entity.Property(e => e.DeletedAt).IsRequired();
        });

        modelBuilder.Entity<PersonArchive>(entity =>
        {
            entity.ToTable("person_archives");
            entity.HasKey(e => e.PersonArchiveId);
            entity.Property(e => e.PersonPesel).HasMaxLength(11).IsRequired();
            entity.Property(e => e.FirstName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.MiddleName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Gender).HasMaxLength(20).IsRequired();
            entity.Property(e => e.BirthPlace).HasMaxLength(200).IsRequired();
            entity.Property(e => e.DocumentName).HasMaxLength(255);
            entity.Property(e => e.DeletedAt).IsRequired();
        });

        modelBuilder.Entity<BirthRecordArchive>(entity =>
        {
            entity.ToTable("birth_record_archives");
            entity.HasKey(e => e.BirthRecordArchiveId);
            entity.Property(e => e.RegistryNumber).HasMaxLength(50).IsRequired();
            entity.Property(e => e.BornPersonPesel).HasMaxLength(11).IsRequired();
            entity.Property(e => e.MotherPesel).HasMaxLength(11);
            entity.Property(e => e.FatherPesel).HasMaxLength(11);
            entity.Property(e => e.BirthPlace).HasMaxLength(200).IsRequired();
            entity.Property(e => e.DocumentName).HasMaxLength(255);
            entity.Property(e => e.DeletedAt).IsRequired();
        });

        modelBuilder.Entity<DeathRecordArchive>(entity =>
        {
            entity.ToTable("death_record_archives");
            entity.HasKey(e => e.DeathRecordArchiveId);
            entity.Property(e => e.RegistryNumber).HasMaxLength(50).IsRequired();
            entity.Property(e => e.PersonPesel).HasMaxLength(11).IsRequired();
            entity.Property(e => e.DeathPlace).HasMaxLength(200).IsRequired();
            entity.Property(e => e.CauseOfDeath).HasMaxLength(200).IsRequired();
            entity.Property(e => e.DocumentName).HasMaxLength(255);
            entity.Property(e => e.DeletedAt).IsRequired();
        });

        modelBuilder.Entity<MarriageRecordArchive>(entity =>
        {
            entity.ToTable("marriage_record_archives");
            entity.HasKey(e => e.MarriageRecordArchiveId);
            entity.Property(e => e.RegistryNumber).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Spouse1Pesel).HasMaxLength(11).IsRequired();
            entity.Property(e => e.Spouse2Pesel).HasMaxLength(11).IsRequired();
            entity.Property(e => e.MarriagePlace).HasMaxLength(200).IsRequired();
            entity.Property(e => e.DocumentName).HasMaxLength(255);
            entity.Property(e => e.DeletedAt).IsRequired();
        });

        modelBuilder.Entity<AddressArchive>(entity =>
        {
            entity.ToTable("address_archives");
            entity.HasKey(e => e.AddressArchiveId);
            entity.Property(e => e.City).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Street).HasMaxLength(150).IsRequired();
            entity.Property(e => e.HouseNumber).HasMaxLength(20).IsRequired();
            entity.Property(e => e.ApartmentNumber).HasMaxLength(20);
            entity.Property(e => e.PostalCode).HasMaxLength(20).IsRequired();
            entity.Property(e => e.Country).HasMaxLength(100).IsRequired();
            entity.Property(e => e.DocumentName).HasMaxLength(255);
            entity.Property(e => e.DeletedAt).IsRequired();
        });
    }
}