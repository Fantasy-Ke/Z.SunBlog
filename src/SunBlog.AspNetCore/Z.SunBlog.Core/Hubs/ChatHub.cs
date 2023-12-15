using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using Z.Fantasy.Core.RedisModule;
using Z.Fantasy.Core.UserSession;
using Z.SunBlog.Core.Const;
using Z.SunBlog.Core.MessageModule.DomainManager;
using Z.SunBlog.Core.MessageModule.Dto;

namespace Z.SunBlog.Core.Hubs
{
    public class ChatHub : Hub<IChatService>
    {
        private readonly ILogger _logger;
        private readonly ICacheManager _cacheManager;
        private readonly IMessageManager _messageManager;
        public ChatHub(ILoggerFactory loggerFactory, ICacheManager cacheManager, IMessageManager messageManager)
        {
            _logger = loggerFactory.CreateLogger<ChatHub>();
            _cacheManager = cacheManager;
            _messageManager = messageManager;
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
                var accountIds =  await _cacheManager.LRangeAsync("Blog.Blogger.Key", 0, -1);
                accountIds.ToList().ForEach(async c =>
                {
                    await _messageManager.SendAll(new MessageInput() { Title = "提示！！", Message = "博主重新连接了大家赶紧拜访拜访！！！" });
                });
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

        // 判断指定的 contextId 是否还在连接
        public bool IsConnectionActive(string contextId)
        {
            // 使用 Clients 检查指定的 connectionId 是否处于连接状态
            var connection = HubClients.ConnectionClient.FirstOrDefault(c=>c.ConnectionId == contextId);
            return connection != null;
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
            HubClients.ConnectionClient.Add(new ConnectionClient()
            {
                GroupName = groupName,
                ConnectionId = Context.ConnectionId
            });
            var connectionClients = await _cacheManager.LRangeAsync<ConnectionClient>(CacheConst.SignlRKey, 0, -1);
            var clients = connectionClients.Where(n => n.GroupName == groupName).ToList();
            if (clients.Any())
            {
                clients.ForEach(async client =>
                {
                    if (!IsConnectionActive(client.ConnectionId))
                    {
                        int index = Array.IndexOf(connectionClients, client);
                        await _cacheManager.LRemAsync(CacheConst.SignlRKey, index, client);
                        await _cacheManager.LPushAsync(CacheConst.SignlRKey, new ConnectionClient()
                        {
                            GroupName = groupName,
                            ConnectionId = Context.ConnectionId
                        });
                    }
                });
            }
            else
            {
                await _cacheManager.LPushAsync(CacheConst.SignlRKey, new ConnectionClient()
                {
                    GroupName = groupName,
                    ConnectionId = Context.ConnectionId
                });
            }
        }

        public async Task<string> RemoveCacheClient()
        {
            HubClients.ConnectionClient.Remove(HubClients.ConnectionClient.FirstOrDefault(c=>c.ConnectionId == Context.ConnectionId));
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
