using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.EventBus.Attributes;

namespace Z.SunBlog.Core.Handlers.FileHandlers
{
    [EventDiscriptor("FileEvent")]
    public class FileEventDto
    {
        public Stream Stream { get; set; }
        public string File { get; set; }
        public string ContentType { get; set; }

        public FileEventDto(Stream stream = null, string file = "", string contentType = "")
        {
            Stream = stream;
            File = file;
            ContentType = contentType;
        }
    }
}
