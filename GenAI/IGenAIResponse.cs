namespace GenAI
{
    public interface IGenAIResponse<T>
    {
        /// <summary>
        /// Override this method to provide a sample instance of the response type.
        /// </summary>
        /// <returns>Sample instance of the response type class</returns>
        public T GetSampleInstance();
    }
}
