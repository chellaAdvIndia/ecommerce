using AutoMapper;
using ECommerce.Api.Products.Db;
using ECommerce.Api.Products.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models = ECommerce.Api.Products.Models;

namespace ECommerce.Api.Products.Providers
{
    public class ProductProvider : IProductsProvider
    {
        private readonly ProductDbContext dbContext;
        private readonly ILogger<ProductProvider> logger;
        private readonly IMapper mapper;

        public ProductProvider(ProductDbContext dbContext, ILogger<ProductProvider> logger, IMapper mapper) 
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }

        private void SeedData()
        {
            if(!this.dbContext.Products.Any())
            {
                this.dbContext.Products.Add(new Product() { Id = 1, Name = "Keyboard", Price = 650, Inventory = 20 });
                this.dbContext.Products.Add(new Product() { Id = 2, Name = "Mouse", Price = 450, Inventory = 20 });
                this.dbContext.Products.Add(new Product() { Id = 3, Name = "Monitor", Price = 5650, Inventory = 20 });
                this.dbContext.Products.Add(new Product() { Id = 4, Name = "Headpone", Price = 1650, Inventory = 20 });
                this.dbContext.SaveChanges();
            }
        }

        async Task<(bool IsSuccess, IEnumerable<Models.Product> products, string ErrorMessage)> IProductsProvider.GetProductsAsync()
        {
            try
            {
                var products = await dbContext.Products.ToListAsync();
                if (products.Any())
                {
                    var result = mapper.Map<IEnumerable<Db.Product>, IEnumerable<Models.Product>>(products);
                    return (true, result, null);
                }

                return (false, null, "Not found");

            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());

                return (false, null, ex.Message);
               
            }
        }

        public async Task<(bool IsSuccess, Models.Product product, string ErrorMessage)> GetProductAsync(int Id)
        {
            try
            {
                var product = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == Id);
                if (product != null)    
                {
                    var result = mapper.Map<Db.Product, Models.Product>(product);
                    return (true, result, null);
                }

                return (false, null, "Not found");

            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());

                return (false, null, ex.Message);

            }
        }
    }
}
