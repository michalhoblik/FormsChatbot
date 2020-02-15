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

        Task<GetSessionResponse> GetSessionAsync(string userId);
        Task<GetSessionResponse> GetSessionAsync(string userId, CancellationToken cancellationToken);

        Task<PutSessionResponse> PutSessionAsync(string userId);
        Task<PutSessionResponse> PutSessionAsync(string userId, DialogAction dialogAction);
        Task<PutSessionResponse> PutSessionAsync(string userId, DialogAction dialogAction, Dictionary<string, string> lexSessionAttributes);
        Task<PutSessionResponse> PutSessionAsync(string userId, DialogAction dialogAction, Dictionary<string, string> lexSessionAttributes, CancellationToken cancellationToken);

        Task<DeleteSessionResponse> DeleteSessionAsync(string userId);
        Task<DeleteSessionResponse> DeleteSessionAsync(string userId, CancellationToken cancellationToken);

        Task<PostContentResponse> PostContentAsync(Stream content, string contentType, string userId);
        Task<PostContentResponse> PostContentAsync(Stream content, string contentType, string userId, string lexSessionAttributes);
        Task<PostContentResponse> PostContentAsync(Stream content, string contentType, string userId, string lexSessionAttributes, CancellationToken cancellationToken);

        Task<PostTextResponse> PostTextAsync(string messageToSend, string userId);
        Task<PostTextResponse> PostTextAsync(string messageToSend, string userId, Dictionary<string, string> lexSessionAttributes);
        Task<PostTextResponse> PostTextAsync(string messageToSend, string userId, Dictionary<string, string> lexSessionAttributes, CancellationToken cancellationToken);
    }
}
