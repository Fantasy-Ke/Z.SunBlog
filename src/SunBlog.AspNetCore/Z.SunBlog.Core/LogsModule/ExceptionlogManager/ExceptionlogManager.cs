using Z.Fantasy.Core.DomainServiceRegister.Domain;
using Z.Fantasy.Core.Entities.EntityLog;

namespace Z.SunBlog.Core.LogsModule.ExceptionlogManager
{
    public class ExceptionLogManager : BusinessDomainService<ZExceptionLog>, IExceptionLogManager
    {
        public ExceptionLogManager(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override Task ValidateOnCreateOrUpdate(ZExceptionLog entity)
        {
            return Task.CompletedTask;
        }
    }
}
