using AutoMapper;
using DecenaSoluciones.POS.API.Models;
using DecenaSoluciones.POS.Shared.Dtos;
using Microsoft.Extensions.Logging;

namespace DecenaSoluciones.POS.API.Helper
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<Product, ProductViewModel>();
            CreateMap<Product, AddEditProduct>().ReverseMap();
            CreateMap<CustomerProduct, AddEditCustomerProduct>().ReverseMap();
            CreateMap<Customer, AddEditCustomer>()
                .ForMember(dest => dest.CustomerProducts, opt => opt.MapFrom(source => source.CustomerProducts != null ? source.CustomerProducts : new List<CustomerProduct>()))
                .ReverseMap();
            CreateMap<Customer, CustomerViewModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(source => $"{source.Name} {source.LastName}"))
                .ForMember(dest => dest.Product, opt => opt.MapFrom(source => getMaintenanceProduct(source.CustomerProducts)))
                .ForMember(dest => dest.NextMaintenance, opt => opt.MapFrom(source => getMaintenanceDate(source.CustomerProducts)));
        }

        private string getMaintenanceProduct(IEnumerable<CustomerProduct>? customerProducts)
        {
            if (customerProducts != null && customerProducts.Count() > 0)
            {
                var customerProduct = customerProducts
                        .Where(p => p.NeedMaintenance && p.NextMaintenance != null)
                        .OrderBy(p => p.NextMaintenance)
                        .FirstOrDefault();

                if (customerProduct != null)
                    return customerProduct.Product.Description;
            }

            return string.Empty; ;
        }
        private DateOnly? getMaintenanceDate(IEnumerable<CustomerProduct>? customerProducts)
        {
            if (customerProducts != null && customerProducts.Count() > 0)
            {
                var customerProduct = customerProducts
                        .Where(p => p.NeedMaintenance && p.NextMaintenance != null)
                        .OrderBy(p => p.NextMaintenance)
                        .FirstOrDefault();

                if (customerProduct != null)
                    return DateOnly.FromDateTime(customerProduct.NextMaintenance!.Value);
            }

            return null;
        }
    }
}
