using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Update;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using PurchaseRequests.DomainModels;
using PurchaseRequests.Context;
using PurchaseRequests.Enums;
using PurchaseRequests.Repositories.Concrete;

namespace PurchaseRequestsTests
{
    public class PurchaseRequestSqlRepoTest
    {
        public PurchaseRequestSqlRepoTest() { }

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


        private Mock<Context> GetDbContext()
        {
            var context = new Mock<Context>();
            context.Object.AddRange(GetTestPurchaseRequests());
            context.Object.SaveChanges();

            return context;
        }

        private Mock<DbSet<PurchaseRequestDomainModel>> GetMockDbSet()
        {
            return GetTestPurchaseRequests().AsQueryable().BuildMockDbSet();
        }


        [Fact]
        public async void GetAllPurchaseRequestsAsync_ShouldReturnListOfPurchaseRequests()
        {
            //Arrange
            var dbContextMock = GetDbContext();
            var dbSetMock = GetMockDbSet();
            dbContextMock.SetupGet(c => c._purchaseRequests).Returns(dbSetMock.Object);
            var sqlPurchaseRequestsCRUDRepository = new SqlPurchaseRequestsRepository(dbContextMock.Object);
            var expectedResult = GetTestPurchaseRequests();

            //Act
            var result = await sqlPurchaseRequestsCRUDRepository.GetAllPurchaseRequestsAsync();

            //Assert
            Assert.NotNull(result);
            var actionResult = Assert.IsType<List<PurchaseRequestDomainModel>>(result);
            var model = Assert.IsAssignableFrom<List<PurchaseRequestDomainModel>>(actionResult);
        }

        [Fact]
        public async void GetAllPurchaseRequestsAsync_ShouldReturnAllPurchaseRequests()
        {
            //Arrange
            var dbContextMock = GetDbContext();
            var dbSetMock = GetMockDbSet();
            dbContextMock.SetupGet(c => c._purchaseRequests).Returns(dbSetMock.Object);
            var sqlPurchaseRequestsCRUDRepository = new SqlPurchaseRequestsRepository(dbContextMock.Object);
            var expectedResult = GetTestPurchaseRequests();

            //Act
            var result = await sqlPurchaseRequestsCRUDRepository.GetAllPurchaseRequestsAsync();

            //Assert
            Assert.NotNull(result);
            var actionResult = Assert.IsType<List<PurchaseRequestDomainModel>>(result);
            var model = Assert.IsAssignableFrom<List<PurchaseRequestDomainModel>>(actionResult);
            Assert.Equal(expectedResult.Count(), model.Count());
        }

