using MessagePack;
using MessagePack.Resolvers;

namespace Z.RabbitMQ;

public interface IMsgPackSerializer
{
    byte[] MessageToBytes<T>(T message);

    byte[] MessageToBytes(Type type, object message);

    T BytesToMessage<T>(byte[] bytes);
}

/// <summary>
/// MsgPack 序列化器
/// </summary>
public class MsgPackSerializer : IMsgPackSerializer
{
    static MessagePackSerializerOptions _serializerOptions;
    private static volatile bool isInit = true;

    public MsgPackSerializer()
    {
        Init();
    }

    protected static void Init()
    {
        if (isInit)
        {
            StaticCompositeResolver.Instance.Register(
              NativeDateTimeResolver.Instance,
              StandardResolverAllowPrivate.Instance,
              ContractlessStandardResolver.Instance
            );
            _serializerOptions = MessagePackSerializerOptions.Standard.WithResolver(StaticCompositeResolver.Instance);

            isInit = false;
        }
    }

    public byte[] MessageToBytes<T>(T message)
    {
        return MessageToBytes(typeof(T), message);
    }
    public byte[] MessageToBytes(Type type, object message)
    {
        return MessagePackSerializer.Serialize(type, message, _serializerOptions);
    }
    public T BytesToMessage<T>(byte[] bytes)
    {
        return (T)MessagePackSerializer.Deserialize(typeof(T), bytes, _serializerOptions);
    }
}
