using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CE.Assessment.BusinessLogic.Entities
{
    public class ExtraData
    {
        [JsonProperty("Extra Data")]
        public string Extra_Data { get; set; }
    }
}
