using AutoMapper;
using DecenaSoluciones.POS.API.Models;
using DecenaSoluciones.POS.Shared.Dtos;
using DecenaSoluciones.POS.Shared.Enums;
using Microsoft.Extensions.Logging;
using InventoryEntryType = DecenaSoluciones.POS.API.Models.InventoryEntryType;

namespace DecenaSoluciones.POS.API.Helper
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<Product, ProductViewModel>();
            CreateMap<Product, ProductsReportViewModel>();
            CreateMap<Product, AddEditProduct>().ReverseMap();
            CreateMap<Company, CompanyViewModel>();
            CreateMap<Company, AddEditCompany>().ReverseMap();
            CreateMap<CustomerProduct, AddEditCustomerProduct>().ReverseMap();
            CreateMap<SaleProduct, AddEditSaleProduct>().ReverseMap();
            CreateMap<QuotationProduct, AddEditSaleProduct>()
                .ForMember(dest => dest.SaleId, opt => opt.MapFrom(source => source.QuotationId))
                .ReverseMap();
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
                .ForMember(dest => dest.DismissedBySaleCode, opt => opt.MapFrom(source => source.DismissedSale != null ? source.DismissedSale.Code : null))
                .ForMember(dest => dest.Dismissed, opt => opt.MapFrom(source => source.Dismissed == true))
                .ReverseMap();
             CreateMap<Quotation, AddEditSale>()
                  .ForMember(dest => dest.IsAQuotation, opt => opt.MapFrom(source => true))
                  .ForMember(dest => dest.SaleProducts, opt => opt.MapFrom(source => source.QuotationProducts != null ? source.QuotationProducts : new List<QuotationProduct>()));
            CreateMap<AddEditSale, Quotation>()
                .ForMember(dest => dest.QuotationProducts, opt => opt.MapFrom(source => source.SaleProducts != null ? source.SaleProducts : new List<AddEditSaleProduct>()));
            CreateMap<Sale, SalesViewModel>()
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(source => GetCustomerName(source.Customer)))
                .ForMember(dest => dest.Total, opt => opt.MapFrom(source => GetTotalAmount(source)))
                .ForMember(dest => dest.Dismissed, opt => opt.MapFrom(source => source.Dismissed == true))
                .ForMember(dest => dest.IsAQuotation, opt => opt.MapFrom(source => false));
            CreateMap<Quotation, SalesViewModel>()
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(source => GetCustomerName(source.Customer)))
                .ForMember(dest => dest.Total, opt => opt.MapFrom(source => GetTotalAmount(source)))
                .ForMember(dest => dest.IsAQuotation, opt => opt.MapFrom(source => true));
            CreateMap<Sale, SalesReportViewModel>()
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(source => GetCustomerName(source.Customer)))
                .ForMember(dest => dest.Total, opt => opt.MapFrom(source => GetTotalAmount(source)))
                .ForMember(dest => dest.PayedAmount, opt => opt.MapFrom(source => GetPayedAmount(source)))
                .ForMember(dest => dest.ProductsQuantity, opt => opt.MapFrom(source => source.SaleProducts!.Sum(p => p.Quantity)));
            CreateMap<SaleProduct, LastSaleXProductViewModel>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(source => source.Id))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(source => source.Quantity))
                .ForMember(dest => dest.SalePrice, opt => opt.MapFrom(source => source.UnitPrice))
                .ForMember(dest => dest.SaleDate, opt => opt.MapFrom(source => source.Sale.CreationDate));
            CreateMap<Provider, AddEditProvider>()
                .ReverseMap();
            CreateMap<InventoryEntry, InventoryEntryViewModel>()
                .ForMember(dest => dest.InventoryEntryType, opt => opt.MapFrom(source => source.InventoryEntryTypeId))
                .ForMember(dest => dest.Details, opt => opt.MapFrom(source => source.InventoryEntryDetails))
                .ForMember(dest => dest.TotalCost, opt => opt.MapFrom(source => source.InventoryEntryDetails != null ? source.InventoryEntryDetails.Sum(p => p.TotalCost) : 0))
                .ForMember(dest => dest.ProviderName, opt => opt.MapFrom(source => source.Provider != null ? source.Provider.Name : ""));
            CreateMap<InventoryEntryViewModel, InventoryEntry>()
                 .ForMember(dest => dest.InventoryEntryType, opt => opt.MapFrom(source => new InventoryEntryType() { Id = (int)source.InventoryEntryType }))
                 .ForMember(dest => dest.InventoryEntryTypeId, opt => opt.MapFrom(source => (int)source.InventoryEntryType))
                 .ForMember(dest => dest.InventoryEntryDetails, opt => opt.MapFrom(source => source.Details));
            CreateMap<InventoryEntryDetail, InventoryEntryDetailViewModel>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(source => source.ProductId))
                .ReverseMap();
            CreateMap<InventoryEntryDetail, UpdateInventory>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(source => source.UnitPrice))
                .ForMember(dest => dest.Cost, opt => opt.MapFrom(source => source.UnitCost))
                .ReverseMap();
            CreateMap<InventoryEntry, InventoryInReport>()
                .ForMember(dest => dest.ProviderName, opt => opt.MapFrom(source => source.Provider!.Name))
                .ForMember(dest => dest.ProductQuantity, opt => opt.MapFrom(source => source.InventoryEntryDetails!.Count));
            CreateMap<InventoryEntry, InventoryOutReport>()
                .ForMember(dest => dest.ProductQuantity, opt => opt.MapFrom(source => source.InventoryEntryDetails!.Count));
            CreateMap<InventoryEntryDetail, InventoryInDetailReport>()
                .ForMember(dest => dest.ProductCode, opt => opt.MapFrom(source => source.Product.Code))
                .ForMember(dest => dest.ProductDescription, opt => opt.MapFrom(source => source.Product.Description));
            CreateMap<InventoryEntryDetail, InventoryOutDetailReport>()
                .ForMember(dest => dest.ProductCode, opt => opt.MapFrom(source => source.Product.Code))
                .ForMember(dest => dest.ProductDescription, opt => opt.MapFrom(source => source.Product.Description));
            CreateMap<List<InventoryEntry>, InventoryReportViewModel>()
                .ForMember(dest => dest.inventoryInEntries, opt => opt.MapFrom(source => source.Where(p => p.InventoryEntryTypeId == (int)Shared.Enums.InventoryEntryType.In)))
                .ForMember(dest => dest.inventoryOutEntries, opt => opt.MapFrom(source => source.Where(p => p.InventoryEntryTypeId == (int)Shared.Enums.InventoryEntryType.Out)))
                .ForMember(dest => dest.inventoryInEntriesDetails, opt => opt.MapFrom(source => source.Where(p => p.InventoryEntryTypeId == (int)Shared.Enums.InventoryEntryType.In)
                                                                                                        .Select(p => p.InventoryEntryDetails)
                                                                                                        .SelectMany(p => p!)))
                .ForMember(dest => dest.inventoryOutEntriesDetails, opt => opt.MapFrom(source => source.Where(p => p.InventoryEntryTypeId == (int)Shared.Enums.InventoryEntryType.Out)
                                                                                                        .Select(p => p.InventoryEntryDetails)
                                                                                                        .SelectMany(p => p!)));
        }

        private decimal GetPayedAmount(Sale sale)
        {
            decimal registeredAmounts = 
                (sale.TCAmount ?? 0m) +
                (sale.DepositsAmount ?? 0m) +
                (sale.CashAmount ?? 0m);

            return registeredAmounts > 0m ? registeredAmounts : GetTotalAmount(sale);
        }
        private string GetCustomerName(Customer? customer)
        {
            if (customer == null)
                return string.Empty;

            return $"{customer.Name} {customer.LastName}";
        }
        private decimal GetTotalAmount(Sale sale)
        {
            decimal calc = 0.0M;
            calc += sale.SaleProducts!.Sum(p => p.Total);
            calc += sale.WorkForceValue ?? 0.0M;
            calc -= sale.Discount ?? 0.0M;
            return calc;
        }
        private decimal GetTotalAmount(Quotation quotation)
        {
            decimal calc = 0.0M;
            calc += quotation.QuotationProducts!.Sum(p => p.Total);
            calc += quotation.WorkForceValue ?? 0.0M;
            calc -= quotation.Discount ?? 0.0M;
            return calc;
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
