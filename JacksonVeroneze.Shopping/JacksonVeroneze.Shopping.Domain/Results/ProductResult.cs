using Newtonsoft.Json;

namespace JacksonVeroneze.Shopping.Domain.Results
{
    public class ProductResult
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Photo { get; set; }

        public double Price { get; set; }

        [JsonProperty("category_id")]
        public int? CategoryId { get; set; }
    }
}