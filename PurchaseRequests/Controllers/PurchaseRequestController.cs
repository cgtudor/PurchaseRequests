using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using PurchaseRequests.AutomatedCacher.Model;
using PurchaseRequests.DomainModels;
using PurchaseRequests.DTOs;
using PurchaseRequests.Enums;
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
        private readonly IMemoryCache _memoryCache;
        private readonly MemoryCacheModel _memoryCacheModel;

        public PurchaseRequestController(IPurchaseRequestsRepository purchaseRequestsRepository, IMapper mapper, IMemoryCache memoryCache,
            IOptions<MemoryCacheModel> memoryCacheModel)
        {
            _purchaseRequestsRepository = purchaseRequestsRepository;
            _mapper = mapper;
            _memoryCache = memoryCache;
            _memoryCacheModel = memoryCacheModel.Value;
        }

        /// <summary>
        /// GET all purchase requests.
        /// /api/purchase-requests
        /// </summary>
        /// <returns>A list of all purchase requests.</returns>
        /// <response code="200">Retrieval of the purchase requests was successful</response>
        /// <response code="400">Bad request.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="403">Lack of required permissions.</response>
        [HttpGet]
        [Authorize("ReadPurchaseRequests")]
        public async Task<ActionResult<IEnumerable<PurchaseRequestReadDTO>>> GetAllPurchaseRequests()
        {
            // If the cache is loaded, we retrieve them from the cache.
            if (_memoryCache.TryGetValue(_memoryCacheModel.PurchaseRequests, out List<PurchaseRequestDomainModel> purchaseRequestValues))
                return Ok(_mapper.Map<IEnumerable<PurchaseRequestReadDTO>>(purchaseRequestValues));

            var purchaseRequestsDomainModels = await _purchaseRequestsRepository.GetAllPurchaseRequestsAsync();
            return Ok(_mapper.Map<IEnumerable<PurchaseRequestReadDTO>>(purchaseRequestsDomainModels));
        }

        /// <summary>
        /// GET all pending purchase requests.
        /// /api/purchase-requests/pending
        /// </summary>
        /// <returns>A list of all pending purchase requests.</returns>
        /// <response code="200">Retrieval of the pending purchase requests was successful</response>
        /// <response code="400">Bad request.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="403">Lack of required permissions.</response>
        [HttpGet]
        [Route("pending")]
        [Authorize("ReadPendingPurchaseRequests")]
        public async Task<ActionResult<IEnumerable<PurchaseRequestReadDTO>>> GetAllPendingPurchaseRequests()
        {
            // If the cache is loaded, we retrieve the requests from the cache.
            if (_memoryCache.TryGetValue(_memoryCacheModel.PurchaseRequests, out List<PurchaseRequestDomainModel> purchaseRequestValues))
            {
                return Ok(_mapper.Map<IEnumerable<PurchaseRequestReadDTO>>(purchaseRequestValues.Where(p => p.PurchaseRequestStatus == PurchaseRequestStatus.PENDING)
                                                                                                .AsEnumerable()));
            }

            var purchaseRequestsDomainModels = await _purchaseRequestsRepository.GetAllPendingPurchaseRequestsAsync();
            return Ok(_mapper.Map<IEnumerable<PurchaseRequestReadDTO>>(purchaseRequestsDomainModels));
        }

        /// <summary>
        /// GET an individual purchase request.
        /// /api/purchase-requests/{id}
        /// </summary>
        /// <param name="ID">Represents the purchase request ID and is used to get a specific purchase request.</param>
        /// <returns>The purchase request found.</returns>
        /// <response code="200">Retrieval of the requested purchase request was successful</response>
        /// <response code="400">Bad request.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="403">Lack of required permissions.</response>
        /// <response code="404">No purchase request was found with the provided ID.</response>
        [HttpGet("{ID}")]
        [Authorize("ReadPurchaseRequest")]
        [ActionName(nameof(GetPurchaseRequest))]
        public async Task<ActionResult<PurchaseRequestReadDTO>> GetPurchaseRequest(int ID)
        {
            PurchaseRequestDomainModel purchaseRequestDomainModel;
            // If cache exists and we find the entity.
            if (_memoryCache.TryGetValue(_memoryCacheModel.PurchaseRequests, out List<PurchaseRequestDomainModel> purchaseRequestValues))
            {
                // Return the entity if we find it in the cache.
                purchaseRequestDomainModel = purchaseRequestValues.Find(o => o.PurchaseRequestID == ID);
                if (purchaseRequestDomainModel != null)
                    return Ok(_mapper.Map<PurchaseRequestReadDTO>(purchaseRequestDomainModel));

                // Otherwise, get the entity from the DB, add it to the cache and return it.
                purchaseRequestDomainModel = await _purchaseRequestsRepository.GetPurchaseRequestAsync(ID);
                if (purchaseRequestDomainModel != null)
                {
                    purchaseRequestValues.Add(purchaseRequestDomainModel);
                    return Ok(_mapper.Map<PurchaseRequestReadDTO>(purchaseRequestDomainModel));
                }

                throw new ResourceNotFoundException("A resource for ID: " + ID + " does not exist.");
            }

            purchaseRequestDomainModel = await _purchaseRequestsRepository.GetPurchaseRequestAsync(ID);

            if (purchaseRequestDomainModel != null)
                return Ok(_mapper.Map<PurchaseRequestReadDTO>(purchaseRequestDomainModel));

            throw new ResourceNotFoundException();
        }

        /// <summary>
        /// POST information to create a purchase request
        /// /api/purchase-requests
        /// </summary>
        /// <param name="purchaseRequestCreateDTO">The properties supplied to create a purchase request.</param>
        /// <returns>CreatedAtAction containing the endpoint to get the new purchase request, its ID and its model.</returns>
        /// <response code="200">Creation of the purchase request was successful</response>
        /// <response code="400">Bad request.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="403">Lack of required permissions.</response>
        [HttpPost]
        [Authorize("CreatePurchaseRequest")]
        public async Task<ActionResult> CreatePurchaseRequest([FromBody] PurchaseRequestCreateDTO purchaseRequestCreateDTO)
        {
            var purchaseRequestModel = _mapper.Map<PurchaseRequestDomainModel>(purchaseRequestCreateDTO);

            int newProductID = _purchaseRequestsRepository.CreatePurchaseRequest(purchaseRequestModel);

            await _purchaseRequestsRepository.SaveChangesAsync();

            // We add the new request created to the cache as well.
            if (_memoryCache.TryGetValue(_memoryCacheModel.PurchaseRequests, out List<PurchaseRequestDomainModel> purchaseRequestValues))
                purchaseRequestValues.Add(purchaseRequestModel);

            return CreatedAtAction(nameof(GetPurchaseRequest), new { ID = newProductID }, purchaseRequestModel);
        }

        /// <summary>
        /// This function will update a purchase request with the passed in parameters.
        /// </summary>
        /// <param name="ID">The ID of the purchase request that will be updated.</param>
        /// <param name="purchaseRequestEditPatch">The json object containing the patch details</param>
        /// <returns></returns>
        /// <response code="200">Update of the requested purchase request was successful</response>
        /// <response code="400">Bad request.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="403">Lack of required permissions.</response>
        /// <response code="404">No purchase request was found with the provided ID.</response>
        [HttpPatch("{ID}")]
        [Authorize("UpdatePurchaseRequest")]
        public async Task<ActionResult> UpdatePurchaseRequest(int ID, JsonPatchDocument<PurchaseRequestEditDTO> purchaseRequestEditPatch)
        {
            var purchaseRequestModel = await _purchaseRequestsRepository.GetPurchaseRequestAsync(ID);
            if (purchaseRequestModel == null)
                throw new ResourceNotFoundException();

            var newProduct = _mapper.Map<PurchaseRequestEditDTO>(purchaseRequestModel);
            purchaseRequestEditPatch.ApplyTo(newProduct, ModelState);

            if (!TryValidateModel(newProduct))
                throw new ArgumentException();

            _mapper.Map(newProduct, purchaseRequestModel);

            _purchaseRequestsRepository.UpdatePurchaseRequest(purchaseRequestModel);
            await _purchaseRequestsRepository.SaveChangesAsync();

            // We update the cache with the new information
            if (_memoryCache.TryGetValue(_memoryCacheModel.PurchaseRequests, out List<PurchaseRequestDomainModel> purchaseRequestValues))
            {
                purchaseRequestValues.RemoveAll(o => o.PurchaseRequestID == purchaseRequestModel.PurchaseRequestID);
                purchaseRequestValues.Add(purchaseRequestModel);
            }

            return Ok();
        }

    }
}
