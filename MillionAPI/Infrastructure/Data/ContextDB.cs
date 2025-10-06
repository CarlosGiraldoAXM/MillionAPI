using Microsoft.EntityFrameworkCore;
using MillionAPI.Domain.Entities;

namespace MillionAPI.Infrastructure.Data;

public partial class ContextDB : DbContext
{
    public ContextDB()
    {
    }

    public ContextDB(DbContextOptions<ContextDB> options)
        : base(options)
    {
    }

    public virtual DbSet<Owner> Owner { get; set; }
    public virtual DbSet<Property> Property { get; set; }
    public virtual DbSet<PropertyImage> PropertyImage { get; set; }
    public virtual DbSet<PropertyTrace> PropertyTrace { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=PC-CARLOS\\SQLEXPRESS;Database=MillionDB;Trusted_Connection=False;User=MillionDB;Password=milliondb;TrustServerCertificate=True;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuración de Owner
        modelBuilder.Entity<Owner>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("Owner");
            entity.Property(e => e.Id).HasColumnName("IdOwner");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .IsRequired()
                .HasColumnType("nvarchar(150)");
            entity.Property(e => e.Address)
                .HasMaxLength(250)
                .IsRequired()
                .HasColumnType("nvarchar(250)");
            entity.Property(e => e.Photo)
                .HasColumnType("varbinary(max)");
            entity.Property(e => e.Birthday)
                .HasColumnType("date");
        });

        // Configuración de Property
        modelBuilder.Entity<Property>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("Property");
            entity.Property(e => e.Id).HasColumnName("IdProperty");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .IsRequired()
                .HasColumnType("nvarchar(150)");
            entity.Property(e => e.Address)
                .HasMaxLength(250)
                .IsRequired()
                .HasColumnType("nvarchar(250)");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(18, 2)")
                .IsRequired();
            entity.Property(e => e.CodeInternal)
                .HasMaxLength(50)
                .IsRequired()
                .HasColumnType("nvarchar(50)");
            entity.Property(e => e.Year);
            entity.Property(e => e.OwnerId).HasColumnName("IdOwner").IsRequired();

            entity.HasOne(d => d.Owner)
                .WithMany(p => p.Properties)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configuración de PropertyImage
        modelBuilder.Entity<PropertyImage>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("PropertyImage");
            entity.Property(e => e.Id).HasColumnName("IdPropertyImage");
            entity.Property(e => e.PropertyId).HasColumnName("IdProperty").IsRequired();
            entity.Property(e => e.File)
                .IsRequired()
                .HasColumnType("varbinary(max)");
            entity.Property(e => e.Enabled).IsRequired();

            entity.HasOne(d => d.Property)
                .WithMany(p => p.Images)
                .HasForeignKey(d => d.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configuración de PropertyTrace
        modelBuilder.Entity<PropertyTrace>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("PropertyTrace");
            entity.Property(e => e.Id).HasColumnName("IdPropertyTrace");
            entity.Property(e => e.PropertyId).HasColumnName("IdProperty").IsRequired();
            entity.Property(e => e.DateSale)
                .IsRequired()
                .HasColumnType("datetime2");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsRequired()
                .HasColumnType("nvarchar(200)");
            entity.Property(e => e.Value)
                .HasColumnType("decimal(18, 2)")
                .IsRequired();
            entity.Property(e => e.Tax)
                .HasColumnType("decimal(18, 2)")
                .IsRequired();

            entity.HasOne(d => d.Property)
                .WithMany(p => p.Traces)
                .HasForeignKey(d => d.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
