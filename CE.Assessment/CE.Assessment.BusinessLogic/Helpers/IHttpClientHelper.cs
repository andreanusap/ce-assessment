namespace CE.Assessment.BusinessLogic.Helpers
{
    public interface IHttpClientHelper
    {
        Task<T> HttpGet<T>(string uri, string requestParam = null);

        Task<bool> HttpPut(string uri, string serializedDoc = null);
    }
}
