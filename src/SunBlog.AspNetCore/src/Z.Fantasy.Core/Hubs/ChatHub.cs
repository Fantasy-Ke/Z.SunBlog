using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Z.Fantasy.Core.Authorization;
using Z.EventBus.EventBus;
using Z.Fantasy.Core.UserSession;

namespace Z.Fantasy.Core.Hubs
{
    [ZAuthorization]
    public class ChatHub : Hub
    {
        private readonly ILogger _logger;
        private readonly ILocalEventBus _eventBus;
        private readonly IUserSession _userSession;
        public ChatHub(ILoggerFactory loggerFactory
            , ILocalEventBus eventBus,
        IUserSession userSession)
        {
            _logger = loggerFactory.CreateLogger<ChatHub>();
            _eventBus = eventBus;
            _userSession = userSession;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = GetUserId();
            var status = new UserStatus(userId);
            if (await RedisHelper.ExistsAsync(userId.ToString()))
            {
                await RedisHelper.SetAsync(userId.ToString(), status, exists: CSRedis.RedisExistence.Xx);
            }
            else
            {
                await RedisHelper.SetAsync(userId.ToString(), status, exists: CSRedis.RedisExistence.Nx);
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _logger.LogWarning(exception?.Message ?? "断开连接信息异常");
            var userId = GetUserId();
            var userStaus = await RedisHelper.GetAsync<UserStatus>(userId.ToString());
            userStaus.SetLeave();
            await RedisHelper.SetAsync(userId.ToString(), userStaus, exists: CSRedis.RedisExistence.Xx);

        }

        public Task SendMessage(SendMessageModel message)
        {
            return Task.CompletedTask;
        }

        private string GetUserId()
        {
            return _userSession.UserId;
        }
    }
}
