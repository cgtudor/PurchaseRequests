using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PurchaseRequests.DomainModels;
using PurchaseRequests.DTOs;
using PurchaseRequests.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseRequests.Controllers
{
    [Route("api/purchase-requests")]
    [ApiController]
    public class PurchaseRequestController : ControllerBase
    {
        private IPurchaseRequestsRepository _purchaseRequestsRepository;
        private IMapper _mapper;

        public PurchaseRequestController(IPurchaseRequestsRepository purchaseRequestsRepository, IMapper mapper)
        {
            _purchaseRequestsRepository = purchaseRequestsRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// GET all purchase requests.
        /// /api/purchase-requests
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<PurchaseRequestReadDTO>>> GetAllPurchaseRequests()
        {
            var purchaseRequestsDomainModels = await _purchaseRequestsRepository.GetAllPurchaseRequestsAsync();

            return Ok(_mapper.Map<IEnumerable<PurchaseRequestReadDTO>>(purchaseRequestsDomainModels));
        }

        /// <summary>
        /// GET individual purchase request.
        /// /api/purchase-requests/{id}
        /// </summary>
        /// <param name="ID">Represents the purchase request ID and is used to get a specific purchase request.</param>
        /// <returns></returns>
        [HttpGet("{ID}")]
        [Authorize]
        [ActionName(nameof(GetPurchaseRequest))]
        public async Task<ActionResult<PurchaseRequestReadDTO>> GetPurchaseRequest(int ID)
        {
            var purchaseRequestDomainModel = await _purchaseRequestsRepository.GetPurchaseRequestAsync(ID);

            if (purchaseRequestDomainModel != null)
                return Ok(_mapper.Map<PurchaseRequestReadDTO>(purchaseRequestDomainModel));

            return NotFound();
        }

        /// <summary>
        /// This function is used to create a purchase request.
        /// /api/purchase-requests
        /// </summary>
        /// <param name="purchaseRequestCreateDTO">The properties supplied to create a purchase request from the POSTing API.</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> CreateProduct([FromBody] PurchaseRequestCreateDTO purchaseRequestCreateDTO)
        {
            var purchaseRequestModel = _mapper.Map<PurchaseRequestDomainModel>(purchaseRequestCreateDTO);

            int newProductID = _purchaseRequestsRepository.CreatePurchaseRequest(purchaseRequestModel);

            await _purchaseRequestsRepository.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPurchaseRequest), new { ID = newProductID }, purchaseRequestModel);
        }

        /// <summary>
        /// This function will update a purchase request with the passed in parameters.
        /// </summary>
        /// <param name="ID">The ID of the purchase request that will be updated.</param>
        /// <returns></returns>
        /// <response code="200">Patching of the purchase request was successful</response>
        /// <response code="401">Unauthorized access.</response>
        [HttpPatch("{ID}")]
        [Authorize]
        public async Task<ActionResult> UpdateOrder(int ID, JsonPatchDocument<PurchaseRequestEditDTO> purchaseRequestEditPatch)
        {
            var purchaseRequestModel = await _purchaseRequestsRepository.GetPurchaseRequestAsync(ID);
            if (purchaseRequestModel == null)
                return NotFound();

            var newProduct = _mapper.Map<PurchaseRequestEditDTO>(purchaseRequestModel);
            purchaseRequestEditPatch.ApplyTo(newProduct, ModelState);

            if (!TryValidateModel(newProduct))
                return ValidationProblem(ModelState);

            _mapper.Map(newProduct, purchaseRequestModel);

            _purchaseRequestsRepository.UpdatePurchaseRequest(purchaseRequestModel);
            await _purchaseRequestsRepository.SaveChangesAsync();

            return Ok();
        }

    }
}
