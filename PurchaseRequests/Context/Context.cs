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
        virtual public DbSet<PurchaseRequestDomainModel> _purchaseRequests { get; set; }

        public Context(DbContextOptions<Context> options) : base(options)
        {

        }

        public Context() { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        }
    }
}
