namespace CE.Assessment.BusinessLogic.Entities
{
    public class PatchResponse
    {
        public Content Content { get; set; }
        public int StatusCode { get; set; }
        public string LogId { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public object ValidationErrors { get; set; }
    }

    public class Content
    {
        public int RejectedCount { get; set; }
        public int AcceptedCount { get; set; }
        public dynamic ProductMessages { get; set; }
    }
}
