using System;
using PurchaseRequests.DomainModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseRequests.Repositories.Interface
{
    public interface IPurchaseRequestsRepository
    {
        /// <summary>
        /// Get all the purchase requests.
        /// </summary>
        /// <returns>A task that resolves into a list of all purchase requests</returns>
        public Task<IEnumerable<PurchaseRequestDomainModel>> GetAllPurchaseRequestsAsync();
        /// <summary>
        /// Get all pending purchase requests.
        /// </summary>
        /// <returns>A task that resolves into a list of all pending purchase requests.</returns>
        public Task<IEnumerable<PurchaseRequestDomainModel>> GetAllPendingPurchaseRequestsAsync();
        /// <summary>
        /// Get a purchase request.
        /// </summary>
        /// <param name="ID">ID of the purchase request to retrieve.</param>
        /// <returns>A task that resolves into the domain model of the purchase request found.</returns>
        public Task<PurchaseRequestDomainModel> GetPurchaseRequestAsync(int ID);
        /// <summary>
        /// Create a purchase request.
        /// </summary>
        /// <param name="purchaseRequestDomainModel">Domain model of the purchase request to create.</param>
        /// <returns>The ID of the newly created purchase request.</returns>
        public int CreatePurchaseRequest(PurchaseRequestDomainModel purchaseRequestDomainModel);
        /// <summary>
        /// Update a purchase request.
        /// </summary>
        /// <param name="purchaseRequestDomainModel">Domain model containing new attributes.</param>
        public void UpdatePurchaseRequest(PurchaseRequestDomainModel purchaseRequestDomainModel);
        /// <summary>
        /// Commit changes to database.
        /// </summary>
        /// <returns>Task that contains the result of the query.</returns>
        public Task SaveChangesAsync();
    }
}
