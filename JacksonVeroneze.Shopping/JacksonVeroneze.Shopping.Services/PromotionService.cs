using JacksonVeroneze.Shopping.Common;
using JacksonVeroneze.Shopping.Domain.Interface.Services;
using JacksonVeroneze.Shopping.Domain.Results;
using JacksonVeroneze.Shopping.Infra.CrossCutting.Network;
using JacksonVeroneze.Shopping.Infra.CrossCutting.Network.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JacksonVeroneze.Shopping.Services
{
    //
    // Summary:
    //     /// Class responsible for the service. ///
    //
    public class PromotionService : BaseService, IPromotionService
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
        public PromotionService(MakeRequest makeRequest)
            => _makeRequest = makeRequest;

        //
        // Summary:
        //     /// Method responsible for search a data. ///
        //
        public Task<IList<PromotionResult>> FindAllAsync()
        {
            IPromotionServiceRequest promotionServiceRequest = RefitServiceBuilder.Build<IPromotionServiceRequest>("http://pastebin.com/raw");

            return _makeRequest.StartUseCache(promotionServiceRequest.FindAllAsync(), "Promotion");
        }
    }
}