using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Module.DependencyInjection;

namespace Z.Fantasy.Core.Authorization
{
    public class PermissionCheck : IPermissionCheck
    {
        public virtual bool IsGranted(UserTokenModel userTokenModel, string[] authorizationNames)
        {
            var array = new string[] { "tttt" };
            return array.Contains(authorizationNames[0]);
        }
    }

    public interface IPermissionCheck : ITransientDependency
    {
        bool IsGranted(UserTokenModel userTokenModel, string[] authorizationNames);
    }
}
