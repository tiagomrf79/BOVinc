﻿// <auto-generated />
using System;
using Animal.API.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Animal.API.Migrations
{
    [DbContext(typeof(AnimalContext))]
    [Migration("20240219215106_AddPkToAnimalStatus")]
    partial class AddPkToAnimalStatus
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.HasSequence("animal_hilo")
                .IncrementsBy(10);

            modelBuilder.Entity("Animal.API.Enums.BreedingStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("BreedingStatus");

                    b.HasKey("Id");

                    b.ToTable("BreedingStatus", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Open"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Bred"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Confirmed"
                        });
                });

            modelBuilder.Entity("Animal.API.Enums.Catalog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("Catalog");

                    b.HasKey("Id");

                    b.ToTable("Catalog", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Initial inventory"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Historic record"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Calving"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Transfer"
                        });
                });

            modelBuilder.Entity("Animal.API.Enums.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("Category");

                    b.HasKey("Id");

                    b.ToTable("Category", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Calf"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Heifer"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Milking Cow"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Dry Cow"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Bull"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Steer"
                        });
                });

            modelBuilder.Entity("Animal.API.Enums.MilkingStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("MilkingStatus");

                    b.HasKey("Id");

                    b.ToTable("MilkingStatus", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Milking"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Dry"
                        });
                });

            modelBuilder.Entity("Animal.API.Enums.Purpose", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("Purpose");

                    b.HasKey("Id");

                    b.ToTable("Purpose", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Breeding"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Milk"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Meat"
                        },
                        new
                        {
                            Id = 4,
                            Name = "To cull"
                        },
                        new
                        {
                            Id = 5,
                            Name = "To sell"
                        });
                });

            modelBuilder.Entity("Animal.API.Enums.Sex", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("Sex");

                    b.HasKey("Id");

                    b.ToTable("Sex", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Female"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Male"
                        });
                });

            modelBuilder.Entity("Animal.API.Models.AnimalStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AnimalId")
                        .HasColumnType("int");

                    b.Property<int?>("BreedingStatusId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CurrentGroupName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("DateLeftHerd")
                        .HasColumnType("date");

                    b.Property<DateTime?>("DueDateForCalving")
                        .HasColumnType("date");

                    b.Property<DateTime?>("ExpectedHeatDate")
                        .HasColumnType("date");

                    b.Property<string>("LastBreedingBull")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("LastBreedingDate")
                        .HasColumnType("date");

                    b.Property<DateTime?>("LastCalvingDate")
                        .HasColumnType("date");

                    b.Property<DateTime?>("LastDryDate")
                        .HasColumnType("date");

                    b.Property<DateTime?>("LastHeatDate")
                        .HasColumnType("date");

                    b.Property<DateTime>("LastUpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("MilkingStatusId")
                        .HasColumnType("int");

                    b.Property<string>("ReasonLeftHerd")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("SheduledDryDate")
                        .HasColumnType("date");

                    b.HasKey("Id");

                    b.HasIndex("AnimalId")
                        .IsUnique();

                    b.HasIndex("BreedingStatusId");

                    b.HasIndex("MilkingStatusId");

                    b.ToTable("AnimalStatus", (string)null);
                });

            modelBuilder.Entity("Animal.API.Models.Breed", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("GestationLength")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastUpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Breed", (string)null);
                });

            modelBuilder.Entity("Animal.API.Models.FarmAnimal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseHiLo(b.Property<int>("Id"), "animal_hilo");

                    b.Property<int>("BreedId")
                        .HasColumnType("int");

                    b.Property<int>("CatalogId")
                        .HasColumnType("int");

                    b.Property<int?>("CategoryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("DamId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("date");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastUpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Notes")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<int?>("PurposeId")
                        .HasColumnType("int");

                    b.Property<string>("RegistrationId")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("SexId")
                        .HasColumnType("int");

                    b.Property<int?>("SireId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BreedId");

                    b.HasIndex("CatalogId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("DamId");

                    b.HasIndex("PurposeId");

                    b.HasIndex("SexId");

                    b.HasIndex("SireId");

                    b.ToTable("Animal", (string)null);
                });

            modelBuilder.Entity("Animal.API.Models.Lactation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CalvingDate")
                        .HasColumnType("date");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("date");

                    b.Property<int?>("FarmAnimalId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int>("LactationNumber")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastUpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("FarmAnimalId");

                    b.ToTable("Lactation", (string)null);
                });

            modelBuilder.Entity("Animal.API.Models.AnimalStatus", b =>
                {
                    b.HasOne("Animal.API.Models.FarmAnimal", "Animal")
                        .WithOne()
                        .HasForeignKey("Animal.API.Models.AnimalStatus", "AnimalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Animal.API.Enums.BreedingStatus", "BreedingStatus")
                        .WithMany()
                        .HasForeignKey("BreedingStatusId");

                    b.HasOne("Animal.API.Enums.MilkingStatus", "MilkingStatus")
                        .WithMany()
                        .HasForeignKey("MilkingStatusId");

                    b.Navigation("Animal");

                    b.Navigation("BreedingStatus");

                    b.Navigation("MilkingStatus");
                });

            modelBuilder.Entity("Animal.API.Models.FarmAnimal", b =>
                {
                    b.HasOne("Animal.API.Models.Breed", "Breed")
                        .WithMany()
                        .HasForeignKey("BreedId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Animal.API.Enums.Catalog", "Catalog")
                        .WithMany()
                        .HasForeignKey("CatalogId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Animal.API.Enums.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Animal.API.Models.FarmAnimal", "Dam")
                        .WithMany()
                        .HasForeignKey("DamId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Animal.API.Enums.Purpose", "Purpose")
                        .WithMany()
                        .HasForeignKey("PurposeId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Animal.API.Enums.Sex", "Sex")
                        .WithMany()
                        .HasForeignKey("SexId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Animal.API.Models.FarmAnimal", "Sire")
                        .WithMany()
                        .HasForeignKey("SireId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Breed");

                    b.Navigation("Catalog");

                    b.Navigation("Category");

                    b.Navigation("Dam");

                    b.Navigation("Purpose");

                    b.Navigation("Sex");

                    b.Navigation("Sire");
                });

            modelBuilder.Entity("Animal.API.Models.Lactation", b =>
                {
                    b.HasOne("Animal.API.Models.FarmAnimal", "FarmAnimal")
                        .WithMany()
                        .HasForeignKey("FarmAnimalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FarmAnimal");
                });
#pragma warning restore 612, 618
        }
    }
}
