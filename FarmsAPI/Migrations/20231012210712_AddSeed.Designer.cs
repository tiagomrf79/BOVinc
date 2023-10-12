﻿// <auto-generated />
using System;
using FarmsAPI.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FarmsAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231012210712_AddSeed")]
    partial class AddSeed
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("FarmsAPI.Models.Farm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("City")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Country")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Region")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Farms");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Address = "Avenida Diogo Leite 70",
                            City = "Porto",
                            Country = "Portugal",
                            DateCreated = new DateTime(2023, 10, 7, 15, 57, 36, 0, DateTimeKind.Unspecified),
                            DateUpdated = new DateTime(2023, 10, 11, 17, 51, 22, 0, DateTimeKind.Unspecified),
                            Name = "Jorge Oliveira de Pacheco",
                            Region = "Porto"
                        },
                        new
                        {
                            Id = 2,
                            Address = "Estrada Monumental 316",
                            City = "Funchal",
                            Country = "Portugal",
                            DateCreated = new DateTime(2023, 4, 3, 17, 32, 38, 0, DateTimeKind.Unspecified),
                            DateUpdated = new DateTime(2023, 4, 9, 15, 34, 34, 0, DateTimeKind.Unspecified),
                            Name = "Eduarda Clara Moreira",
                            Region = "Madeira"
                        },
                        new
                        {
                            Id = 3,
                            Address = "Avenida Calouste Gulbenkian 22B",
                            City = "Coimbra",
                            Country = "Portugal",
                            DateCreated = new DateTime(2023, 7, 15, 16, 42, 14, 0, DateTimeKind.Unspecified),
                            Name = "Nuno Abreu Nascimento",
                            Region = "Coimbra"
                        },
                        new
                        {
                            Id = 4,
                            Address = "Largo Senhora-A-Branca 144\r\n",
                            City = "Braga",
                            Country = "Portugal",
                            DateCreated = new DateTime(2023, 8, 28, 15, 34, 34, 0, DateTimeKind.Unspecified),
                            Name = "André Raúl Cardoso",
                            Region = "Braga"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
