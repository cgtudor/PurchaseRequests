using AutoMapper;
using PurchaseRequests.DomainModels;
using PurchaseRequests.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseRequests.Profiles
{
    public class PurchaseRequestProfile : Profile
    {
        public PurchaseRequestProfile()
        {
            CreateMap<PurchaseRequestDomainModel, PurchaseRequestReadDTO>();
            CreateMap<PurchaseRequestCreateDTO, PurchaseRequestDomainModel>();
            CreateMap<PurchaseRequestDomainModel, PurchaseRequestEditDTO>();
            CreateMap<PurchaseRequestEditDTO, PurchaseRequestDomainModel>();
        }
    }
}
