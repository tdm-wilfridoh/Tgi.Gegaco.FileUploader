using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tgi.Gegaco.FileUploader.Domain.Entities;

namespace Tgi.Gegaco.FileUploader.Application.Common.Interfaces
{
    public interface IDocumentRepository
    {
        Task<Documento> AddAsync(Documento documento);
        Task<bool> DeleteAsync(Guid id);
        Task<Documento> GetByIdAsync(Guid id);
        Task<IEnumerable<Documento>> GetAllAsync();
    }
}
