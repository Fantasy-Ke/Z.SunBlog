using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Z.EventBus.EventBus;
using Z.Fantasy.Core.UserSession;

namespace Z.Fantasy.Core.Hubs
{
    public class ChatHub : Hub<IChatService>
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
            await base.OnConnectedAsync();
            if (Context.User.Claims.Count() > 0)
            {
                //按用户分组
                //是有必要的 例如多个浏览器、多个标签页使用同个用户登录 应当归属于一组
                var groupName = Context.User.Claims.FirstOrDefault(c => c.Type == ZClaimTypes.UserId).Value;
                await AddToGroup(groupName);
                AddCacheClient(groupName);
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _logger.LogWarning(exception?.Message ?? "断开连接信息异常");
            if (Context.User.Claims.Count() > 0)
            {
                //按用户分组
                //是有必要的 例如多个浏览器、多个标签页使用同个用户登录 应当归属于一组
                var groupName = Context.User.Claims.FirstOrDefault(c => c.Type == ZClaimTypes.UserId).Value;
                await RemoveToGroup(groupName);
                RemoveCacheClient(groupName);
            }

            await base.OnDisconnectedAsync(exception);

        }

        /// <summary>
        /// 加入指定组
        /// </summary>
        /// <param name="groupName">组名</param>
        /// <returns></returns>
        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        /// <summary>
        /// 加入指定组
        /// </summary>
        /// <param name="groupName">组名</param>
        /// <returns></returns>
        public async Task RemoveToGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        public void AddCacheClient(string groupName)
        {
            HubClients.ConnectionClient.Add(new ConnectionClient()
            {
                GroupName = groupName,
                ConnectionId = Context.ConnectionId
            });
        }

        public void RemoveCacheClient(string groupName)
        {
            var client = HubClients.ConnectionClient.FirstOrDefault(n => n.ConnectionId == Context.ConnectionId && n.GroupName == groupName);
            if (client != null)
            {
                HubClients.ConnectionClient.Remove(client);
                _logger.LogWarning($"remove :{client.ConnectionId}");
            }
        }
    }

    public static class HubClients
    {
        public static List<ConnectionClient> ConnectionClient = new List<ConnectionClient>();
    }

    public class ConnectionClient
    {
        public string GroupName { get; set; }
        public string ConnectionId { get; set; }
    }
}
