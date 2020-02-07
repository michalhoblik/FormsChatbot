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

        public async Task<GetSessionResponse> GetSessionAsync(string userId)
        {
            return await GetSessionAsync(userId, default);
        }

        public async Task<GetSessionResponse> GetSessionAsync(string userId, CancellationToken cancellationToken)
        {
            var getSessionRequest = new GetSessionRequest
            {
                BotAlias = _awsOptions.LexBotAlias,
                BotName = _awsOptions.LexBotName,
                UserId = userId
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

        public async Task<DeleteSessionResponse> DeleteSessionAsync(string userId)
        {
            return await DeleteSessionAsync(userId, default);
        }

        public async Task<DeleteSessionResponse> DeleteSessionAsync(string userId, CancellationToken cancellationToken)
        {
            var deleteSessionRequest = new DeleteSessionRequest
            {
                BotAlias = _awsOptions.LexBotAlias,
                BotName = _awsOptions.LexBotName,
                UserId = userId
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

        public async Task<PutSessionResponse> PutSessionAsync(string userId)
        {
            return await PutSessionAsync(userId, null, default);
        }

        public async Task<PutSessionResponse> PutSessionAsync(string userId, Dictionary<string, string> lexSessionAttributes)
        {
            return await PutSessionAsync(userId, lexSessionAttributes, default);
        }

        public async Task<PutSessionResponse> PutSessionAsync(string userId, Dictionary<string, string> lexSessionAttributes, CancellationToken cancellationToken)
        {
            var putSessionRequest = new PutSessionRequest
            {
                BotAlias = _awsOptions.LexBotAlias,
                BotName = _awsOptions.LexBotName,
                UserId = userId
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

        public async Task<PostTextResponse> PostTextAsync(string messageToSend, string userId)
        {
            return await PostTextAsync(messageToSend, userId, null);
        }

        public async Task<PostTextResponse> PostTextAsync(string messageToSend, string userId, Dictionary<string, string> lexSessionAttributes)
        {
            return await PostTextAsync(messageToSend, userId, lexSessionAttributes, default);
        }

        public async Task<PostTextResponse> PostTextAsync(string messageToSend, string userId, Dictionary<string, string> lexSessionAttributes, CancellationToken cancellationToken)
        {
            var lexTextRequest = new PostTextRequest
            {
                BotAlias = _awsOptions.LexBotAlias,
                BotName = _awsOptions.LexBotName,
                UserId = userId,
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

        public async Task<PostContentResponse> PostContentAsync(Stream content, string contentType, string userId)
        {
            return await PostContentAsync(content, contentType, userId, null, default);
        }

        public async Task<PostContentResponse> PostContentAsync(Stream content, string contentType, string userId, string lexSessionAttributes)
        {
            return await PostContentAsync(content, contentType, userId, lexSessionAttributes, default);
        }

        public async Task<PostContentResponse> PostContentAsync(Stream content, string contentType, string userId, string lexSessionAttributes, CancellationToken cancellationToken)
        {
            var lexPostContentRequest = new PostContentRequest
            {
                BotAlias = _awsOptions.LexBotAlias,
                BotName = _awsOptions.LexBotName,
                UserId = userId,
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
    }
}
