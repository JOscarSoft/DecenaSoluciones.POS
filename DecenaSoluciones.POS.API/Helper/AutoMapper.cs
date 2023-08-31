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
            CreateMap<SaleProduct, AddEditSaleProduct>().ReverseMap();
            CreateMap<QuotationProduct, AddEditSaleProduct>().ReverseMap();
            CreateMap<Customer, AddEditCustomer>()
                .ForMember(dest => dest.CustomerProducts, opt => opt.MapFrom(source => source.CustomerProducts != null ? source.CustomerProducts : new List<CustomerProduct>()))
                .ReverseMap();
            CreateMap<Customer, CustomerViewModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(source => $"{source.Name} {source.LastName}"))
                .ForMember(dest => dest.Product, opt => opt.MapFrom(source => getMaintenanceProduct(source.CustomerProducts)))
                .ForMember(dest => dest.NextMaintenance, opt => opt.MapFrom(source => getMaintenanceDate(source.CustomerProducts)));
            CreateMap<Sale, AddEditSale>()
                .ForMember(dest => dest.IsAQuotation, opt => opt.MapFrom(source => false))
                .ForMember(dest => dest.SaleProducts, opt => opt.MapFrom(source => source.SaleProducts != null ? source.SaleProducts : new List<SaleProduct>()))
                .ReverseMap();
            CreateMap<Quotation, AddEditSale>()
                .ForMember(dest => dest.IsAQuotation, opt => opt.MapFrom(source => true))
                .ForMember(dest => dest.SaleProducts, opt => opt.MapFrom(source => source.QuotationProducts != null ? source.QuotationProducts : new List<QuotationProduct>()))
                .ReverseMap();
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
