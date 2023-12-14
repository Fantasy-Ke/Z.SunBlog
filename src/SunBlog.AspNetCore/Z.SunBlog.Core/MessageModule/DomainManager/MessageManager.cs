using Microsoft.AspNetCore.SignalR;
using Z.Fantasy.Core.DomainServiceRegister.Domain;
using Z.Fantasy.Core.RedisModule;
using Z.SunBlog.Core.Const;
using Z.SunBlog.Core.Hubs;
using Z.SunBlog.Core.MessageModule.Dto;

namespace Z.SunBlog.Core.MessageModule.DomainManager
{
    public class MessageManager : DomainService, IMessageManager
    {
        private readonly IHubContext<ChatHub, IChatService> _hubContext;
        private readonly ICacheManager _cacheManager;
        public MessageManager(IServiceProvider serviceProvider,
            IHubContext<ChatHub, IChatService> hubContext,
            ICacheManager cacheManager) : base(serviceProvider)
        {
            _hubContext = hubContext;
            _cacheManager = cacheManager;
        }

        public async Task SendAll(MessageInput input)
        {
            await _hubContext.Clients.All.ReceiveMessage(input);
        }

        public async Task SendOtherUser(MessageInput input)
        {
            var connectionClients = await _cacheManager.LRangeAsync<ConnectionClient>(CacheConst.SignlRKey, 0, -1);
            var hubClient = connectionClients.FirstOrDefault(p => p.GroupName == (input.UserId ?? UserService.UserId));
            if (hubClient != null)
            {
                await _hubContext.Clients.AllExcept(hubClient.ConnectionId).ReceiveMessage(input);
            }
        }

        public async Task SendUser(MessageInput input)
        {
            var connectionClients = await _cacheManager.LRangeAsync<ConnectionClient>(CacheConst.SignlRKey, 0, -1);
            var hubClient = connectionClients.FirstOrDefault(p => p.GroupName == input.UserId);
            if (hubClient == null) return;
            await _hubContext.Clients.Client(hubClient.ConnectionId).ReceiveMessage(input);
        }

        public async Task SendUsers(MessageInput input)
        {
            var connectionClients = await _cacheManager.LRangeAsync<ConnectionClient>(CacheConst.SignlRKey, 0, -1);
            var hubClients = connectionClients.Where(p => input.UserIds.Contains(p.GroupName));
            if (!hubClients.Any()) return;
            var connectionIds = hubClients.Select(c=>c.ConnectionId).ToList();
            await _hubContext.Clients.Clients(connectionIds).ReceiveMessage(input);
        }
    }
}
