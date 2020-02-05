using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Lex.Model;
using Amazon.Runtime;

namespace FormsChatbot.Services
{
    public interface IAWSLexService
    {
        ResponseEventHandler AfterResponseEvent { get; }
        RequestEventHandler BeforeRequestEvent { get; }
        ExceptionEventHandler ExceptionEvent { get; }

        Task<GetSessionResponse> GetSessionAsync(string sessionId);
        Task<GetSessionResponse> GetSessionAsync(string sessionId, CancellationToken cancellationToken);

        Task<PutSessionResponse> PutSessionAsync(string sessionId);
        Task<PutSessionResponse> PutSessionAsync(string sessionId, Dictionary<string, string> lexSessionAttributes);
        Task<PutSessionResponse> PutSessionAsync(string sessionId, Dictionary<string, string> lexSessionAttributes, CancellationToken cancellationToken);

        Task<DeleteSessionResponse> DeleteSessionAsync(string sessionId);
        Task<DeleteSessionResponse> DeleteSessionAsync(string sessionId, CancellationToken cancellationToken);

        Task<PostContentResponse> PostContentAsync(Stream content, string contentType, string sessionId);
        Task<PostContentResponse> PostContentAsync(Stream content, string contentType, string sessionId, string lexSessionAttributes);
        Task<PostContentResponse> PostContentAsync(Stream content, string contentType, string sessionId, string lexSessionAttributes, CancellationToken cancellationToken);

        Task<PostTextResponse> PostTextAsync(string messageToSend, string sessionId);
        Task<PostTextResponse> PostTextAsync(string messageToSend, string sessionId, Dictionary<string, string> lexSessionAttributes);
        Task<PostTextResponse> PostTextAsync(string messageToSend, string sessionId, Dictionary<string, string> lexSessionAttributes, CancellationToken cancellationToken);
    }
}
