namespace Z.EventBus.Exceptions
{
    public class ChannelNullException : Exception
    {
        public ChannelNullException() { }

        public ChannelNullException(string message) : base(message) { }
    }

    public static class ThrowChannelNullException
    {
        public static void ThrowException(string str)
        {
            throw new ChannelNullException($"没有实现{str}Channel通道");
        }
    }
}
