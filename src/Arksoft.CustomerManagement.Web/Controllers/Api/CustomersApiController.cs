using Arksoft.CustomerManagement.Application.Common.DTOs;
using Arksoft.CustomerManagement.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Arksoft.CustomerManagement.Web.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CustomersApiController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<CustomersApiController> _logger;

        public CustomersApiController(ICustomerService customerService, ILogger<CustomersApiController> logger)
        {
            _customerService = customerService;
            _logger = logger;
        }

        /// <summary>
        /// Gets a paginated list of customers with optional search filtering
        /// </summary>
        /// <param name="search">Optional search term to filter by name or VAT number</param>
        /// <param name="page">Page number (default: 1)</param>
        /// <param name="pageSize">Number of items per page (default: 10)</param>
        /// <returns>Paginated list of customers</returns>
        /// <response code="200">Returns the paginated customer list</response>
        /// <response code="401">If API key is missing or invalid</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCustomers([FromQuery] string? search = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                _logger.LogInformation("API: Getting customers - Search: {Search}, Page: {Page}, PageSize: {PageSize}", 
                    search, page, pageSize);

                var customers = await _customerService.GetCustomersAsync(page, pageSize, search);
                return Ok(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "API: Error getting customers");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets a specific customer by ID
        /// </summary>
        /// <param name="id">The customer ID</param>
        /// <returns>Customer details</returns>
        /// <response code="200">Returns the customer</response>
        /// <response code="401">If API key is missing or invalid</response>
        /// <response code="404">If customer is not found</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCustomer(int id)
        {
            try
            {
                _logger.LogInformation("API: Getting customer {CustomerId}", id);

                var customer = await _customerService.GetCustomerByIdAsync(id);
                if (customer == null)
                {
                    _logger.LogWarning("API: Customer {CustomerId} not found", id);
                    return NotFound($"Customer with ID {id} not found");
                }

                return Ok(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "API: Error getting customer {CustomerId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Creates a new customer
        /// </summary>
        /// <param name="customerDto">Customer creation data</param>
        /// <returns>Created customer details</returns>
        /// <response code="201">Customer created successfully</response>
        /// <response code="400">If the customer data is invalid</response>
        /// <response code="401">If API key is missing or invalid</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerDto customerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("API: Invalid model state for customer creation");
                    return BadRequest(ModelState);
                }

                _logger.LogInformation("API: Creating customer {CustomerName}", customerDto.Name);

                var createdCustomer = await _customerService.CreateCustomerAsync(customerDto);
                return CreatedAtAction(nameof(GetCustomer), new { id = createdCustomer.Id }, createdCustomer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "API: Error creating customer");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Updates an existing customer
        /// </summary>
        /// <param name="id">The customer ID to update</param>
        /// <param name="customerDto">Updated customer data</param>
        /// <returns>Updated customer details</returns>
        /// <response code="200">Customer updated successfully</response>
        /// <response code="400">If the customer data is invalid</response>
        /// <response code="401">If API key is missing or invalid</response>
        /// <response code="404">If customer is not found</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] UpdateCustomerDto customerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("API: Invalid model state for customer {CustomerId} update", id);
                    return BadRequest(ModelState);
                }

                _logger.LogInformation("API: Updating customer {CustomerId}", id);

                var updatedCustomer = await _customerService.UpdateCustomerAsync(id, customerDto);
                if (updatedCustomer == null)
                {
                    return NotFound($"Customer with ID {id} not found");
                }

                return Ok(updatedCustomer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "API: Error updating customer {CustomerId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Deletes a customer
        /// </summary>
        /// <param name="id">The customer ID to delete</param>
        /// <returns>No content if successful</returns>
        /// <response code="204">Customer deleted successfully</response>
        /// <response code="401">If API key is missing or invalid</response>
        /// <response code="404">If customer is not found</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {
                _logger.LogInformation("API: Deleting customer {CustomerId}", id);

                var result = await _customerService.DeleteCustomerAsync(id);
                if (!result)
                {
                    _logger.LogWarning("API: Customer {CustomerId} not found for deletion", id);
                    return NotFound($"Customer with ID {id} not found");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "API: Error deleting customer {CustomerId}", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}