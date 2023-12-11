namespace Z.Fantasy.Core.DependencyInjection;

public class ServiceRegistrationActionList : List<Action<IOnServiceRegistredContext>>
{
    public bool IsClassInterceptorsDisabled { get; set; }
}
