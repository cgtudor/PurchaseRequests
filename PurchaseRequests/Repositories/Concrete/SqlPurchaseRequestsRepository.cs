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

        public int CreatePurchaseRequest(PurchaseRequestDomainModel purchaseRequestDomainModel)
        {
            purchaseRequestDomainModel.PurchaseRequestStatus = PurchaseRequestStatus.PENDING;
            return _context._purchaseRequests.Add(purchaseRequestDomainModel).Entity.PurchaseRequestID;
        }

        public async Task<IEnumerable<PurchaseRequestDomainModel>> GetAllPurchaseRequestsAsync()
        {
            return await _context._purchaseRequests.ToListAsync();
        }

        public async Task<PurchaseRequestDomainModel> GetPurchaseRequestAsync(int ID)
        {
            return await _context._purchaseRequests.FirstOrDefaultAsync(o => o.PurchaseRequestID == ID);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void UpdatePurchaseRequest(PurchaseRequestDomainModel purchaseRequestDomainModel)
        {
            
        }
    }
}
