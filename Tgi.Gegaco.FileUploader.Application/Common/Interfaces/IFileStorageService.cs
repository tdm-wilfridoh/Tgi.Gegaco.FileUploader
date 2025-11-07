using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Tgi.Gegaco.FileUploader.Domain.Entities;

namespace Tgi.Gegaco.FileUploader.Application.Common.Interfaces
{
    public interface IFileStorageService
    {
        Task<Documento> UploadFileAsync(IFormFile archivo, CancellationToken ct);
        Task<IEnumerable<Documento>> GetAllFilesAsync();
        Task<Documento> GetFileByIdAsync(Guid id);
        Task<bool> DeleteFileAsync(Guid id);
    }
}
