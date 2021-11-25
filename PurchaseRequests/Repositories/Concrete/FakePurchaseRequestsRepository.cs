using PurchaseRequests.DomainModels;
using PurchaseRequests.Repositories.Interface;
using PurchaseRequests.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseRequests.Repositories.Concrete
{
    public class FakePurchaseRequestsRepository : IPurchaseRequestsRepository
    {
        public List<PurchaseRequestDomainModel> _purchaseRequests = new List<PurchaseRequestDomainModel>
        {
                new PurchaseRequestDomainModel { PurchaseRequestID = 1,
                    AccountName = "Test John Doe",
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
                    AccountName = "Test Johnny Silverhand",
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
                    AccountName = "Test S. Mario",
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
                    AccountName = "Test G. Bowser",
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
                    AccountName = "Test Yoshi",
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
                }
        };
        public int CreatePurchaseRequest(PurchaseRequestDomainModel purchaseRequestDomainModel)
        {
            int newPurchaseRequestID = (_purchaseRequests.Count);
            purchaseRequestDomainModel.PurchaseRequestID = newPurchaseRequestID;
            purchaseRequestDomainModel.PurchaseRequestStatus = PurchaseRequestStatus.PENDING;
            _purchaseRequests.Add(purchaseRequestDomainModel);

            return newPurchaseRequestID;
        }

        public Task<IEnumerable<PurchaseRequestDomainModel>> GetAllPurchaseRequestsAsync()
        {
            return Task.FromResult(_purchaseRequests.AsEnumerable());
        }

        public Task<PurchaseRequestDomainModel> GetPurchaseRequestAsync(int ID)
        {
            return Task.FromResult(_purchaseRequests.FirstOrDefault(o => o.PurchaseRequestID == ID));
        }

        public Task SaveChangesAsync()
        {
            return Task.CompletedTask;
        }

        public void UpdatePurchaseRequest(PurchaseRequestDomainModel purchaseRequestDomainModel)
        {
            var oldPurchaseRequestDomainModel = _purchaseRequests.FirstOrDefault(o => o.PurchaseRequestID == purchaseRequestDomainModel.PurchaseRequestID);
            _purchaseRequests.Remove(oldPurchaseRequestDomainModel);
            _purchaseRequests.Add(purchaseRequestDomainModel);
        }
    }
}
