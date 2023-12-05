namespace Z.EventBus
{
    public class EventAttributeNullException : Exception
    {
        public EventAttributeNullException() { }

        public EventAttributeNullException(string message) : base(message)
        {
        }
    }

    public static class ThorwEventAttributeNullException
    {
        public static void ThorwException()
        {
            throw new EventAttributeNullException("Eto请实现EventDiscriptorAttribute特性");
        }
    }
}
