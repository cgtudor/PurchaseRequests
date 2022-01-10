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

        /// <summary>
        /// Create a purchase request.
        /// </summary>
        /// <param name="purchaseRequestDomainModel">Domain model of the purchase request to create.</param>
        /// <returns>The ID of the newly created purchase request.</returns>
        public int CreatePurchaseRequest(PurchaseRequestDomainModel purchaseRequestDomainModel)
        {
            int newPurchaseRequestID = (_purchaseRequests.Count);
            purchaseRequestDomainModel.PurchaseRequestID = newPurchaseRequestID;
            purchaseRequestDomainModel.PurchaseRequestStatus = PurchaseRequestStatus.PENDING;
            _purchaseRequests.Add(purchaseRequestDomainModel);

            return newPurchaseRequestID;
        }

        /// <summary>
        /// Get all the purchase requests.
        /// </summary>
        /// <returns>A task that resolves into a list of all purchase requests</returns>
        public Task<IEnumerable<PurchaseRequestDomainModel>> GetAllPurchaseRequestsAsync()
        {
            return Task.FromResult(_purchaseRequests.AsEnumerable());
        }

        /// <summary>
        /// Get all pending purchase requests.
        /// </summary>
        /// <returns>A task that resolves into a list of all pending purchase requests.</returns>
        public Task<IEnumerable<PurchaseRequestDomainModel>> GetAllPendingPurchaseRequestsAsync()
        {
            return Task.FromResult(_purchaseRequests
                                    .Where(p => p.PurchaseRequestStatus == PurchaseRequestStatus.PENDING)
                                    .AsEnumerable());
        }

        /// <summary>
        /// Get a purchase request.
        /// </summary>
        /// <param name="ID">ID of the purchase request to retrieve.</param>
        /// <returns>A task that resolves into the domain model of the purchase request found.</returns>
        public Task<PurchaseRequestDomainModel> GetPurchaseRequestAsync(int ID)
        {
            return Task.FromResult(_purchaseRequests.FirstOrDefault(o => o.PurchaseRequestID == ID));
        }

        /// <summary>
        /// Commit changes to database.
        /// </summary>
        /// <returns>Task that contains the result of the query.</returns>
        public Task SaveChangesAsync()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Update a purchase request.
        /// </summary>
        /// <param name="purchaseRequestDomainModel">Domain model containing new attributes.</param>
        public void UpdatePurchaseRequest(PurchaseRequestDomainModel purchaseRequestDomainModel)
        {
            var oldPurchaseRequestDomainModel = _purchaseRequests.FirstOrDefault(o => o.PurchaseRequestID == purchaseRequestDomainModel.PurchaseRequestID);
            _purchaseRequests.Remove(oldPurchaseRequestDomainModel);
            _purchaseRequests.Add(purchaseRequestDomainModel);
        }
    }
}
