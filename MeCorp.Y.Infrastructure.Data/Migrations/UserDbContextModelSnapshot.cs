﻿// <auto-generated />
using System;
using MeCorp.Y.Api;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MeCorp.Y.Infrastructure.Data.Migrations
{
    [DbContext(typeof(UserDbContext))]
    partial class UserDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MeCorp.Y.Infrastructure.Data.PersistenceEntities.BlockedIpEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("BlockUntil")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("CreatedAtUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("FailedLoginAttempts")
                        .HasColumnType("integer");

                    b.Property<string>("IpAddress")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("BlockedIps");
                });

            modelBuilder.Entity("MeCorp.Y.Infrastructure.Data.PersistenceEntities.ReferralTokenEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAtUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsValid")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("ReferralTokens");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Code = "CreateAsManager",
                            CreatedAtUtc = new DateTime(2025, 4, 1, 19, 49, 25, 108, DateTimeKind.Utc).AddTicks(5401),
                            IsValid = true
                        },
                        new
                        {
                            Id = 2,
                            Code = "FromLinkedin",
                            CreatedAtUtc = new DateTime(2025, 4, 1, 19, 49, 25, 108, DateTimeKind.Utc).AddTicks(5717),
                            IsValid = true
                        });
                });

            modelBuilder.Entity("MeCorp.Y.Infrastructure.Data.PersistenceEntities.UserEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAtUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedAtUtc = new DateTime(2025, 4, 1, 19, 49, 25, 108, DateTimeKind.Utc).AddTicks(1545),
                            PasswordHash = "k75FPfxn177WTgOsJH251v3sLKFCy7rH0tA1Xq3bveIf1KxwSsxnaIKTOnkA67DSohFqwUwCJz4ByFKZuDhM3Q==",
                            Role = "Admin",
                            Username = "mehmet"
                        },
                        new
                        {
                            Id = 2,
                            CreatedAtUtc = new DateTime(2025, 4, 1, 19, 49, 25, 108, DateTimeKind.Utc).AddTicks(1962),
                            PasswordHash = "k75FPfxn177WTgOsJH251v3sLKFCy7rH0tA1Xq3bveIf1KxwSsxnaIKTOnkA67DSohFqwUwCJz4ByFKZuDhM3Q==",
                            Role = "Admin",
                            Username = "mecorp"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
