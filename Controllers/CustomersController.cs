using CustomerManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using CustomerManagement.Helper;

namespace CustomerManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private static List<Customer> customers = new List<Customer>();
        public CustomersController()
        {
            customers = CustomersHelper.GetCustomersFromFile();
        }

        // POST api/customers
        [HttpPost]
        public IActionResult Post([FromBody] List<Customer> newCustomers)
        {
            if (newCustomers == null || !newCustomers.Any())
            {
                return BadRequest("Invalid request");
            }

            var validCustomers = new List<Customer>();

            foreach (var customer in newCustomers)
            {
                if (CustomersHelper.ValidateCustomer(customer, customers))
                {
                    validCustomers.Add(customer);
                    CustomersHelper.InsertCustomer(customer, customers);
                }
            }

            CustomersHelper.SaveCustomersToFile(customers);

            return StatusCode(201, new { Message = "Customers added successfully", Customers = validCustomers });
        }

        // GET api/customers
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(customers);
        }
    }
}
