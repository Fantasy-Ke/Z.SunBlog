using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Fantasy.Core.Entities.IAuditing;
using Z.Fantasy.Core.Extensions;
using Z.Fantasy.Core.Helper;
using Z.Fantasy.Core.UserSession;
using Z.Module.DependencyInjection;

namespace Z.Fantasy.Core.Entities.Auditing
{
    public class AuditPropertySetter : IAuditPropertySetter, ITransientDependency
    {
        private readonly IUserSession _userSession;
        public AuditPropertySetter(IUserSession userSession)
        {
            _userSession = userSession;
        }
        public virtual void SetCreationProperties(object targetObject)
        {
            SetCreationTime(targetObject);
            SetCreatorId(targetObject);
            SetIsDeleter(targetObject);
        }

        public virtual void SetDeletionProperties(object targetObject)
        {
            SetDeletionTime(targetObject);
            SetDeleterId(targetObject);
            SetIsDeleter(targetObject, true);
        }


        protected virtual void SetCreationTime(object targetObject)
        {
            if (!(targetObject is IHasCreationTime objectCreationTime))
            {
                return;
            }

            if (objectCreationTime.CreationTime == default)
            {
                ObjectPropertyHelper.TrySetProperty(objectCreationTime, x => x.CreationTime, () => DateTime.Now);
            }
        }

        protected virtual void SetCreatorId(object targetObject)
        {
            if (_userSession.UserId.IsNullEmpty())
            {
                return;
            }

            if (targetObject is IMayHaveCreator mayHaveCreatorObject)
            {
                if (!mayHaveCreatorObject.CreatorId.IsNullEmpty() && mayHaveCreatorObject.CreatorId != default)
                {
                    return;
                }

                ObjectPropertyHelper.TrySetProperty(mayHaveCreatorObject, x => x.CreatorId, () => _userSession.UserId);
            }
        }


        protected virtual void SetDeletionTime(object targetObject)
        {
            if (targetObject is IHasDeletionTime objectWithDeletionTime)
            {
                if (objectWithDeletionTime.DeletionTime == null)
                {
                    ObjectPropertyHelper.TrySetProperty(objectWithDeletionTime, x => x.DeletionTime, () => DateTime.Now);
                }
            }
        }

        protected virtual void SetDeleterId(object targetObject)
        {
            if (!(targetObject is IDeletionAuditedObject deletionAuditedObject))
            {
                return;
            }

            if (deletionAuditedObject.DeleterId != null)
            {
                return;
            }

            if (_userSession.UserId.IsNullEmpty())
            {
                ObjectPropertyHelper.TrySetProperty(deletionAuditedObject, x => x.DeleterId, () => null);
                return;
            }
            ObjectPropertyHelper.TrySetProperty(deletionAuditedObject, x => x.DeleterId, () => _userSession.UserId);
        }

        protected virtual void SetIsDeleter(object targetObject, bool isDelete = false)
        {
            if (!(targetObject is ISoftDelete softDelete))
            {
                return;
            }

            ObjectPropertyHelper.TrySetProperty(softDelete, x => x.IsDeleted, () => isDelete);
        }
    }
}
