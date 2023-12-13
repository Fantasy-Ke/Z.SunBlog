using Microsoft.AspNetCore.SignalR;
using Z.Fantasy.Core.DomainServiceRegister.Domain;
using Z.Fantasy.Core.Hubs;
using Z.SunBlog.Core.MessageModule.Dto;

namespace Z.SunBlog.Core.MessageModule.DomainManager
{
    public class MessageManager : DomainService, IMessageManager
    {
        private readonly IHubContext<ChatHub, IChatService> _hubContext;
        public MessageManager(IServiceProvider serviceProvider,
            IHubContext<ChatHub, IChatService> hubContext) : base(serviceProvider)
        {
            _hubContext = hubContext;
        }

        public async Task SendAll(MessageInput input)
        {
            await _hubContext.Clients.All.ReceiveMessage(input);
        }

        public async Task SendOtherUser(MessageInput input)
        {
            var hubClient = HubClients.ConnectionClient.FirstOrDefault(p => p.GroupName == (input.UserId ?? UserService.UserId));
            if (hubClient != null)
            {
                await _hubContext.Clients.AllExcept(hubClient.ConnectionId).ReceiveMessage(input);
            }
        }

        public async Task SendUser(MessageInput input)
        {
            var hubClient = HubClients.ConnectionClient.FirstOrDefault(p => p.GroupName == input.UserId);
            if (hubClient == null) return;
            await _hubContext.Clients.Client(hubClient.ConnectionId).ReceiveMessage(input);
        }

        public async Task SendUsers(MessageInput input)
        {
            var hubClients = HubClients.ConnectionClient.Where(p => input.UserIds.Contains(p.GroupName));
            if (!hubClients.Any()) return;
            var connectionIds = hubClients.Select(c=>c.ConnectionId).ToList();
            await _hubContext.Clients.Clients(connectionIds).ReceiveMessage(input);
        }
    }
}
