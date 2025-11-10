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
        Task<(bool Succes, string FilePath)> SaveFileAsync(IFormFile archivo, Guid id);
        Task<bool> DeleteFileAsync(string filePath);
        public bool ValidateFileExtension(string fileExtension);
        public bool ValidateFileSize(long fileSize);

    }
}
