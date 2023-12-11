namespace Z.Fantasy.Core.Authorization.Authorize
{
    public class SystemPermission : IDisposable
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public string ParentCode { get; set; }

        public bool IsGroup { get; set; }

        public bool Page { get; set; }

        public bool Button { get; set; }

        public List<SystemPermission> Childrens { get; set; }

        private bool Disposed { get; set; }

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
                if (Childrens != null)
                {
                    Childrens.Clear();
                    Childrens = null;
                }
            }
            Disposed = true;
        }
    }
}
