using System;
using PurchaseRequests.DomainModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseRequests.Repositories.Interface
{
    public interface IPurchaseRequestsRepository
    {
        public Task<IEnumerable<PurchaseRequestDomainModel>> GetAllPurchaseRequestsAsync();
        public Task<PurchaseRequestDomainModel> GetPurchaseRequestAsync(int ID);
        public int CreatePurchaseRequest(PurchaseRequestDomainModel purchaseRequestDomainModel);
        public void UpdatePurchaseRequest(PurchaseRequestDomainModel purchaseRequestDomainModel);
        public Task SaveChangesAsync();
    }
}
