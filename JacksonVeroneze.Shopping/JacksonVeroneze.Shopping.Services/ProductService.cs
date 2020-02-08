using JacksonVeroneze.Shopping.Common;
using JacksonVeroneze.Shopping.Domain.Interface.Services;
using JacksonVeroneze.Shopping.Domain.Results;
using JacksonVeroneze.Shopping.Infra.CrossCutting.Network;
using JacksonVeroneze.Shopping.Infra.CrossCutting.Network.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JacksonVeroneze.Shopping.Services
{
    //
    // Summary:
    //     /// Class responsible for the service. ///
    //
    public class ProductService : BaseService, IProductService
    {
        private readonly MakeRequest _makeRequest;

        //
        // Summary:
        //     /// Method responsible for initializing the service. ///
        //
        // Parameters:
        //   makeRequest:
        //     The makeRequest param.
        //
        public ProductService(MakeRequest makeRequest)
            => _makeRequest = makeRequest;

        //
        // Summary:
        //     /// Method responsible for search a data. ///
        //
        public Task<IList<ProductResult>> FindAllAsync()
        {
            IProductServiceRequest productServiceRequest = RefitServiceBuilder.Build<IProductServiceRequest>("http://pastebin.com/raw");

            return _makeRequest.StartUseCache(productServiceRequest.FindAllAsync(), "Product");
        }

        //
        // Summary:
        //     /// Method responsible for search a data by param. ///
        //
        // Parameters:
        //   categoryId:
        //     The categoryId param.
        //
        public async Task<IList<ProductResult>> FindByCategotyIdAsync(int categoryId)
        {
            IList<ProductResult> productResults= await FindAllAsync();

            return productResults.Where(x => x.CategoryId == categoryId).ToList();
        }

        //
        // Summary:
        //     /// Method responsible for search a data by param. ///
        //
        // Parameters:
        //   name:
        //     The name param.
        //
        public async Task<IList<ProductResult>> SearchAsync(string name)
        {
            IList<ProductResult> productResults = await FindAllAsync();

            return productResults.Where(x => x.Name.ToUpperInvariant().Contains(name.ToUpperInvariant())).ToList();
        }
    }
}