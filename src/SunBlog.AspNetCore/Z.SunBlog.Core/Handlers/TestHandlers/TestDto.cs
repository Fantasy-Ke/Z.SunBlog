using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.EventBus.Attributes;

namespace Z.SunBlog.Core.Handlers.TestHandlers
{
    [EventDiscriptor("test", 1000, false)]
    public class TestDto
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
