namespace Z.Fantasy.Core.Authorization.Authorize
{
    public class AuthorizePermissionContext : IAuthorizePermissionContext
    {
        public SystemPermission Permission { get; private set; }

        public SystemPermission DefinePermission { get => Permission; }

        private bool Disposed { get; set; }
        public AuthorizePermissionContext()
        {
            if (Permission == null)
            {
                Permission = new SystemPermission()
                {
                    IsGroup = true,
                };
            }
        }

        public void AddGroup(string code, string name) => DefinePermission.AddGroup(code, name);

        public void CheckExists(string code)
        {
            var has = DefinePermission.Childrens.Any(p => p.Code == code);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Disposed)
            {
                return;
            }
            if (disposing)
            {
                if (Permission != null)
                {
                    Permission = null;
                }
            }
            Disposed = true;
        }
    }
}
