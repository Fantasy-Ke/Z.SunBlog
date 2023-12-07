namespace Z.Module.Modules.interfaces
{
    public interface IZModule :
        IPreConfigureServices,
        IOnInitApplication,
        IPostInitApplication,
        IOnInitApplicationAsync,
        IPostInitApplicationAsync
    {
        void ConfigureServices(ServiceConfigerContext context);
    }
}
