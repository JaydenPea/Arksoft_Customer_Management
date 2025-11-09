using Arksoft.CustomerManagement.Application.Common.DTOs;
using Arksoft.CustomerManagement.Application.Common.Interfaces;
using Arksoft.CustomerManagement.Application.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Arksoft.CustomerManagement.Web.Controllers;

public class CustomerController : Controller
{
    private readonly ICustomerService _customerService;
    private readonly IValidator<CreateCustomerDto> _createValidator;
    private readonly IValidator<UpdateCustomerDto> _updateValidator;
    private readonly ILogger<CustomerController> _logger;

    public CustomerController(
        ICustomerService customerService,
        IValidator<CreateCustomerDto> createValidator,
        IValidator<UpdateCustomerDto> updateValidator,
        ILogger<CustomerController> logger)
    {
        _customerService = customerService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _logger = logger;
    }

    // GET: /Customer - List View with sorting, paging, filtering
    public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string? search = null, string? sortBy = null, bool sortDesc = false)
    {
        try
        {
            _logger.LogInformation("Fetching customers - Page: {Page}, Search: {Search}, Sort: {SortBy}", page, search, sortBy);

            var customers = await _customerService.GetCustomersAsync(page, pageSize, search, sortBy, sortDesc);
            
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.Search = search;
            ViewBag.SortBy = sortBy;
            ViewBag.SortDesc = sortDesc;
            
            return View(customers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching customers");
            TempData["Error"] = "An error occurred while loading customers. Please try again.";
            return View(new PagedResult<CustomerListDto>());
        }
    }

    // GET: /Customer/Details/5
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                _logger.LogWarning("Customer with ID {CustomerId} not found", id);
                TempData["Error"] = "Customer not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(customer);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching customer {CustomerId}", id);
            TempData["Error"] = "An error occurred while loading customer details.";
            return RedirectToAction(nameof(Index));
        }
    }

    // GET: /Customer/Create - Management Page for Adding
    public IActionResult Create()
    {
        return View(new CreateCustomerDto());
    }

    // POST: /Customer/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateCustomerDto createCustomerDto)
    {
        try
        {
            // Validate using FluentValidation
            var validationResult = await _createValidator.ValidateAsync(createCustomerDto);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(createCustomerDto);
            }

            var customer = await _customerService.CreateCustomerAsync(createCustomerDto);
            
            _logger.LogInformation("Customer created successfully with ID {CustomerId}", customer.Id);
            TempData["Success"] = $"Customer '{customer.Name}' has been created successfully.";
            
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating customer");
            TempData["Error"] = "An error occurred while creating the customer. Please try again.";
            return View(createCustomerDto);
        }
    }

    // GET: /Customer/Edit/5 - Management Page for Editing
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                _logger.LogWarning("Customer with ID {CustomerId} not found for editing", id);
                TempData["Error"] = "Customer not found.";
                return RedirectToAction(nameof(Index));
            }

            var updateDto = new UpdateCustomerDto
            {
                Name = customer.Name,
                Address = customer.Address,
                TelephoneNumber = customer.TelephoneNumber,
                ContactPersonName = customer.ContactPersonName,
                ContactPersonEmail = customer.ContactPersonEmail,
                VatNumber = customer.VatNumber
            };

            ViewBag.CustomerId = id;
            return View(updateDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading customer {CustomerId} for editing", id);
            TempData["Error"] = "An error occurred while loading customer for editing.";
            return RedirectToAction(nameof(Index));
        }
    }

    // POST: /Customer/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateCustomerDto updateCustomerDto)
    {
        try
        {
            // Validate using FluentValidation
            var validationResult = await _updateValidator.ValidateAsync(updateCustomerDto);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                ViewBag.CustomerId = id;
                return View(updateCustomerDto);
            }

            var customer = await _customerService.UpdateCustomerAsync(id, updateCustomerDto);
            
            _logger.LogInformation("Customer {CustomerId} updated successfully", id);
            TempData["Success"] = $"Customer '{customer.Name}' has been updated successfully.";
            
            return RedirectToAction(nameof(Index));
        }
        catch (KeyNotFoundException)
        {
            _logger.LogWarning("Customer with ID {CustomerId} not found for update", id);
            TempData["Error"] = "Customer not found.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating customer {CustomerId}", id);
            TempData["Error"] = "An error occurred while updating the customer. Please try again.";
            ViewBag.CustomerId = id;
            return View(updateCustomerDto);
        }
    }

    // POST: /Customer/Delete/5 - Delete with confirmation
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var success = await _customerService.DeleteCustomerAsync(id);
            
            if (success)
            {
                _logger.LogInformation("Customer {CustomerId} deleted successfully", id);
                TempData["Success"] = "Customer has been deleted successfully.";
            }
            else
            {
                _logger.LogWarning("Customer with ID {CustomerId} not found for deletion", id);
                TempData["Error"] = "Customer not found.";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting customer {CustomerId}", id);
            TempData["Error"] = "An error occurred while deleting the customer. Please try again.";
        }

        return RedirectToAction(nameof(Index));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error");
    }
}