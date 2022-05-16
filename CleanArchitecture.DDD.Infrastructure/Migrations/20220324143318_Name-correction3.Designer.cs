﻿// <auto-generated />
using System;
using CleanArchitecture.DDD.Infrastructure.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CleanArchitecture.DDD.Infrastructure.Migrations
{
    [DbContext(typeof(DomainDbContext))]
    [Migration("20220324143318_Name-correction3")]
    partial class Namecorrection3
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-preview.2.22153.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("CleanArchitecture.DDD.Infrastructure.Persistence.Entities.Address", b =>
                {
                    b.Property<Guid>("AddressID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("ID");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StreetAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ZipCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AddressID");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("CleanArchitecture.DDD.Infrastructure.Persistence.Entities.Doctor", b =>
                {
                    b.Property<Guid>("DoctorID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("ID");

                    b.Property<Guid>("AddressId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("DoctorID");

                    b.HasIndex("AddressId");

                    b.ToTable("Doctors");
                });

            modelBuilder.Entity("CleanArchitecture.DDD.Infrastructure.Persistence.Entities.Doctor", b =>
                {
                    b.HasOne("CleanArchitecture.DDD.Infrastructure.Persistence.Entities.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("CleanArchitecture.DDD.Domain.ValueObjects.Name", "Name", b1 =>
                        {
                            b1.Property<Guid>("DoctorID")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("FirstOrLastname")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Lastname")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Middlename")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("DoctorID");

                            b1.ToTable("Doctors");

                            b1.WithOwner()
                                .HasForeignKey("DoctorID");
                        });

                    b.Navigation("Address");

                    b.Navigation("Name")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
