using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tgi.Gegaco.FileUploader.Infrastructure.Models
{
    public class DocumentSettings
    {
        public const string SectionName = "DocumentSettings";
        public List<string> AllowedExtensions { get; set; } = [];
        public long MaxFileSize { get; set; }
        public string StoragePath { get; set; } = string.Empty;
    }
}
