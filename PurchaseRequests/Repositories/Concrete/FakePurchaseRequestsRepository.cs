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
                    ProductId = 1,
                    Quantity = 4,
                    Name = "Offbrand IPhone",
                    PurchaseRequestStatus = PurchaseRequestStatus.PENDING
                },
                new PurchaseRequestDomainModel {
                    PurchaseRequestID = 2,
                    Quantity = 45,
                    Name = "Offbrand Google Pixel",
                    PurchaseRequestStatus = PurchaseRequestStatus.PENDING
                },
                new PurchaseRequestDomainModel {
                    PurchaseRequestID = 3,
                    ProductId = 3,
                    Quantity = 1243,
                    Name = "Offbrand Nokia",
                    PurchaseRequestStatus = PurchaseRequestStatus.ACCEPTED
                },
                new PurchaseRequestDomainModel {
                    PurchaseRequestID = 4,
                    ProductId = 4,
                    Quantity = 23,
                    Name = "Offbrand Blackburry",
                    PurchaseRequestStatus = PurchaseRequestStatus.DENIED
                },
                new PurchaseRequestDomainModel {
                    PurchaseRequestID = 5,
                    ProductId = 5,
                    Quantity = 43,
                    Name = "Offbrand Samsung",
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
        public Task<IEnumerable<PurchaseRequestDomainModel>> GetAllPendingPurchaseRequestsAsync()
        {
            return Task.FromResult(_purchaseRequests
                                    .Where(p => p.PurchaseRequestStatus == PurchaseRequestStatus.PENDING)
                                    .AsEnumerable());
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
