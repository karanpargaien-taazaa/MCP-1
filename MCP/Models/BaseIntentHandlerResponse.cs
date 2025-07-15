namespace MCP.Models
{
    public class BaseIntentHandlerResponse
    {
        public ResponseData ResponseData { get; set; }
        public BaseIntentHandlerResponse()
        {
            ResponseData = new ResponseData
            {
                IsSuccess = true,
                IsAnythingExtraRequiredInPrompt = false,
                RequiredParameters = new List<string>()
            };
        }
    }
    public class ResponseData
    {
        public bool IsSuccess { get; set; }
        public bool IsAnythingExtraRequiredInPrompt { get; set; }
        public List<string> RequiredParameters { get; set; }
    }
}
