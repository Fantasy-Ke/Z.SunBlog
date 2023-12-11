using Microsoft.Extensions.Logging;

namespace Z.Fantasy.Core.Exceptions
{
    public class UserFriendlyException : ZException
    {
        public UserFriendlyException(string message)
        : base(message)
        {
        }

        public UserFriendlyException(string message, string details)
        : base(message)
        {
        }

        public UserFriendlyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public UserFriendlyException(
            string errorCode,
            params object[] parameters)
            : base(errorCode, null, parameters)
        {
        }

        public UserFriendlyException(
            string errorCode,
            LogLevel? logLevel,
            params object[] parameters)
            : base(errorCode, logLevel, parameters)
        {
        }

        public UserFriendlyException(
            Exception innerException,
            string errorCode,
            LogLevel? logLevel = null,
            params object[] parameters)
            : base(innerException, errorCode, logLevel, parameters)
        {
        }

        protected UserFriendlyException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
        }
    }
}
