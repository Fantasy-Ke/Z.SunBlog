using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Z.Fantasy.Core.RedisModule;
using Z.Fantasy.Core.UserSession;
using Z.SunBlog.Core.Const;

namespace Z.SunBlog.Core.Hubs
{
    public class ChatHub : Hub<IChatService>
    {
        private readonly ILogger _logger;
        private readonly ICacheManager _cacheManager;
        public ChatHub(ILoggerFactory loggerFactory, ICacheManager cacheManager)
        {
            _logger = loggerFactory.CreateLogger<ChatHub>();
            _cacheManager = cacheManager;
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            if (Context.User?.Identity?.IsAuthenticated == true)
            {
                var claims = Context.User.Claims.ToList();
                //按用户分组
                //是有必要的 例如多个浏览器、多个标签页使用同个用户登录 应当归属于一组
                var groupName = claims.FirstOrDefault(c => c.Type == ZClaimTypes.UserId).Value;
                await AddToGroup(groupName);
                await AddCacheClient(groupName);
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (string.IsNullOrEmpty(Context.ConnectionId)) return;
            _logger.LogWarning(exception?.Message ?? "断开连接信息异常");
            //按用户分组
            //是有必要的 例如多个浏览器、多个标签页使用同个用户登录 应当归属于一组
            var groupName = await RemoveCacheClient();
            await RemoveToGroup(groupName);
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

        public async Task AddCacheClient(string groupName)
        {
            var connectionClients = await _cacheManager.LRangeAsync<ConnectionClient>(CacheConst.SignlRKey, 0, -1);
            var client = connectionClients.FirstOrDefault(n => n.GroupName == groupName);
            int index = Array.IndexOf(connectionClients, client);
            if (index == -1)
            {
                await _cacheManager.LPushAsync(CacheConst.SignlRKey, new ConnectionClient()
                {
                    GroupName = groupName,
                    ConnectionId = Context.ConnectionId
                });
            }
            else
            {
                await _cacheManager.LSetAsync(CacheConst.SignlRKey, index, new ConnectionClient()
                {
                    GroupName = groupName,
                    ConnectionId = Context.ConnectionId
                });
            }

        }

        public async Task<string> RemoveCacheClient()
        {
            var connectionClients = await _cacheManager.LRangeAsync<ConnectionClient>(CacheConst.SignlRKey, 0, -1);
            var client = connectionClients.FirstOrDefault(n => n.ConnectionId == Context.ConnectionId);
            if (client != null)
            {
                await _cacheManager.LRemAsync(CacheConst.SignlRKey, 0, client);
                _logger.LogWarning($"remove :{client.ConnectionId}");
                return client.GroupName;
            }
            return string.Empty;
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
