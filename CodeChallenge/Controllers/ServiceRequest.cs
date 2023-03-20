using Microsoft.AspNetCore.Mvc;
using Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/servicerequest")]
    public class ServiceRequestController : ControllerBase
    {
        private List<ServiceRequest> _serviceRequests;

        public ServiceRequestController(List<ServiceRequest> _requests)
        {
            _serviceRequests = _requests;
        }

        [HttpGet]
        public ActionResult<IList<ServiceRequest>> Get()
        {
            if (_serviceRequests.Any())
            {
                return Ok(_serviceRequests);
            }

            return NoContent();
        }

        [HttpGet("{id}")]
        public ActionResult<ServiceRequest> Get(Guid id)
        {
            var serviceRequest = _serviceRequests.FirstOrDefault(r => r.Id == id);

            if (serviceRequest == null)
            {
                return NotFound();
            }

            return Ok(serviceRequest);
        }

        [HttpPost]
        public ActionResult<ServiceRequest> Create([FromBody] ServiceRequest serviceRequest)
        {
            if (serviceRequest == null || string.IsNullOrEmpty(serviceRequest.BuildingCode))
            {
                return BadRequest();
            }

            serviceRequest.Id = Guid.NewGuid();
            serviceRequest.CreatedDate = DateTime.UtcNow;
            serviceRequest.CurrentStatus = CurrentStatus.Created;

            _serviceRequests.Add(serviceRequest);

            return CreatedAtAction(nameof(Get), new { id = serviceRequest.Id }, serviceRequest);
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, ServiceRequest serviceRequest)
        {
            if (serviceRequest == null || id != serviceRequest.Id)
            {
                return BadRequest();
            }

            var existingServiceRequest = _serviceRequests.FirstOrDefault(r => r.Id == id);

            if (existingServiceRequest == null)
            {
                return NotFound();
            }

            existingServiceRequest.BuildingCode = serviceRequest.BuildingCode;
            existingServiceRequest.Description = serviceRequest.Description;
            existingServiceRequest.CurrentStatus = serviceRequest.CurrentStatus;
            existingServiceRequest.LastModifiedBy = serviceRequest.LastModifiedBy;
            existingServiceRequest.LastModifiedDate = DateTime.UtcNow;

            return Ok(existingServiceRequest);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var serviceRequest = _serviceRequests.FirstOrDefault(r => r.Id == id);

            if (serviceRequest == null)
            {
                return NotFound();
            }

            _serviceRequests.Remove(serviceRequest);

            return NoContent();
        }


    }

}
