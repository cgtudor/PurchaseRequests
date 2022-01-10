using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using PurchaseRequests.AutomatedCacher.Model;
using PurchaseRequests.Controllers;
using PurchaseRequests.DomainModels;
using PurchaseRequests.DTOs;
using PurchaseRequests.Enums;
using PurchaseRequests.Profiles;
using PurchaseRequests.Repositories.Interface;
using PurchaseRequestsTests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PurchaseRequestsTests
{
    public class PurchaseRequestControllerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<IMemoryCache> _memoryCacheMock;
        private readonly IOptions<MemoryCacheModel> _memoryCacheModel;

        private PurchaseRequestDomainModel[] GetTestPurchaseRequests() => new PurchaseRequestDomainModel[]
        {
                new PurchaseRequestDomainModel { 
                    PurchaseRequestID = 1,
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
                    Quantity = 1243,
                    Name = "Offbrand Nokia",
                    PurchaseRequestStatus = PurchaseRequestStatus.ACCEPTED
                },
                new PurchaseRequestDomainModel {
                    PurchaseRequestID = 4,
                    Quantity = 23,
                    Name = "Offbrand Blackburry",
                    PurchaseRequestStatus = PurchaseRequestStatus.DENIED
                },
                new PurchaseRequestDomainModel {
                    PurchaseRequestID = 5,
                    Quantity = 43,
                    Name = "Offbrand Samsung",
                    PurchaseRequestStatus = PurchaseRequestStatus.PENDING
                }
        };

        public PurchaseRequestControllerTest()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new PurchaseRequestProfile());
            });

            _memoryCacheMock = new Mock<IMemoryCache>();
            object expectedValue = null;
            _memoryCacheMock
                .Setup(x => x.TryGetValue(It.IsAny<object>(), out expectedValue))
                .Returns(false);
            var options = Options.Create(new MemoryCacheModel());
            _memoryCacheModel = options;
            _mapper = config.CreateMapper();

        }

        [Fact]
        public async Task GetAll_WhenCalled_ReturnsAllPurchaseRequests()
        {
            // Arrange

            var expected = GetTestPurchaseRequests();
            var mockPurchaseRequests = new Mock<IPurchaseRequestsRepository>(MockBehavior.Strict);
            mockPurchaseRequests.Setup(r => r.GetAllPurchaseRequestsAsync())
                .ReturnsAsync(expected)
                .Verifiable();

            var controller = new PurchaseRequestController(mockPurchaseRequests.Object, _mapper, _memoryCacheMock.Object, _memoryCacheModel);

            // Act
            var result = await controller.GetAllPurchaseRequests();

            // Assert
            var viewResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsAssignableFrom<IEnumerable<PurchaseRequestReadDTO>>(
                viewResult.Value);
            Assert.Equal(expected.Length, model.Count());
            mockPurchaseRequests.Verify(r => r.GetAllPurchaseRequestsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAll_WhenCalled_ReturnsCorrectPurchaseRequests()
        {
            // Arrange

            var expected = GetTestPurchaseRequests();
            var mockPurchaseRequests = new Mock<IPurchaseRequestsRepository>(MockBehavior.Strict);
            mockPurchaseRequests.Setup(r => r.GetAllPurchaseRequestsAsync())
                .ReturnsAsync(expected)
                .Verifiable();
            var controller = new PurchaseRequestController(mockPurchaseRequests.Object, _mapper, _memoryCacheMock.Object, _memoryCacheModel);

            // Act
            var result = await controller.GetAllPurchaseRequests();

            // Assert
            var viewResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsAssignableFrom<IEnumerable<PurchaseRequestReadDTO>>(
                viewResult.Value);
            model.Should().BeEquivalentTo(expected);
            mockPurchaseRequests.Verify(r => r.GetAllPurchaseRequestsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAll_WhenCalled_ReturnsOkResult()
        {
            // Arrange

            var expected = GetTestPurchaseRequests();
            var mockPurchaseRequests = new Mock<IPurchaseRequestsRepository>(MockBehavior.Strict);
            mockPurchaseRequests.Setup(r => r.GetAllPurchaseRequestsAsync())
                .ReturnsAsync(expected)
                .Verifiable();
            var controller = new PurchaseRequestController(mockPurchaseRequests.Object, _mapper, _memoryCacheMock.Object, _memoryCacheModel);

            // Act
            var result = await controller.GetAllPurchaseRequests();

            // Assert
            var viewResult = Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetAll_WhenCalledWithNoPurchaseRequests_ReturnsNoPurchaseRequests()
        {
            // Arrange
            var expected = new PurchaseRequestDomainModel[] { };
            var mockPurchaseRequests = new Mock<IPurchaseRequestsRepository>(MockBehavior.Strict);
            mockPurchaseRequests.Setup(r => r.GetAllPurchaseRequestsAsync())
                .ReturnsAsync(expected)
                .Verifiable();
            var controller = new PurchaseRequestController(mockPurchaseRequests.Object, _mapper, _memoryCacheMock.Object, _memoryCacheModel);

            // Act
            var result = await controller.GetAllPurchaseRequests();

            // Assert
            var viewResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsAssignableFrom<IEnumerable<PurchaseRequestReadDTO>>(
                viewResult.Value);
            Assert.Equal(expected.Length, model.Count());
            mockPurchaseRequests.Verify(r => r.GetAllPurchaseRequestsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAll_WhenBadServiceCall_ShouldInternalError()
        {
            // Arrange
            var mockPurchaseRequests = new Mock<IPurchaseRequestsRepository>(MockBehavior.Strict);
            mockPurchaseRequests.Setup(r => r.GetAllPurchaseRequestsAsync())
                .ThrowsAsync(new Exception())
                .Verifiable();
            var controller = new PurchaseRequestController(mockPurchaseRequests.Object, _mapper, _memoryCacheMock.Object, _memoryCacheModel);

            await Assert.ThrowsAsync<Exception>(async () => await controller.GetAllPurchaseRequests());
        }

        [Fact]
        public async Task GetOne_WhenCalled_ReturnsPurchaseRequest()
        {
            // Arrange

            var expected = new PurchaseRequestDomainModel
            {
                PurchaseRequestID = 2,
                Quantity = 45,
                Name = "Offbrand Google Pixel",
                PurchaseRequestStatus = PurchaseRequestStatus.PENDING
            };
            var mockPurchaseRequests = new Mock<IPurchaseRequestsRepository>(MockBehavior.Strict);
            mockPurchaseRequests.Setup(r => r.GetPurchaseRequestAsync(2))
                        .ReturnsAsync(expected)
                        .Verifiable();

            var controller = new PurchaseRequestController(mockPurchaseRequests.Object, _mapper, _memoryCacheMock.Object, _memoryCacheModel);

            // Act
            var result = await controller.GetPurchaseRequest(2);

            // Assert
            var viewResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsAssignableFrom<PurchaseRequestReadDTO>(
                viewResult.Value);
            mockPurchaseRequests.Verify(r => r.GetPurchaseRequestAsync(2), Times.Once);
        }

        [Fact]
        public async Task GetOne_WhenCalled_ReturnsCorrectPurchaseRequest()
        {
            // Arrange
            var expected = new PurchaseRequestDomainModel
            {
                PurchaseRequestID = 2,
                Quantity = 45,
                Name = "Offbrand Google Pixel",
                PurchaseRequestStatus = PurchaseRequestStatus.PENDING
            };
            var mockPurchaseRequests = new Mock<IPurchaseRequestsRepository>(MockBehavior.Strict);
            mockPurchaseRequests.Setup(r => r.GetPurchaseRequestAsync(2))
                        .ReturnsAsync(expected)
                        .Verifiable();

            var controller = new PurchaseRequestController(mockPurchaseRequests.Object, _mapper, _memoryCacheMock.Object, _memoryCacheModel);

            // Act
            var result = await controller.GetPurchaseRequest(2);

            // Assert
            var viewResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsAssignableFrom<PurchaseRequestReadDTO>(
                viewResult.Value);

            model.Should().BeEquivalentTo(expected);
            mockPurchaseRequests.Verify(r => r.GetPurchaseRequestAsync(2), Times.Once);
        }

        [Fact]
        public async Task GetOne_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            var expected = new PurchaseRequestDomainModel
            {
                PurchaseRequestID = 2,
                Quantity = 45,
                Name = "Offbrand Google Pixel",
                PurchaseRequestStatus = PurchaseRequestStatus.PENDING
            };
            var mockPurchaseRequests = new Mock<IPurchaseRequestsRepository>(MockBehavior.Strict);
            mockPurchaseRequests.Setup(r => r.GetPurchaseRequestAsync(2))
                        .ReturnsAsync(expected)
                        .Verifiable();

            var controller = new PurchaseRequestController(mockPurchaseRequests.Object, _mapper, _memoryCacheMock.Object, _memoryCacheModel);

            // Act
            var result = await controller.GetPurchaseRequest(2);

            // Assert
            var viewResult = Assert.IsType<OkObjectResult>(result.Result);
            mockPurchaseRequests.Verify(r => r.GetPurchaseRequestAsync(2), Times.Once);
        }

        [Fact]
        public async Task GetOne_WhenCalledWithNoPurchaseRequests_ThrowsNotFound()
        {
            // Arrange

            var expected = new PurchaseRequestDomainModel
            {
                PurchaseRequestID = 2,
                Quantity = 45,
                Name = "Offbrand Google Pixel",
                PurchaseRequestStatus = PurchaseRequestStatus.PENDING
            };
            var mockPurchaseRequests = new Mock<IPurchaseRequestsRepository>(MockBehavior.Strict);
            mockPurchaseRequests.Setup(r => r.GetPurchaseRequestAsync(2))
                        .ReturnsAsync((PurchaseRequestDomainModel)null)
                        .Verifiable();

            var controller = new PurchaseRequestController(mockPurchaseRequests.Object, _mapper, _memoryCacheMock.Object, _memoryCacheModel);

            // Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(async () => await controller.GetPurchaseRequest(2));
        }

        [Fact]
        public async Task GetOne_WhenBadServiceCall_ShouldInternalError()
        {
            // Arrange

            var expected = new PurchaseRequestDomainModel
            {
                PurchaseRequestID = 2,
                Quantity = 45,
                Name = "Offbrand Google Pixel",
                PurchaseRequestStatus = PurchaseRequestStatus.PENDING
            };
            var mockPurchaseRequests = new Mock<IPurchaseRequestsRepository>(MockBehavior.Strict);
            mockPurchaseRequests.Setup(r => r.GetPurchaseRequestAsync(2))
                        .ThrowsAsync(new Exception())
                        .Verifiable();

            var controller = new PurchaseRequestController(mockPurchaseRequests.Object, _mapper, _memoryCacheMock.Object, _memoryCacheModel);

            // Assert
            await Assert.ThrowsAsync<Exception>(async () => await controller.GetPurchaseRequest(2));
        }

        [Fact]
        public async Task Create_WhenCalled_CreatesPurchaseRequest()
        {
            // Arrange

            var createDTO = new PurchaseRequestCreateDTO
            {
                Quantity = 45,
                Name = "Offbrand Google Pixel"
            };
            var mockPurchaseRequests = new Mock<IPurchaseRequestsRepository>(MockBehavior.Strict);
            mockPurchaseRequests.Setup(r => r.CreatePurchaseRequest(It.IsAny<PurchaseRequestDomainModel>()))
                        .Returns(2)
                        .Verifiable();

            mockPurchaseRequests.Setup(r => r.SaveChangesAsync())
                        .Returns(Task.CompletedTask)
                        .Verifiable();

            var controller = new PurchaseRequestController(mockPurchaseRequests.Object, _mapper, _memoryCacheMock.Object, _memoryCacheModel);

            // Act
            var result = await controller.CreatePurchaseRequest(createDTO);

            // Assert
            var viewResult = Assert.IsType<CreatedAtActionResult>(result);
            var model = Assert.IsAssignableFrom<PurchaseRequestDomainModel>(
                viewResult.Value);
            mockPurchaseRequests.Verify(r => r.CreatePurchaseRequest(It.IsAny<PurchaseRequestDomainModel>()), Times.Once);
            mockPurchaseRequests.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Create_WhenCalled_CreatesCorrectPurchaseRequest()
        {
            // Arrange

            var createDTO = new PurchaseRequestCreateDTO
            {
                ProductId = 2,
                Quantity = 45,
                Name = "Offbrand Google Pixel"
            };
            var expectedAfterCreation = new PurchaseRequestDomainModel
            {
                PurchaseRequestID = 0,
                ProductId = 2,
                Quantity = 45,
                Name = "Offbrand Google Pixel",
            };
            var mockPurchaseRequests = new Mock<IPurchaseRequestsRepository>(MockBehavior.Strict);
            mockPurchaseRequests.Setup(r => r.CreatePurchaseRequest(It.IsAny<PurchaseRequestDomainModel>()))
                        .Returns(2)
                        .Verifiable();

            mockPurchaseRequests.Setup(r => r.SaveChangesAsync())
                        .Returns(Task.CompletedTask)
                        .Verifiable();

            var controller = new PurchaseRequestController(mockPurchaseRequests.Object, _mapper, _memoryCacheMock.Object, _memoryCacheModel);

            // Act
            var result = await controller.CreatePurchaseRequest(createDTO);

            // Assert
            var viewResult = Assert.IsType<CreatedAtActionResult>(result);
            var model = Assert.IsAssignableFrom<PurchaseRequestDomainModel>(
                viewResult.Value);

            model.Should().BeEquivalentTo(expectedAfterCreation);

            mockPurchaseRequests.Verify(r => r.CreatePurchaseRequest(It.IsAny<PurchaseRequestDomainModel>()), Times.Once);
            mockPurchaseRequests.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Create_WhenCalled_ReturnsCreatedAtActionResult()
        {
            // Arrange

            var createDTO = new PurchaseRequestCreateDTO
            {
                Quantity = 45,
                Name = "Offbrand Google Pixel"
            };
            var mockPurchaseRequests = new Mock<IPurchaseRequestsRepository>(MockBehavior.Strict);
            mockPurchaseRequests.Setup(r => r.CreatePurchaseRequest(It.IsAny<PurchaseRequestDomainModel>()))
                        .Returns(2)
                        .Verifiable();

            mockPurchaseRequests.Setup(r => r.SaveChangesAsync())
                        .Returns(Task.CompletedTask)
                        .Verifiable();

            var controller = new PurchaseRequestController(mockPurchaseRequests.Object, _mapper, _memoryCacheMock.Object, _memoryCacheModel);

            // Act
            var result = await controller.CreatePurchaseRequest(createDTO);

            // Assert
            var viewResult = Assert.IsType<CreatedAtActionResult>(result);

            mockPurchaseRequests.Verify(r => r.CreatePurchaseRequest(It.IsAny<PurchaseRequestDomainModel>()), Times.Once);
            mockPurchaseRequests.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Create_WhenCalled_ReturnsCorrectID()
        {
            // Arrange

            var createDTO = new PurchaseRequestCreateDTO
            {
                Quantity = 45,
                Name = "Offbrand Google Pixel"
            };
            var mockPurchaseRequests = new Mock<IPurchaseRequestsRepository>(MockBehavior.Strict);
            mockPurchaseRequests.Setup(r => r.CreatePurchaseRequest(It.IsAny<PurchaseRequestDomainModel>()))
                        .Returns(2)
                        .Verifiable();

            mockPurchaseRequests.Setup(r => r.SaveChangesAsync())
                        .Returns(Task.CompletedTask)
                        .Verifiable();

            var controller = new PurchaseRequestController(mockPurchaseRequests.Object, _mapper, _memoryCacheMock.Object, _memoryCacheModel);

            // Act
            var result = await controller.CreatePurchaseRequest(createDTO);

            // Assert
            var viewResult = Assert.IsType<CreatedAtActionResult>(result);

            var resultRouteValue = Assert.IsType<RouteValueDictionary>(viewResult.RouteValues);

            var resultID = Assert.IsType<int>(resultRouteValue.FirstOrDefault().Value);

            Assert.Equal(2, resultID);

            mockPurchaseRequests.Verify(r => r.CreatePurchaseRequest(It.IsAny<PurchaseRequestDomainModel>()), Times.Once);
            mockPurchaseRequests.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Create_WhenCalled_ReturnsCorrectActionName()
        {
            // Arrange

            var createDTO = new PurchaseRequestCreateDTO
            {
                Quantity = 45,
                Name = "Offbrand Google Pixel"
            }; 
            var mockPurchaseRequests = new Mock<IPurchaseRequestsRepository>(MockBehavior.Strict);
            mockPurchaseRequests.Setup(r => r.CreatePurchaseRequest(It.IsAny<PurchaseRequestDomainModel>()))
                        .Returns(2)
                        .Verifiable();

            mockPurchaseRequests.Setup(r => r.SaveChangesAsync())
                        .Returns(Task.CompletedTask)
                        .Verifiable();

            var controller = new PurchaseRequestController(mockPurchaseRequests.Object, _mapper, _memoryCacheMock.Object, _memoryCacheModel);

            // Act
            var result = await controller.CreatePurchaseRequest(createDTO);

            // Assert
            var viewResult = Assert.IsType<CreatedAtActionResult>(result);

            var resultActionName = Assert.IsType<string>(viewResult.ActionName);

            Assert.Equal("GetPurchaseRequest", resultActionName);

            mockPurchaseRequests.Verify(r => r.CreatePurchaseRequest(It.IsAny<PurchaseRequestDomainModel>()), Times.Once);
            mockPurchaseRequests.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Create_WhenBadServiceCall_ShouldInternalError()
        {
            // Arrange
            var createDTO = new PurchaseRequestCreateDTO
            {
                Quantity = 45,
                Name = "Offbrand Google Pixel"
            }; 
            var mockPurchaseRequests = new Mock<IPurchaseRequestsRepository>(MockBehavior.Strict);
            mockPurchaseRequests.Setup(r => r.CreatePurchaseRequest(It.IsAny<PurchaseRequestDomainModel>()))
                        .Throws(new Exception())
                        .Verifiable();

            mockPurchaseRequests.Setup(r => r.SaveChangesAsync())
                        .Returns(Task.CompletedTask)
                        .Verifiable();
            var controller = new PurchaseRequestController(mockPurchaseRequests.Object, _mapper, _memoryCacheMock.Object, _memoryCacheModel);

            await Assert.ThrowsAsync<Exception>(async () => await controller.CreatePurchaseRequest(createDTO));
        }

        [Fact]
        public async Task Update_WhenCalled_UpdatesPurchaseRequest()
        {
            // Arrange
            var updateDto = new PurchaseRequestEditDTO { PurchaseRequestStatus = PurchaseRequestStatus.ACCEPTED };
            JsonPatchDocument<PurchaseRequestEditDTO> jsonPatchDocument = new JsonPatchDocument<PurchaseRequestEditDTO>();
            jsonPatchDocument.Replace<PurchaseRequestStatus>(jp => jp.PurchaseRequestStatus, updateDto.PurchaseRequestStatus);
            var originalModel = new PurchaseRequestDomainModel
            {
                PurchaseRequestID = 2,
                Quantity = 45,
                Name = "Offbrand Google Pixel",
                PurchaseRequestStatus = PurchaseRequestStatus.PENDING
            };
            var mockPurchaseRequests = new Mock<IPurchaseRequestsRepository>(MockBehavior.Strict);
            mockPurchaseRequests.Setup(r => r.UpdatePurchaseRequest(It.IsAny<PurchaseRequestDomainModel>()))
                        .Verifiable();

            mockPurchaseRequests.Setup(r => r.SaveChangesAsync())
                        .Returns(Task.CompletedTask)
                        .Verifiable();

            mockPurchaseRequests.Setup(r => r.GetPurchaseRequestAsync(2))
                        .ReturnsAsync(originalModel)
                        .Verifiable();

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));

            var controller = new PurchaseRequestController(mockPurchaseRequests.Object, _mapper, _memoryCacheMock.Object, _memoryCacheModel);

            controller.ObjectValidator = objectValidator.Object;

            // Act
            var result = await controller.UpdatePurchaseRequest(2, jsonPatchDocument);

            // Assert
            var viewResult = Assert.IsType<OkResult>(result);
            mockPurchaseRequests.Verify(r => r.GetPurchaseRequestAsync(It.IsAny<int>()), Times.Once);
            mockPurchaseRequests.Verify(r => r.UpdatePurchaseRequest(It.IsAny<PurchaseRequestDomainModel>()), Times.Once);
            mockPurchaseRequests.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Update_WhenBadServiceCall_ShouldInternalError()
        {
            // Arrange

            var updateDto = new PurchaseRequestEditDTO { PurchaseRequestStatus = PurchaseRequestStatus.ACCEPTED };
            JsonPatchDocument<PurchaseRequestEditDTO> jsonPatchDocument = new JsonPatchDocument<PurchaseRequestEditDTO>();
            jsonPatchDocument.Replace<PurchaseRequestStatus>(jp => jp.PurchaseRequestStatus, updateDto.PurchaseRequestStatus);
            var originalModel = new PurchaseRequestDomainModel
            {
                PurchaseRequestID = 2,
                Quantity = 45,
                Name = "Offbrand Google Pixel",
                PurchaseRequestStatus = PurchaseRequestStatus.PENDING
            }; 
            var mockPurchaseRequests = new Mock<IPurchaseRequestsRepository>(MockBehavior.Strict);
            mockPurchaseRequests.Setup(r => r.UpdatePurchaseRequest(It.IsAny<PurchaseRequestDomainModel>()))
                        .Throws(new Exception())
                        .Verifiable();

            mockPurchaseRequests.Setup(r => r.GetPurchaseRequestAsync(2))
                        .ReturnsAsync(originalModel)
                        .Verifiable();

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));

            var controller = new PurchaseRequestController(mockPurchaseRequests.Object, _mapper, _memoryCacheMock.Object, _memoryCacheModel);

            controller.ObjectValidator = objectValidator.Object;

            // Act
            await Assert.ThrowsAsync<Exception>(async () => await controller.UpdatePurchaseRequest(2, jsonPatchDocument));

            mockPurchaseRequests.Verify(r => r.GetPurchaseRequestAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task Update_WhenCalledWithWrongID_ThrowsNotFound()
        {
            // Arrange

            var updateDto = new PurchaseRequestEditDTO { PurchaseRequestStatus = PurchaseRequestStatus.ACCEPTED };
            JsonPatchDocument<PurchaseRequestEditDTO> jsonPatchDocument = new JsonPatchDocument<PurchaseRequestEditDTO>();
            jsonPatchDocument.Replace<PurchaseRequestStatus>(jp => jp.PurchaseRequestStatus, updateDto.PurchaseRequestStatus);
            var mockPurchaseRequests = new Mock<IPurchaseRequestsRepository>(MockBehavior.Strict);

            mockPurchaseRequests.Setup(r => r.GetPurchaseRequestAsync(2))
                        .ReturnsAsync((PurchaseRequestDomainModel)null)
                        .Verifiable();

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));

            var controller = new PurchaseRequestController(mockPurchaseRequests.Object, _mapper, _memoryCacheMock.Object, _memoryCacheModel);

            controller.ObjectValidator = objectValidator.Object;

            // Act
            await Assert.ThrowsAsync<ResourceNotFoundException>(async () => await controller.UpdatePurchaseRequest(2, jsonPatchDocument));

            mockPurchaseRequests.Verify(r => r.GetPurchaseRequestAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task Update_WhenCalledWithModelStateError_ThrowsArgumentException()
        {
            // Arrange

            var updateDto = new PurchaseRequestEditDTO { PurchaseRequestStatus = PurchaseRequestStatus.ACCEPTED };
            JsonPatchDocument<PurchaseRequestEditDTO> jsonPatchDocument = new JsonPatchDocument<PurchaseRequestEditDTO>();
            jsonPatchDocument.Replace<PurchaseRequestStatus>(jp => jp.PurchaseRequestStatus, updateDto.PurchaseRequestStatus);
            var originalModel = new PurchaseRequestDomainModel
            {
                PurchaseRequestID = 2,
                Quantity = 45,
                Name = "Offbrand Google Pixel",
                PurchaseRequestStatus = PurchaseRequestStatus.PENDING
            }; 
            var mockPurchaseRequests = new Mock<IPurchaseRequestsRepository>(MockBehavior.Strict);

            mockPurchaseRequests.Setup(r => r.GetPurchaseRequestAsync(2))
                        .ReturnsAsync(originalModel)
                        .Verifiable();

            var controller = new PurchaseRequestController(mockPurchaseRequests.Object, _mapper, _memoryCacheMock.Object, _memoryCacheModel);

            controller.ObjectValidator = new FaultyValidator();

            // Act
            await Assert.ThrowsAsync<ArgumentException>(async () => await controller.UpdatePurchaseRequest(2, jsonPatchDocument));

            // Assert
            mockPurchaseRequests.Verify(r => r.GetPurchaseRequestAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetAllPending_WhenCalled_ReturnsAllPendingPurchaseRequests()
        {
            // Arrange

            var expected = GetTestPurchaseRequests().Where(p => p.PurchaseRequestStatus == PurchaseRequestStatus.PENDING);
            var mockPurchaseRequests = new Mock<IPurchaseRequestsRepository>(MockBehavior.Strict);
            mockPurchaseRequests.Setup(r => r.GetAllPurchaseRequestsAsync())
                .ReturnsAsync(expected)
                .Verifiable();

            var controller = new PurchaseRequestController(mockPurchaseRequests.Object, _mapper, _memoryCacheMock.Object, _memoryCacheModel);

            // Act
            var result = await controller.GetAllPurchaseRequests();

            // Assert
            var viewResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsAssignableFrom<IEnumerable<PurchaseRequestReadDTO>>(
                viewResult.Value);
            Assert.Equal(expected.Count(), model.Count());
            mockPurchaseRequests.Verify(r => r.GetAllPurchaseRequestsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllPending_WhenCalled_ReturnsCorrectPurchaseRequests()
        {
            // Arrange

            var expected = GetTestPurchaseRequests().Where(p => p.PurchaseRequestStatus == PurchaseRequestStatus.PENDING);
            var mockPurchaseRequests = new Mock<IPurchaseRequestsRepository>(MockBehavior.Strict);
            mockPurchaseRequests.Setup(r => r.GetAllPurchaseRequestsAsync())
                .ReturnsAsync(expected)
                .Verifiable();
            var controller = new PurchaseRequestController(mockPurchaseRequests.Object, _mapper, _memoryCacheMock.Object, _memoryCacheModel);

            // Act
            var result = await controller.GetAllPurchaseRequests();

            // Assert
            var viewResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsAssignableFrom<IEnumerable<PurchaseRequestReadDTO>>(
                viewResult.Value);
            model.Should().BeEquivalentTo(expected);
            mockPurchaseRequests.Verify(r => r.GetAllPurchaseRequestsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllPending_WhenCalled_ReturnsOkResult()
        {
            // Arrange

            var expected = GetTestPurchaseRequests().Where(p => p.PurchaseRequestStatus == PurchaseRequestStatus.PENDING);
            var mockPurchaseRequests = new Mock<IPurchaseRequestsRepository>(MockBehavior.Strict);
            mockPurchaseRequests.Setup(r => r.GetAllPurchaseRequestsAsync())
                .ReturnsAsync(expected)
                .Verifiable();
            var controller = new PurchaseRequestController(mockPurchaseRequests.Object, _mapper, _memoryCacheMock.Object, _memoryCacheModel);

            // Act
            var result = await controller.GetAllPurchaseRequests();

            // Assert
            var viewResult = Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetAllPending_WhenCalledWithNoPurchaseRequests_ReturnsNoPurchaseRequests()
        {
            // Arrange
            var expected = new PurchaseRequestDomainModel[] { };
            var mockPurchaseRequests = new Mock<IPurchaseRequestsRepository>(MockBehavior.Strict);
            mockPurchaseRequests.Setup(r => r.GetAllPurchaseRequestsAsync())
                .ReturnsAsync(expected)
                .Verifiable();
            var controller = new PurchaseRequestController(mockPurchaseRequests.Object, _mapper, _memoryCacheMock.Object, _memoryCacheModel);

            // Act
            var result = await controller.GetAllPurchaseRequests();

            // Assert
            var viewResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsAssignableFrom<IEnumerable<PurchaseRequestReadDTO>>(
                viewResult.Value);
            Assert.Equal(expected.Length, model.Count());
            mockPurchaseRequests.Verify(r => r.GetAllPurchaseRequestsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllPending_WhenBadServiceCall_ShouldInternalError()
        {
            // Arrange
            var mockPurchaseRequests = new Mock<IPurchaseRequestsRepository>(MockBehavior.Strict);
            mockPurchaseRequests.Setup(r => r.GetAllPurchaseRequestsAsync())
                .ThrowsAsync(new Exception())
                .Verifiable();
            var controller = new PurchaseRequestController(mockPurchaseRequests.Object, _mapper, _memoryCacheMock.Object, _memoryCacheModel);

            await Assert.ThrowsAsync<Exception>(async () => await controller.GetAllPurchaseRequests());
        }
    }
}
