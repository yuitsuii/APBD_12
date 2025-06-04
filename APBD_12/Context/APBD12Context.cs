using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace APBD_12.Models;

public partial class APBD12Context : DbContext
{
    public APBD12Context()
    {
    }

    public APBD12Context(DbContextOptions<APBD12Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<ClientTrip> ClientTrips { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Trip> Trips { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.IdClient).HasName("Client_pk");

            entity.ToTable("Client");

            entity.Property(e => e.Email).HasMaxLength(120);
            entity.Property(e => e.FirstName).HasMaxLength(120);
            entity.Property(e => e.LastName).HasMaxLength(120);
            entity.Property(e => e.Pesel).HasMaxLength(120);
            entity.Property(e => e.Telephone).HasMaxLength(120);
        });

        modelBuilder.Entity<ClientTrip>(entity =>
        {
            entity.HasKey(e => new { e.IdClient, e.IdTrip }).HasName("Client_Trip_pk");

            entity.ToTable("Client_Trip");

            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.RegisteredAt).HasColumnType("datetime");

            entity.HasOne(d => d.IdClientNavigation).WithMany(p => p.ClientTrips)
                .HasForeignKey(d => d.IdClient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Table_5_Client");

            entity.HasOne(d => d.IdTripNavigation).WithMany(p => p.ClientTrips)
                .HasForeignKey(d => d.IdTrip)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Table_5_Trip");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.IdCountry).HasName("Country_pk");

            entity.ToTable("Country");

            entity.Property(e => e.Name).HasMaxLength(120);

            entity.HasMany(d => d.IdTrips).WithMany(p => p.IdCountries)
                .UsingEntity<Dictionary<string, object>>(
                    "CountryTrip",
                    r => r.HasOne<Trip>().WithMany()
                        .HasForeignKey("IdTrip")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Country_Trip_Trip"),
                    l => l.HasOne<Country>().WithMany()
                        .HasForeignKey("IdCountry")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Country_Trip_Country"),
                    j =>
                    {
                        j.HasKey("IdCountry", "IdTrip").HasName("Country_Trip_pk");
                        j.ToTable("Country_Trip");
                    });
        });

        modelBuilder.Entity<Trip>(entity =>
        {
            entity.HasKey(e => e.IdTrip).HasName("Trip_pk");

            entity.ToTable("Trip");

            entity.Property(e => e.DateFrom).HasColumnType("datetime");
            entity.Property(e => e.DateTo).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(220);
            entity.Property(e => e.Name).HasMaxLength(120);
        });

        modelBuilder.Entity<Client>().HasData(
            new Client
            {
                IdClient = 1,
                FirstName = "John",
                LastName = "Smith",
                Email = "john.smith@example.com",
                Telephone = "123456789",
                Pesel = "90010112345"
            },
            new Client
            {
                IdClient = 2,
                FirstName = "Jake",
                LastName = "Doe",
                Email = "jake.doe@example.com",
                Telephone = "987654321",
                Pesel = "92020254321"
            },
            new Client
            {
                IdClient = 10,
                FirstName = "Delete",
                LastName = "test",
                Email = "delete@example.com",
                Telephone = "897654321",
                Pesel = "29020254321"
            }
        );

        modelBuilder.Entity<Trip>().HasData(
            new Trip
            {
                IdTrip = 1,
                Name = "ABC",
                Description = "Lorem ipsum...",
                DateFrom = new DateTime(2025, 7, 1),
                DateTo = new DateTime(2025, 7, 10),
                MaxPeople = 20
            }
        );

        modelBuilder.Entity<Country>().HasData(
            new Country { IdCountry = 1, Name = "Poland" },
            new Country { IdCountry = 2, Name = "Germany" }
        );

        modelBuilder.Entity<ClientTrip>().HasData(
            new ClientTrip
            {
                IdClient = 1,
                IdTrip = 1,
                RegisteredAt = new DateTime(2025, 1, 1),
                PaymentDate = null
            },
            new ClientTrip
            {
                IdClient = 2,
                IdTrip = 1,
                RegisteredAt = new DateTime(2025, 1, 1),
                PaymentDate = null
            }
        );

        modelBuilder.Entity<Country>()
            .HasMany(c => c.IdTrips)
            .WithMany(t => t.IdCountries)
            .UsingEntity<Dictionary<string, object>>(
                "Country_Trip",
                r => r.HasOne<Trip>().WithMany().HasForeignKey("IdTrip"),
                l => l.HasOne<Country>().WithMany().HasForeignKey("IdCountry"),
                je =>
                {
                    je.HasKey("IdCountry", "IdTrip");
                    je.HasData(
                        new { IdCountry = 1, IdTrip = 1 },
                        new { IdCountry = 2, IdTrip = 1 }
                    );
                });
        
        
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    
    
    
}
