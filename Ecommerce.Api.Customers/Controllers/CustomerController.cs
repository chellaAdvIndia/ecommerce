using Ecommerce.Api.Customers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Ecommerce.Api.Customers.Controllers
{
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerProvider customerProvider;

        public CustomerController(ICustomerProvider customerProvider) 
        {
            this.customerProvider = customerProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomersAsync()
        {
            var result = await this.customerProvider.GetCustomerAsync();
            if(result.IsSuccess)
            {
                return Ok(result.customer);
            }
            return NotFound();

        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetCustomerAsync(int Id)
        {
            var result = await this.customerProvider.GetCustomerAsync(Id);
            if(result.IsSuccess)
            {
                return Ok(result.customer);
            }

            return NotFound();
        }
    }
}
