using Microsoft.EntityFrameworkCore;
using PurchaseRequests.DomainModels;
using PurchaseRequests.Enums;
using PurchaseRequests.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseRequests.Repositories.Concrete
{
    public class SqlPurchaseRequestsRepository : IPurchaseRequestsRepository
    {
        private readonly Context.Context _context;

        public SqlPurchaseRequestsRepository(Context.Context context)
        {
            _context = context;
        }

        /// <summary>
        /// Create a purchase request.
        /// </summary>
        /// <param name="purchaseRequestDomainModel">Domain model of the purchase request to create.</param>
        /// <returns>The ID of the newly created purchase request.</returns>
        public int CreatePurchaseRequest(PurchaseRequestDomainModel purchaseRequestDomainModel)
        {
            purchaseRequestDomainModel.PurchaseRequestStatus = PurchaseRequestStatus.PENDING;
            return _context._purchaseRequests.Add(purchaseRequestDomainModel).Entity.PurchaseRequestID;
        }

        /// <summary>
        /// Get all the purchase requests.
        /// </summary>
        /// <returns>A task that resolves into a list of all purchase requests</returns>
        public async Task<IEnumerable<PurchaseRequestDomainModel>> GetAllPurchaseRequestsAsync()
        {
            return await _context._purchaseRequests.ToListAsync();
        }

        /// <summary>
        /// Get all pending purchase requests.
        /// </summary>
        /// <returns>A task that resolves into a list of all pending purchase requests.</returns>
        public async Task<IEnumerable<PurchaseRequestDomainModel>> GetAllPendingPurchaseRequestsAsync()
        {
            return await _context._purchaseRequests
                                    .Where(p => p.PurchaseRequestStatus == PurchaseRequestStatus.PENDING)
                                    .ToListAsync();
        }

        /// <summary>
        /// Get a purchase request.
        /// </summary>
        /// <param name="ID">ID of the purchase request to retrieve.</param>
        /// <returns>A task that resolves into the domain model of the purchase request found.</returns>
        public async Task<PurchaseRequestDomainModel> GetPurchaseRequestAsync(int ID)
        {
            return await _context._purchaseRequests.FirstOrDefaultAsync(o => o.PurchaseRequestID == ID);
        }

        /// <summary>
        /// Commit changes to database.
        /// </summary>
        /// <returns>Task that contains the result of the query.</returns>
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Update a purchase request.
        /// </summary>
        /// <param name="purchaseRequestDomainModel">Domain model containing new attributes.</param>
        public void UpdatePurchaseRequest(PurchaseRequestDomainModel purchaseRequestDomainModel)
        {
            if (purchaseRequestDomainModel == null)
                throw new ArgumentNullException(nameof(purchaseRequestDomainModel), "The product model to be updated cannot be null");
            _context._purchaseRequests.Update(purchaseRequestDomainModel);
        }
    }
}
