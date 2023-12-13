using Z.Fantasy.Core.DomainServiceRegister.Domain;
using Z.SunBlog.Core.FriendLinkModule;
using Z.SunBlog.Core.MessageModule.Dto;

namespace Z.SunBlog.Core.MessageModule.DomainManager
{
    public interface IMessageManager : IDomainService
    {
        /// <summary>
        /// 发送消息给所有
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task SendAll(MessageInput input);

        /// <summary>
        /// 发送消息给除了发送人的其他人
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task SendOtherUser(MessageInput input);

        /// <summary>
        /// 发送消息给某个人
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task SendUser(MessageInput input);

        /// <summary>
        /// 发送消息给某些人
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task SendUsers(MessageInput input);
    }
}
