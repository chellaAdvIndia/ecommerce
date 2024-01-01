using Ecommerce.Api.Customers.Db;

namespace Ecommerce.Api.Customers.Interfaces
{
    public interface ICustomerProvider
    {
        Task<(bool IsSuccess, IEnumerable<Models.Customer> customer, string errorMessage)> GetCustomerAsync();
        Task<(bool IsSuccess, Models.Customer customer, string errorMessage)> GetCustomerAsync(int Id);
    }
}
