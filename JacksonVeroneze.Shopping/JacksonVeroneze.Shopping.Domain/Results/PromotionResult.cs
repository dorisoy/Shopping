using Newtonsoft.Json;
using System.Collections.Generic;

namespace JacksonVeroneze.Shopping.Domain.Results
{
    public class PromotionResult
    {
        public string Name { get; set; }

        [JsonProperty("category_id")]
        public int CategoryId { get; set; }

        public IList<PromotionPoliceResult> Policies { get; set; }
    }
}