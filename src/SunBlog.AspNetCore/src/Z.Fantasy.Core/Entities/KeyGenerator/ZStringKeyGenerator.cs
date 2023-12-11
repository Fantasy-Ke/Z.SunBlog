using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Z.Fantasy.Core.Entities.KeyGenerator
{
    public class ZStringKeyGenerator : ValueGenerator<string>
    {
        public override bool GeneratesTemporaryValues => false;

        public override string Next(EntityEntry entry)
        {
            return Guid.NewGuid().ToString("N"); // 使用 GUID 生成字符串主键
        }

        public override bool GeneratesStableValues => false;
    }
}
