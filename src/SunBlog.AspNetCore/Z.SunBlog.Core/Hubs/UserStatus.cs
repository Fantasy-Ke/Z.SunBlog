namespace Z.SunBlog.Core.Hubs
{
    public class UserStatus
    {
        public string Id { get; private set; }
        public OnlineType OnlineType { get; private set; }

        public DateTime OnLineTime { get; private set; }

        public DateTime? LeaveTime { get; private set; }

        public UserStatus(string id)
        {
            Id = id;
            OnlineType = OnlineType.OnLine;
            OnLineTime = DateTime.Now;
        }

        public void SetLeave()
        {
            OnlineType = OnlineType.Leave;
            LeaveTime = DateTime.Now;
        }
    }
}
