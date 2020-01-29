using JacksonVeroneze.Shopping.Common;
using JacksonVeroneze.Shopping.Domain.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JacksonVeroneze.Shopping.Domain.Interface.Services
{
    public interface IPromotionService : IBaseService
    {
        Task<IList<PromotionResult>> FindAllAsync();
    }
}