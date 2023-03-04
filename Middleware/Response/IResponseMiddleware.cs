namespace Presence_API.Middleware.Response
{
    /// <summary>
    /// A middleware layer which interacts with the completion service, and updates the Memory accordingly
    /// </summary>
    public interface IResponseMiddleware
    {
        /// <summary>
        /// Adds the chatPrompt to memory and sends the complete memory to the completion service. Then adds the completion response to memory and returns that response
        /// </summary>
        Task<string> GetResponseAsync(string chatPrompt);
    }
}
