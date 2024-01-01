using ECommerce.Api.Products.Db;

namespace ECommerce.Api.Products.Profiles
{
    public class ProductProfile : AutoMapper.Profile
    {
        public ProductProfile() 
        {
            CreateMap<Product, Models.Product>();
        }
    }
}
