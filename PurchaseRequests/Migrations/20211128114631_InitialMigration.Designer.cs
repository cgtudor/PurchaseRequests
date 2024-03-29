﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PurchaseRequests.Context;

namespace PurchaseRequests.Migrations
{
    [DbContext(typeof(Context.Context))]
    [Migration("20211128114631_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PurchaseRequests.DomainModels.PurchaseRequestDomainModel", b =>
                {
                    b.Property<int>("PurchaseRequestID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AccountName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("BrandId")
                        .HasColumnType("int");

                    b.Property<string>("BrandName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CardNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<string>("ProductEan")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("PurchaseRequestStatus")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<double>("TotalPrice")
                        .HasColumnType("float");

                    b.Property<DateTime>("When")
                        .HasColumnType("datetime2");

                    b.HasKey("PurchaseRequestID");

                    b.ToTable("_purchaseRequests");

                    b.HasData(
                        new
                        {
                            PurchaseRequestID = 1,
                            AccountName = "John Doe",
                            BrandId = 1,
                            BrandName = "Eypple",
                            CardNumber = "4836725244064865",
                            Description = "Old and dusty.",
                            Name = "Offbrand IPhone",
                            Price = 1.0,
                            ProductEan = "26601113",
                            ProductId = 1,
                            PurchaseRequestStatus = 1,
                            Quantity = 4,
                            TotalPrice = 4.0,
                            When = new DateTime(2021, 11, 28, 11, 46, 30, 566, DateTimeKind.Local).AddTicks(9896)
                        },
                        new
                        {
                            PurchaseRequestID = 2,
                            AccountName = "Johnny Silverhand",
                            BrandId = 2,
                            BrandName = "Gugle",
                            CardNumber = "4890429190081675",
                            Description = "Old and nasty.",
                            Name = "Offbrand Google Pixel",
                            Price = 2.0,
                            ProductEan = "14059292",
                            ProductId = 2,
                            PurchaseRequestStatus = 1,
                            Quantity = 45,
                            TotalPrice = 90.0,
                            When = new DateTime(2021, 11, 28, 11, 46, 30, 573, DateTimeKind.Local).AddTicks(5950)
                        },
                        new
                        {
                            PurchaseRequestID = 3,
                            AccountName = "S. Mario",
                            BrandId = 3,
                            BrandName = "Brick",
                            CardNumber = "4556711787875527",
                            Description = "Old and sassy.",
                            Name = "Offbrand Nokia",
                            Price = 1.0,
                            ProductEan = "62592994",
                            ProductId = 3,
                            PurchaseRequestStatus = 2,
                            Quantity = 1243,
                            TotalPrice = 1243.0,
                            When = new DateTime(2021, 11, 28, 11, 46, 30, 573, DateTimeKind.Local).AddTicks(6124)
                        },
                        new
                        {
                            PurchaseRequestID = 4,
                            AccountName = "G. Bowser",
                            BrandId = 4,
                            BrandName = "Whiteburry",
                            CardNumber = "4539817512278671",
                            Description = "Old and gassy.",
                            Name = "Offbrand Blackburry",
                            Price = 3.0,
                            ProductEan = "16361652",
                            ProductId = 4,
                            PurchaseRequestStatus = 3,
                            Quantity = 23,
                            TotalPrice = 69.0,
                            When = new DateTime(2021, 11, 28, 11, 46, 30, 573, DateTimeKind.Local).AddTicks(6134)
                        },
                        new
                        {
                            PurchaseRequestID = 5,
                            AccountName = "Yoshi",
                            BrandId = 5,
                            BrandName = "Bobsung",
                            CardNumber = "4539919751889166",
                            Description = "Old and glassy.",
                            Name = "Offbrand Samsung",
                            Price = 1.0,
                            ProductEan = "53035172",
                            ProductId = 5,
                            PurchaseRequestStatus = 1,
                            Quantity = 43,
                            TotalPrice = 43.0,
                            When = new DateTime(2021, 11, 28, 11, 46, 30, 573, DateTimeKind.Local).AddTicks(6139)
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
