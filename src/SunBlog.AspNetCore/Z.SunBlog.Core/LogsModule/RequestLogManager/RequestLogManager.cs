using Z.Fantasy.Core.DomainServiceRegister.Domain;
using Z.Fantasy.Core.Entities.EntityLog;

namespace Z.SunBlog.Core.LogsModule.RequestLogManager
{
    public class RequestLogManager : BusinessDomainService<ZRequestLog>, IRequestLogManager
    {
        public RequestLogManager(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override Task ValidateOnCreateOrUpdate(ZRequestLog entity)
        {
            return Task.CompletedTask;
        }
    }
}
