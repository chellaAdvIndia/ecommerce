using AutoMapper;
using Ecommerce.Api.Customers.Db;
using Ecommerce.Api.Customers.Interfaces;
using Ecommerce.Api.Customers.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Api.Customers.Providers
{
    public class CustomerProvider : ICustomerProvider
    {
        private readonly CustomersDbContext customersDbContext;
        private readonly ILogger<CustomerProvider> logger;
        private readonly IMapper mapper;

        public CustomerProvider(CustomersDbContext customersDbContext, ILogger<CustomerProvider> logger, IMapper mapper) 
        {
            this.customersDbContext = customersDbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }

        private void SeedData()
        {
            if(!customersDbContext.Customers.Any())
            {
                this.customersDbContext.Customers.Add(new Db.Customer() { Id = 1, Name = "Customer1", Address = "Bangalore" });
                this.customersDbContext.Customers.Add(new Db.Customer() { Id = 2, Name = "Customer2", Address = "Chennai" });
                this.customersDbContext.Customers.Add(new Db.Customer() { Id = 3, Name = "Customer3", Address = "Delhi" });
                this.customersDbContext.Customers.Add(new Db.Customer() { Id = 4, Name = "Customer4", Address = "Mumbai" });
                this.customersDbContext.Customers.Add(new Db.Customer() { Id = 5, Name = "Customer5", Address = "Hyderabad" });
                customersDbContext.SaveChanges();
            }
            
        }

        public async Task<(bool IsSuccess, IEnumerable<Models.Customer> customer, string errorMessage)> GetCustomerAsync()
        {
            try
            {
                var customers =  await this.customersDbContext.Customers.ToListAsync();
                if(customers.Any())
                {
                    var result = mapper.Map<IEnumerable<Db.Customer>, IEnumerable<Models.Customer>>(customers);
                    return (true, result, null);
                }

                return (false, null, "Not found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false,null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, Models.Customer customer, string errorMessage)> GetCustomerAsync(int Id)
        {
            try
            {
                var customer = await this.customersDbContext.Customers.FirstOrDefaultAsync(c => c.Id == Id);
                if (customer != null)
                {
                    var result = mapper.Map<Db.Customer, Models.Customer>(customer);
                    return (true, result, null);
                }

                return (false, null, "Not Found");
            }
            catch(Exception ex) 
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
