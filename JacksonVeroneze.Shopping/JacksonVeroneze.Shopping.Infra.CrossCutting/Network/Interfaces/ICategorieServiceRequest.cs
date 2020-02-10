using JacksonVeroneze.Shopping.Domain.Results;
using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JacksonVeroneze.Shopping.Infra.CrossCutting.Network.Interfaces
{

    [Headers(new[] { "Content-Type: application/json;charset=UTF-8", "Accept: application/json;charset=UTF-8" })]
    public interface ICategorieServiceRequest
    {
        [Get("/YNR2rsWe")]
        Task<IList<CategoryResult>> FindAllAsync();
    }
}