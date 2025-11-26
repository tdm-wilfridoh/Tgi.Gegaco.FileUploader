using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tgi.Gegaco.FileUploader.Application.Common.Dtos
{
    public class DocumentoDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = default!;
        public string Extension { get; set; } = default!;
        public long Tamano { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
