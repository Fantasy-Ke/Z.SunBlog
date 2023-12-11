namespace Z.Fantasy.Core.Hubs
{
    public class SendMessageModel
    {
        public string RecivedUserId { get; set; }

        public string Message { get; set; }

        public Guid? GroupId { get; set; }

        public string ReplyMessagedId { get; set; }

        public ChatType ChatType { get; set; }
    }
}
