using Newtonsoft.Json;

namespace CE.Assessment.Shared.Entities
{
    public class ExtraData
    {
        [JsonProperty("Extra Data")]
        public string Extra_Data { get; set; }
    }
}
