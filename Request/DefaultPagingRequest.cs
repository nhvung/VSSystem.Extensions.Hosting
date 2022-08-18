using Newtonsoft.Json;

namespace VSSystem.Extensions.Hosting.Request
{
    public class DefaultPagingRequest
    {
        [JsonProperty("PageSize")]
        public string SPageSize
        {
            set
            {
                int.TryParse(value, out _PageSize);
            }
        }

        int _PageSize;
        [JsonIgnore]
        public int PageSize { get { if (_PageSize <= 0) { _PageSize = 50; } return _PageSize; } set { _PageSize = value; } }

        [JsonProperty("PageIndex")]
        public string SPageIndex
        {
            set
            {
                int.TryParse(value, out _PageIndex);
            }
        }
        [JsonProperty("PageNumber")]
        public string SPageNumber
        {
            set
            {
                int.TryParse(value, out _PageIndex);
            }
        }

        int _PageIndex;
        [JsonIgnore]
        public int PageIndex { get { if (_PageIndex <= 0) { _PageIndex = 1; } return _PageIndex; } set { _PageIndex = value; } }

    }
}
