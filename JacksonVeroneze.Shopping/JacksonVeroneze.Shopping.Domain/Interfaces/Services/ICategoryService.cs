using JacksonVeroneze.Shopping.Common;
using JacksonVeroneze.Shopping.Domain.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JacksonVeroneze.Shopping.Domain.Interface.Services
{
    public interface ICategoryService : IBaseService
    {
        Task<IList<CategoryResult>> FindAllAsync();
    }
}