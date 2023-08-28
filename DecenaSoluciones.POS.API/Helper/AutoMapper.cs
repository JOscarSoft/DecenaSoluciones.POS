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
        }
    }
}
