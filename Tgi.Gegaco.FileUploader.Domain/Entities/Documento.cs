using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tgi.Gegaco.FileUploader.Domain.Entities
{
    public class Documento
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = default!;
        public string Ruta { get; set; } = default!;
        public string Extension { get; set; } = default!;
        public long Tamaño { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    }
}
