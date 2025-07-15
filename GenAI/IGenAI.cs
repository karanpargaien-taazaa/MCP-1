namespace GenAI
{
    public interface IGenAI
    {
        /// <summary>
        /// GetResponseAsync methods takes a request and returns a Object Response for that request.
        /// </summary>
        /// <typeparam name="T">The Kind Of Object You Want As Your Response</typeparam>
        /// <param name="request">Request with proper requirement description that you need.</param>
        /// <returns>An Object Response Corresponding To Your Request</returns>
        public Task<T> GetResponseAsync<T>(string request, bool requestIncludeResponseSchema = false) where T : IGenAIResponse<T>, new();
    }
}
