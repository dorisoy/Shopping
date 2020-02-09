using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Refit;
using System;
using System.Net.Http;

namespace JacksonVeroneze.Shopping.Infra.CrossCutting.Network
{
    //
    // Summary:
    //     /// Class responsible for network service. ///
    //
    public class RefitServiceBuilder
    {
        protected static readonly RefitSettings _refitSettings = new RefitSettings
        {
            ContentSerializer = new JsonContentSerializer(new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = { new StringEnumConverter() }
            })
        };

        protected static HttpClient _httpClient;

        //
        // Summary:
        //     Médodo responsável por localizar a interface para enviar a requisição.
        //
        public static T Build<T>(string url) where T : class
        {
            _httpClient = new HttpClient();

            _httpClient.BaseAddress = new Uri(url);

            return RestService.For<T>(_httpClient, _refitSettings);
        }
    }
}