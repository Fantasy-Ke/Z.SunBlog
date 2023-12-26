using Z.EventBus.Manager;

namespace Z.EventBus.EventBus;

public class LocalEventBus : ILocalEventBus
{
    private readonly IEventHandlerManager _eventHandlerManager;

    public LocalEventBus(IEventHandlerManager eventHandlerManager)
    {
        _eventHandlerManager = eventHandlerManager;
    }

    /// <summary>
    /// 写入队列，后台执行事件
    /// </summary>
    /// <typeparam name="TEto"></typeparam>
    /// <param name="eto"></param>
    /// <returns></returns>
    public async Task EnqueueAsync<TEto>(TEto eto)
        where TEto : class
    {
        await _eventHandlerManager.EnqueueAsync(eto);
    }

    /// <summary>
    /// 订阅事件
    /// </summary>
    /// <typeparam name="TEto"></typeparam>
    /// <param name="eto"></param>
    /// <returns></returns>
    public async Task PushAsync<TEto>(TEto eto)
        where TEto : class
    {
        await _eventHandlerManager.ExecuteAsync(eto);
    }
}
