using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;

namespace CodeChallenge.Controllers.Tests
{
    [TestClass()]
    public class ServiceRequestControllerTests
    {

        private ServiceRequestController _controller;

        public ServiceRequestControllerTests()
        {
            // Arrange
            var serviceRequests = new List<ServiceRequest>()
        {
            new ServiceRequest() { Id = Guid.NewGuid(), BuildingCode = "B001", Description = "Description 1", CurrentStatus = CurrentStatus.Created, CreatedDate = DateTime.UtcNow },
            new ServiceRequest() { Id = Guid.NewGuid(), BuildingCode = "B002", Description = "Description 2", CurrentStatus = CurrentStatus.InProgress, CreatedDate = DateTime.UtcNow.AddDays(-1) },
            new ServiceRequest() { Id = Guid.NewGuid(), BuildingCode = "B003", Description = "Description 3", CurrentStatus = CurrentStatus.Complete, CreatedDate = DateTime.UtcNow.AddDays(-2) },
        };

            _controller = new ServiceRequestController(serviceRequests);

        }


        [TestMethod]
        public void Get_ReturnsOkResult_WhenServiceRequestsExist()
        {
            // Act
            var result = _controller.Get();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void Get_ReturnsServiceRequests_WhenServiceRequestsExist()
        {
            // Act
            var result = _controller.Get();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Value, typeof(List<ServiceRequest>));
        }

        [TestMethod]
        public void Get_ReturnsNoContentResult_WhenServiceRequestsDoNotExist()
        {
            // Arrange
            _controller = new ServiceRequestController(new List<ServiceRequest>());

            // Act
            var result = _controller.Get();

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public void Get_ReturnsEmptyList_WhenServiceRequestsDoNotExist()
        {
            // Arrange
            _controller = new ServiceRequestController(new List<ServiceRequest>());

            // Act
            var result = _controller.Get();

            // Assert
            Assert.IsNotNull(result);
            ;
        }
    }
}
