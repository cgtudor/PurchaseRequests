using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using PurchaseRequests.AutomatedCacher.Interface;
using PurchaseRequests.AutomatedCacher.Model;
using PurchaseRequests.DomainModels;
using PurchaseRequests.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProductsCRUD.AutomatedCacher.Concrete
{
    public class MemoryCacheAutomater : IMemoryCacheAutomater
    {
        private readonly IPurchaseRequestsRepository _purchaseRequestsRepository;
        private readonly IMemoryCache _memoryCache;
        private readonly MemoryCacheModel _memoryCacheModel;

        public MemoryCacheAutomater(IServiceScopeFactory serviceProvider, IMemoryCache memoryCache, IOptions<MemoryCacheModel> memoryCacheModel)
        {
            _purchaseRequestsRepository = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IPurchaseRequestsRepository>();
            _memoryCache = memoryCache;
            _memoryCacheModel = memoryCacheModel.Value;
        }

        public void AutomateCache()
        {
            RegisterCache(_memoryCacheModel.PurchaseRequests, null, EvictionReason.None, null);
        }

        private MemoryCacheEntryOptions GetMemoryCacheEntryOptions()
        {
            int cacheExpirationMinutes = 1;
            DateTime cacheExpirationTime = DateTime.Now.AddMinutes(cacheExpirationMinutes);
            CancellationChangeToken cacheExpirationToken = new CancellationChangeToken
            (
                new CancellationTokenSource(TimeSpan.FromMinutes(cacheExpirationMinutes + 0.01)).Token
            );

            return new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(cacheExpirationTime)
                .SetPriority(CacheItemPriority.NeverRemove)
                .AddExpirationToken(cacheExpirationToken)
                .RegisterPostEvictionCallback(callback: RegisterCache, state: this);
        }

        private async void RegisterCache(object key, object value, EvictionReason reason, object state)
        {
            IEnumerable<PurchaseRequestDomainModel> purchaseRequestDomainModels = await _purchaseRequestsRepository.GetAllPurchaseRequestsAsync();
            _memoryCache.Set(key, purchaseRequestDomainModels, GetMemoryCacheEntryOptions());
        }
    }
}
