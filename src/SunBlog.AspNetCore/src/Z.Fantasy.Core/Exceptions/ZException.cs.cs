using Microsoft.Extensions.Logging;

namespace Z.Fantasy.Core.Exceptions;

[Serializable]
public class ZException : Exception
{
    private LogLevel? _logLevel;
    public LogLevel? LogLevel => _logLevel;

    private string _errorCode;
    public string ErrorCode => _errorCode;
    /// <summary>
    /// Provides error message that I18n is not used
    /// </summary>
    protected string ErrorMessage { get; set; }


    public override string Message => GetLocalizedMessage();

    public ZException()
    {
    }

    public ZException(string message, LogLevel? logLevel = null)
        : base(message)
    {
        _logLevel = logLevel;
    }

    public ZException(string errorCode, LogLevel? logLevel, params object[] parameters)
        : this(null, errorCode, logLevel, parameters)
    {
    }

    public ZException(Exception innerException, string errorCode, LogLevel? logLevel = null, params object[] parameters)
        : base(null, innerException)
    {
        _errorCode = errorCode;
        _logLevel = logLevel;
    }

    public ZException(string message, Exception innerException, LogLevel? logLevel = null)
        : base(message, innerException)
    {
        _logLevel = logLevel;
    }

    protected ZException(SerializationInfo serializationInfo, StreamingContext context)
        : base(serializationInfo, context)
    {
    }

    private string GetLocalizedMessage()
    {
        if (string.IsNullOrWhiteSpace(ErrorCode))
            return base.Message;

        return GetLocalizedMessageExecuting();
    }

    protected virtual string GetLocalizedMessageExecuting()
    {
        return base.Message;
        //if (!SupportI18n)
        //{
        //    if (ErrorMessage.IsNullOrWhiteSpace())
        //        return base.Message;

        //    var parameters = GetParameters();
        //    if (parameters != null! && parameters.Any())
        //        return string.Format(ErrorMessage, GetParameters());

        //    return ErrorMessage;
        //}

        //if (ErrorCode!.StartsWith(Masa.BuildingBlocks.Data.Constants.ExceptionErrorCode.FRAMEWORK_PREFIX))
        //{
        //    //The current framework frame exception
        //    return FrameworkI18n!.T(ErrorCode!, false, GetParameters()) ?? base.Message;
        //}

        //return I18n!.T(ErrorCode, false, GetParameters()) ?? base.Message;
    }
}
