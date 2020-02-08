using JacksonVeroneze.Shopping.Common;
using JacksonVeroneze.Shopping.Domain.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JacksonVeroneze.Shopping.Domain.Interface.Services
{
    public interface IProductService : IBaseService
    {
        Task<IList<ProductResult>> FindAllAsync();

        Task<IList<ProductResult>> FindByCategotyIdAsync(int categoryId);

        Task<IList<ProductResult>> SearchAsync(string name);
    }
}