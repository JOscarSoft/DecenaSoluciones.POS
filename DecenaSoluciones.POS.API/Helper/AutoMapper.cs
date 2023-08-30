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
            CreateMap<Customer, AddEditCustomer>().ReverseMap();
            CreateMap<Customer, CustomerViewModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(source => $"{source.Name} {source.LastName}"))
                .ForMember(dest => dest.Product, opt => opt.MapFrom(source => getMaintenanceProduct(source.CustomerProducts)!.Product!.Description ?? string.Empty))
                .ForMember(dest => dest.NextMaintenance, opt => opt.MapFrom(source => getMaintenanceProduct(source.CustomerProducts)!.NextMaintenance));
        }

        private CustomerProduct? getMaintenanceProduct(IEnumerable<CustomerProduct>? customerProducts)
        {
            CustomerProduct? result = null;

            if (customerProducts != null && customerProducts.Count() > 0)
            {
                result = customerProducts
                    .Where(p => p.NextMaintenance != null)
                    .OrderBy(p => p.NextMaintenance)
                    .FirstOrDefault();
            }

            return result;
        }
    }
}
