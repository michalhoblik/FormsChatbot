using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Amazon.CognitoIdentity;
using Amazon.Lex;
using Amazon.Lex.Model;
using Amazon.Runtime;

namespace FormsChatbot.Services
{
    public class AWSLexService : IAWSLexService
    {
        private readonly AWSOptions _awsOptions;
        private CognitoAWSCredentials _awsCredentials;
        private AmazonLexClient _amazonLexClient;

        public AWSLexService(AWSOptions awsOptions)
        {
            _awsOptions = awsOptions;

            InitLexService();
            AddEventHandlers();
        }

        public ResponseEventHandler AfterResponseEvent { get; }
        public RequestEventHandler BeforeRequestEvent { get; }
        public ExceptionEventHandler ExceptionEvent { get; }

        protected void InitLexService()
        {
            //Grab region for Lex Bot services
            var region = Amazon.RegionEndpoint.GetBySystemName(_awsOptions.BotRegion);

            //Get credentials from Cognito
            _awsCredentials = new CognitoAWSCredentials(
                                _awsOptions.CognitoPoolID, // Identity pool ID
                                region); // Region

            //Instantiate Lex Client with Region
            _amazonLexClient = new AmazonLexClient(_awsCredentials, region);
        }

        private void AddEventHandlers()
        {
            _amazonLexClient.AfterResponseEvent += AfterResponseEvent;
            _amazonLexClient.BeforeRequestEvent += BeforeRequestEvent;
            _amazonLexClient.ExceptionEvent += ExceptionEvent;
        }

        //private void RemoveEventHandlers()
        //{
        //    _amazonLexClient.AfterResponseEvent -= AfterResponseEvent;
        //    _amazonLexClient.BeforeRequestEvent -= BeforeRequestEvent;
        //    _amazonLexClient.ExceptionEvent -= ExceptionEvent;
        //}

        public async Task<GetSessionResponse> GetSessionAsync(string sessionId)
        {
            return await GetSessionAsync(sessionId, default);
        }

        public async Task<GetSessionResponse> GetSessionAsync(string sessionId, CancellationToken cancellationToken)
        {
            var getSessionRequest = new GetSessionRequest
            {
                BotAlias = _awsOptions.LexBotAlias,
                BotName = _awsOptions.LexBotName,
                UserId = sessionId
            };

            try
            {
                var getSessionResponse = await _amazonLexClient.GetSessionAsync(getSessionRequest, cancellationToken);
                return getSessionResponse;
            }
            catch (Exception ex)
            {
                throw new BadRequestException(ex);
            }
        }

        public async Task<DeleteSessionResponse> DeleteSessionAsync(string sessionId)
        {
            return await DeleteSessionAsync(sessionId, default);
        }

        public async Task<DeleteSessionResponse> DeleteSessionAsync(string sessionId, CancellationToken cancellationToken)
        {
            var deleteSessionRequest = new DeleteSessionRequest
            {
                BotAlias = _awsOptions.LexBotAlias,
                BotName = _awsOptions.LexBotName,
                UserId = sessionId
            };

            try
            {
                var deleteSessionResponse = await _amazonLexClient.DeleteSessionAsync(deleteSessionRequest, cancellationToken);
                return deleteSessionResponse;
            }
            catch (Exception ex)
            {
                throw new BadRequestException(ex);
            }
        }

        public async Task<PutSessionResponse> PutSessionAsync(string sessionId)
        {
            return await PutSessionAsync(sessionId, null, default);
        }

        public async Task<PutSessionResponse> PutSessionAsync(string sessionId, Dictionary<string, string> lexSessionAttributes)
        {
            return await PutSessionAsync(sessionId, lexSessionAttributes, default);
        }

        public async Task<PutSessionResponse> PutSessionAsync(string sessionId, Dictionary<string, string> lexSessionAttributes, CancellationToken cancellationToken)
        {
            var putSessionRequest = new PutSessionRequest
            {
                BotAlias = _awsOptions.LexBotAlias,
                BotName = _awsOptions.LexBotName,
                UserId = sessionId
            };

            if (lexSessionAttributes != null)
                putSessionRequest.SessionAttributes = lexSessionAttributes;

            try
            {
                var putSessionResponse = await _amazonLexClient.PutSessionAsync(putSessionRequest, cancellationToken);
                return putSessionResponse;
            }
            catch (Exception ex)
            {
                throw new BadRequestException(ex);
            }
        }

        public async Task<PostTextResponse> PostTextAsync(string messageToSend, string sessionId)
        {
            return await PostTextAsync(messageToSend, sessionId, null);
        }

        public async Task<PostTextResponse> PostTextAsync(string messageToSend, string sessionId, Dictionary<string, string> lexSessionAttributes)
        {
            return await PostTextAsync(messageToSend, sessionId, lexSessionAttributes, default);
        }

        public async Task<PostTextResponse> PostTextAsync(string messageToSend, string sessionId, Dictionary<string, string> lexSessionAttributes, CancellationToken cancellationToken)
        {
            var lexTextRequest = new PostTextRequest
            {
                BotAlias = _awsOptions.LexBotAlias,
                BotName = _awsOptions.LexBotName,
                UserId = sessionId,
                InputText = messageToSend
            };

            if (lexSessionAttributes != null)
                lexTextRequest.SessionAttributes = lexSessionAttributes;

            try
            {
                var lexTextResponse = await _amazonLexClient.PostTextAsync(lexTextRequest, cancellationToken);
                return lexTextResponse;
            }
            catch (Exception ex)
            {
                throw new BadRequestException(ex);
            }
        }

        public async Task<PostContentResponse> PostContentAsync(Stream content, string contentType, string sessionId)
        {
            return await PostContentAsync(content, contentType, sessionId, null, default);
        }

        public async Task<PostContentResponse> PostContentAsync(Stream content, string contentType, string sessionId, string lexSessionAttributes)
        {
            return await PostContentAsync(content, contentType, sessionId, lexSessionAttributes, default);
        }

        public async Task<PostContentResponse> PostContentAsync(Stream content, string contentType, string sessionId, string lexSessionAttributes, CancellationToken cancellationToken)
        {
            var lexPostContentRequest = new PostContentRequest
            {
                BotAlias = _awsOptions.LexBotAlias,
                BotName = _awsOptions.LexBotName,
                UserId = sessionId,
                InputStream = content,
                ContentType = contentType
            };

            if (lexSessionAttributes != null)
                lexPostContentRequest.SessionAttributes = lexSessionAttributes;

            try
            {
                var lexContentResponse = await _amazonLexClient.PostContentAsync(lexPostContentRequest, cancellationToken);
                return lexContentResponse;
            }
            catch (Exception ex)
            {
                throw new BadRequestException(ex);
            }
        }

        //#region IDisposable Support

        //private bool disposedValue = false;

        //protected virtual void Dispose(bool disposing)
        //{
        //    if (!disposedValue)
        //    {
        //        if (disposing)
        //        {
        //            RemoveEventHandlers();
        //            _amazonLexClient.Dispose();
        //            _awsCredentials.ClearCredentials();
        //        }

        //        disposedValue = true;
        //    }
        //}

        //public void Dispose()
        //{
        //    Dispose(true);
        //}

        //#endregion
    }
}
