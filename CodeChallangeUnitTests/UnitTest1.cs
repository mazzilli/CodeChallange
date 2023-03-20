using CodeChallenge.Controllers;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace CodeChallangeUnitTests
{
    [TestClass]
    public class UnitTest1
    {
        private ServiceRequestController _controller;
        private List<ServiceRequest> serviceRequests = new()
        {
            new ServiceRequest() { Id = Guid.NewGuid(), BuildingCode = "B001", Description = "Description 1", CurrentStatus = CurrentStatus.Created, CreatedDate = DateTime.UtcNow },
            new ServiceRequest() { Id = Guid.NewGuid(), BuildingCode = "B002", Description = "Description 2", CurrentStatus = CurrentStatus.InProgress, CreatedDate = DateTime.UtcNow.AddDays(-1) },
            new ServiceRequest() { Id = Guid.NewGuid(), BuildingCode = "B003", Description = "Description 3", CurrentStatus = CurrentStatus.Complete, CreatedDate = DateTime.UtcNow.AddDays(-2) },
        };

        public UnitTest1()
        {
            _controller = new ServiceRequestController(serviceRequests);
        }

        [TestMethod]
        public void Get_ReturnsOkResult_WhenServiceRequestsExist()
        {
            // Act
            var result = _controller.Get();

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }


        [TestMethod]
        public void Get_ReturnsNoContentResult_WhenServiceRequestsDoNotExist()
        {
            // Arrange
            _controller = new ServiceRequestController(new List<ServiceRequest>());

            // Act
            var result = _controller.Get();

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(NoContentResult));
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

        }



        [TestMethod]
        public void Create_AddsNewServiceRequest_WhenInputIsValid()
        {
            // Arrange            
            var serviceRequest = new ServiceRequest { CreatedBy = "Me", CreatedDate = DateTime.Now, BuildingCode = "ABC123" };

            // Act
            var result = _controller.Create(serviceRequest);

            // Assert

            Assert.AreEqual(4, serviceRequests.Count);
            Assert.AreEqual(serviceRequest, serviceRequests[3]);
        }


        [TestMethod]
        public void Create_SetsNewServiceRequestProperties()
        {
            // Arrange
            var serviceRequest = new ServiceRequest { BuildingCode = "ABC123", Description = "Test" };

            // Act
            var result = _controller.Create(serviceRequest).Result as CreatedAtActionResult;

            // Assert
            Assert.IsNotNull(result);
            var createdServiceRequest = result.Value as ServiceRequest;
            Assert.IsNotNull(createdServiceRequest);
            Assert.AreEqual(serviceRequest.BuildingCode, createdServiceRequest.BuildingCode);
            Assert.AreEqual(serviceRequest.Description, createdServiceRequest.Description);
            Assert.AreEqual(CurrentStatus.Created, createdServiceRequest.CurrentStatus);
            Assert.IsNotNull(createdServiceRequest.Id);
            Assert.IsTrue(createdServiceRequest.CreatedDate > DateTime.MinValue);
        }

        [TestMethod]
        public void Update_ValidRequest_ReturnsOk()
        {
            // Arrange
            var id = serviceRequests.First().Id;
            var updatedServiceRequest = new ServiceRequest
            {
                Id = id,
                BuildingCode = "B",
                Description = "Service Request B",
                CurrentStatus = CurrentStatus.InProgress,
                LastModifiedBy = "User B"
            };

            // Act
            var result = _controller.Update(id, updatedServiceRequest);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var updatedServiceRequestResult = (ServiceRequest)((OkObjectResult)result).Value;
            Assert.AreEqual(updatedServiceRequest.BuildingCode, updatedServiceRequestResult.BuildingCode);
            Assert.AreEqual(updatedServiceRequest.Description, updatedServiceRequestResult.Description);
            Assert.AreEqual(updatedServiceRequest.CurrentStatus, updatedServiceRequestResult.CurrentStatus);
            Assert.AreEqual(updatedServiceRequest.LastModifiedBy, updatedServiceRequestResult.LastModifiedBy);
            Assert.AreEqual(DateTime.UtcNow.Date, updatedServiceRequestResult.LastModifiedDate.Date);
        }

        [TestMethod]
        public void Update_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var updatedServiceRequest = new ServiceRequest
            {
                Id = id,
                BuildingCode = "B",
                Description = "Service Request B",
                CurrentStatus = CurrentStatus.InProgress,
                LastModifiedBy = "User B"
            };

            // Act
            var result = _controller.Update(id, updatedServiceRequest);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void Update_NullRequest_ReturnsBadRequest()
        {
            // Arrange
            var id = serviceRequests.First().Id;

            // Act
            var result = _controller.Update(id, null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void Update_InvalidRequest_ReturnsBadRequest()
        {
            // Arrange
            var id = serviceRequests.First().Id;
            var updatedServiceRequest = new ServiceRequest
            {
                Id = Guid.NewGuid(),
                BuildingCode = "B",
                Description = "Service Request B",
                CurrentStatus = CurrentStatus.InProgress,
                LastModifiedBy = "User B"
            };

            // Act
            var result = _controller.Update(id, updatedServiceRequest);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }


        [TestMethod]
        public void Delete_ExistingServiceRequest_ReturnsNoContent()
        {
            // Arrange
            int counter = serviceRequests.Count;

            // Act
            var result = _controller.Delete(serviceRequests[0].Id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            Assert.AreEqual(counter-1, serviceRequests.Count);
        }

        [TestMethod]
        public void Delete_NonExistingServiceRequest_ReturnsNotFound()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();
            var count = serviceRequests.Count;

            // Act
            var result = _controller.Delete(nonExistingId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            Assert.AreEqual(count, serviceRequests.Count);
        }



    }
}