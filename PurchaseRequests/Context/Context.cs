using Microsoft.EntityFrameworkCore;
using PurchaseRequests.DomainModels;
using PurchaseRequests.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseRequests.Context
{
    public class Context : DbContext
    {
        public DbSet<PurchaseRequestDomainModel> _purchaseRequests { get; set; }

        public Context(DbContextOptions<Context> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<PurchaseRequestDomainModel>()
                .HasData(
                new PurchaseRequestDomainModel { PurchaseRequestID = 1, 
                    AccountName = "John Doe", 
                    CardNumber = "4836725244064865", 
                    ProductId = 1, 
                    Quantity = 4,
                    When = DateTime.Now,
                    Name = "Offbrand IPhone",
                    Description = "Old and dusty.",
                    ProductEan = "26601113",
                    BrandId = 1,
                    BrandName = "Eypple",
                    Price = 1,
                    TotalPrice = 4,
                    PurchaseRequestStatus = PurchaseRequestStatus.PENDING
                },
                new PurchaseRequestDomainModel { 
                    PurchaseRequestID = 2, 
                    AccountName = "Johnny Silverhand", 
                    CardNumber = "4890429190081675", ProductId = 2, 
                    Quantity = 45,
                    When = DateTime.Now,
                    Name = "Offbrand Google Pixel",
                    Description = "Old and nasty.",
                    ProductEan = "14059292",
                    BrandId = 2,
                    BrandName = "Gugle",
                    Price = 2,
                    TotalPrice = 90,
                    PurchaseRequestStatus = PurchaseRequestStatus.PENDING
                },
                new PurchaseRequestDomainModel { 
                    PurchaseRequestID = 3, 
                    AccountName = "S. Mario", 
                    CardNumber = "4556711787875527", 
                    ProductId = 3, 
                    Quantity = 1243,
                    When = DateTime.Now,
                    Name = "Offbrand Nokia",
                    Description = "Old and sassy.",
                    ProductEan = "62592994",
                    BrandId = 3,
                    BrandName = "Brick",
                    Price = 1,
                    TotalPrice = 1243,
                    PurchaseRequestStatus = PurchaseRequestStatus.ACCEPTED
                },
                new PurchaseRequestDomainModel { 
                    PurchaseRequestID = 4, 
                    AccountName = "G. Bowser", 
                    CardNumber = "4539817512278671", 
                    ProductId = 4, 
                    Quantity = 23,
                    When = DateTime.Now,
                    Name = "Offbrand Blackburry",
                    Description = "Old and gassy.",
                    ProductEan = "16361652",
                    BrandId = 4,
                    BrandName = "Whiteburry",
                    Price = 3,
                    TotalPrice = 69,
                    PurchaseRequestStatus = PurchaseRequestStatus.DENIED
                },
                new PurchaseRequestDomainModel { 
                    PurchaseRequestID = 5, 
                    AccountName = "Yoshi", 
                    CardNumber = "4539919751889166", 
                    ProductId = 5, 
                    Quantity = 43,
                    When = DateTime.Now,
                    Name = "Offbrand Samsung",
                    Description = "Old and glassy.",
                    ProductEan = "53035172",
                    BrandId = 5,
                    BrandName = "Bobsung",
                    Price = 1,
                    TotalPrice = 43,
                    PurchaseRequestStatus = PurchaseRequestStatus.PENDING
                });
        }
    }
}