        [Fact]
        public async void GetAllPurchaseRequestsAsync_ShouldReturnAllCorrectPurchaseRequests()
        {
            //Arrange
            var dbContextMock = GetDbContext();
            var dbSetMock = GetMockDbSet();
            dbContextMock.SetupGet(c => c._purchaseRequests).Returns(dbSetMock.Object);
            var sqlPurchaseRequestsCRUDRepository = new SqlPurchaseRequestsRepository(dbContextMock.Object);
            var expectedResult = GetTestPurchaseRequests();

            //Act
            var result = await sqlPurchaseRequestsCRUDRepository.GetAllPurchaseRequestsAsync();

            //Assert
            Assert.NotNull(result);
            var actionResult = Assert.IsType<List<PurchaseRequestDomainModel>>(result);
            var model = Assert.IsAssignableFrom<List<PurchaseRequestDomainModel>>(actionResult);
            Assert.Equal(expectedResult.Count(), model.Count());
            model.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void GetAllPurchaseRequestsAsync_WhenNoPurchaseRequests_ShouldReturnEmptyList()
        {
            //Arrange
            var dbContextMock = GetDbContext();
            var dbSetMock = new PurchaseRequestDomainModel[0].AsQueryable().BuildMockDbSet();
            dbContextMock.SetupGet(c => c._purchaseRequests).Returns(dbSetMock.Object);
            var sqlPurchaseRequestsCRUDRepository = new SqlPurchaseRequestsRepository(dbContextMock.Object);
            var expectedResult = new PurchaseRequestDomainModel[0];

            //Act
            var result = await sqlPurchaseRequestsCRUDRepository.GetAllPurchaseRequestsAsync();

            //Assert
            Assert.NotNull(result);
            var actionResult = Assert.IsType<List<PurchaseRequestDomainModel>>(result);
            var model = Assert.IsAssignableFrom<List<PurchaseRequestDomainModel>>(actionResult);
            Assert.Equal(expectedResult.Count(), model.Count());
        }

        [Fact]
        public async void GetAllPurchaseRequestsAsync_ShouldReturnListOfPendingPurchaseRequests()
        {
            //Arrange
            var dbContextMock = GetDbContext();
            var dbSetMock = GetMockDbSet();
            dbContextMock.SetupGet(c => c._purchaseRequests).Returns(dbSetMock.Object);
            var sqlPurchaseRequestsCRUDRepository = new SqlPurchaseRequestsRepository(dbContextMock.Object);

            //Act
            var result = await sqlPurchaseRequestsCRUDRepository.GetAllPurchaseRequestsAsync();

            //Assert
            Assert.NotNull(result);
            var actionResult = Assert.IsType<List<PurchaseRequestDomainModel>>(result);
            var model = Assert.IsAssignableFrom<List<PurchaseRequestDomainModel>>(actionResult);
        }

        [Fact]
        public async void GetAllPurchaseRequestsAsync_WhenNoPendingPurchaseRequests_ShouldReturnEmptyList()
        {
            //Arrange
            var dbContextMock = GetDbContext();
            var dbSetMock = new PurchaseRequestDomainModel[0].AsQueryable().BuildMockDbSet();
            dbContextMock.SetupGet(c => c._purchaseRequests).Returns(dbSetMock.Object);
            var sqlPurchaseRequestsCRUDRepository = new SqlPurchaseRequestsRepository(dbContextMock.Object);
            var expectedResult = new PurchaseRequestDomainModel[0];

            //Act
            var result = await sqlPurchaseRequestsCRUDRepository.GetAllPurchaseRequestsAsync();

            //Assert
            Assert.NotNull(result);
            var actionResult = Assert.IsType<List<PurchaseRequestDomainModel>>(result);
            var model = Assert.IsAssignableFrom<List<PurchaseRequestDomainModel>>(actionResult);
            Assert.Equal(expectedResult.Count(), model.Count());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(4)]
        public async void GetPurchaseRequestAsync_ShouldReturnPurchaseRequestModel(int ID)
        {
            //Arrange
            var dbContextMock = GetDbContext();
            var dbSetMock = GetMockDbSet();
            dbContextMock.SetupGet(c => c._purchaseRequests).Returns(dbSetMock.Object);
            var sqlPurchaseRequestsCRUDRepository = new SqlPurchaseRequestsRepository(dbContextMock.Object);
            var expectedResult = GetTestPurchaseRequests().FirstOrDefault(dr => dr.PurchaseRequestID == ID);

            //Act
            var result = await sqlPurchaseRequestsCRUDRepository.GetPurchaseRequestAsync(ID);

            //Assert
            Assert.NotNull(result);
            var actionResult = Assert.IsType<PurchaseRequestDomainModel>(result);
            var model = Assert.IsAssignableFrom<PurchaseRequestDomainModel>(actionResult);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(4)]
        public async void GetPurchaseRequestAsync_ShouldReturnPurchaseRequest(int ID)
        {
            //Arrange
            var dbContextMock = GetDbContext();
            var dbSetMock = GetMockDbSet();
            dbContextMock.SetupGet(c => c._purchaseRequests).Returns(dbSetMock.Object);
            var sqlPurchaseRequestsCRUDRepository = new SqlPurchaseRequestsRepository(dbContextMock.Object);
            var expectedResult = GetTestPurchaseRequests().FirstOrDefault(dr => dr.PurchaseRequestID == ID);

            //Act
            var result = await sqlPurchaseRequestsCRUDRepository.GetPurchaseRequestAsync(ID);

            //Assert
            Assert.NotNull(result);
            var actionResult = Assert.IsType<PurchaseRequestDomainModel>(result);
            var model = Assert.IsAssignableFrom<PurchaseRequestDomainModel>(actionResult);
            model.Should().BeEquivalentTo(expectedResult);
        }

        [Theory]
        [InlineData(6)]
        [InlineData(-5)]
        [InlineData(int.MaxValue)]
        public async void GetPurchaseRequestAsync_WhenNotFound_ReturnsNull(int ID)
        {
            //Arrange
            var dbContextMock = GetDbContext();
            var dbSetMock = GetMockDbSet();
            dbContextMock.SetupGet(c => c._purchaseRequests).Returns(dbSetMock.Object);
            var sqlPurchaseRequestsCRUDRepository = new SqlPurchaseRequestsRepository(dbContextMock.Object);

            //Act
            var result = await sqlPurchaseRequestsCRUDRepository.GetPurchaseRequestAsync(ID);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async void SaveChangesAsync_ShouldSaveChanges()
        {
            //Arrange
            var dbContextMock = GetDbContext();
            var dbSetMock = GetMockDbSet();
            dbContextMock.SetupGet(c => c._purchaseRequests).Returns(dbSetMock.Object);
            dbContextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                         .Verifiable();
            var sqlPurchaseRequestsCRUDRepository = new SqlPurchaseRequestsRepository(dbContextMock.Object);

            //Act
            await sqlPurchaseRequestsCRUDRepository.SaveChangesAsync();

            //Assert
            dbContextMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void UpdatePurchaseRequest_WhenNullPassed_ThrowsArgumentNull()
        {
            //Arrange
            var dbContextMock = GetDbContext();
            var dbSetMock = GetMockDbSet();
            dbContextMock.SetupGet(c => c._purchaseRequests).Returns(dbSetMock.Object);
            var sqlPurchaseRequestsCRUDRepository = new SqlPurchaseRequestsRepository(dbContextMock.Object);

            //Act
            Assert.Throws<ArgumentNullException>(() => sqlPurchaseRequestsCRUDRepository.UpdatePurchaseRequest(null));
        }

        public static IEnumerable<object[]> SplitUpdateData =>
        new List<object[]>
        {
            new object[] {new PurchaseRequestDomainModel {PurchaseRequestID = 0, PurchaseRequestStatus = PurchaseRequestStatus.PENDING} },
            new object[] {new PurchaseRequestDomainModel {PurchaseRequestID = 1, PurchaseRequestStatus = PurchaseRequestStatus.ACCEPTED } },
            new object[] {new PurchaseRequestDomainModel { PurchaseRequestID = 2, PurchaseRequestStatus = PurchaseRequestStatus.DENIED } }
        };
    }
}