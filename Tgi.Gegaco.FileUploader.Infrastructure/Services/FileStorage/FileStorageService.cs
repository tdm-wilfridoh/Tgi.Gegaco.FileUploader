using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Tgi.Gegaco.FileUploader.Application.Common.Interfaces;
using Tgi.Gegaco.FileUploader.Domain.Entities;
using Tgi.Gegaco.FileUploader.Infrastructure.Persistence;

namespace Tgi.Gegaco.FileUploader.Infrastructure.Services.FileStorage
{
    public class FileStorageService : IFileStorageService
    {
        private readonly string _storagePath;
        private readonly long _maxFileSize;
        private readonly List<string> _allowedFileTypes;

        public FileStorageService(IConfiguration configuration, IDocumentRepository documentRepository)
        {
            _storagePath = configuration["DocumentSettings:StoragePath"] ?? throw new Exception("No se encuentra definida la ruta de carga de los documentos.");
            _maxFileSize = long.Parse(configuration["DocumentSettings:MaxFileSize"] ?? throw new Exception("No se estableció el tamaño máximo permitido por documento."));
            _allowedFileTypes = configuration["DocumentSettings:AllowedExtensions"]?.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList() ?? throw new Exception("No se definieron las extensiones permitidas por documento.");

            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
            }

        }

        public Task<bool> DeleteFileAsync(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return Task.FromResult(true);
                }
                return Task.FromResult(false);

            }
            catch (Exception)
            {
                return Task.FromResult(false);
            }
        }

        public async Task<(bool Succes, string FilePath)> SaveFileAsync(IFormFile archivo, Guid id)
        {
            if (archivo == null) throw new InvalidOperationException("No se recibió ningún archivo.");
            if (archivo.Length > _maxFileSize | archivo.Length == 0) throw new InvalidOperationException($"El tamaño del archivo debe ser mayor a 0 y no debe exceder de {_maxFileSize} bytes.");

            var extension = Path.GetExtension(archivo.FileName);

            if (string.IsNullOrEmpty(extension) || !_allowedFileTypes.Contains(extension.ToLowerInvariant(), StringComparer.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException($"El tipo de archivo '{extension}' no está permitido.");
            }

            try
            {
                var filePath = Path.Combine(_storagePath, $"{id}{extension}");

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await archivo.CopyToAsync(stream);
                }

                return (true, filePath);
            }
            catch (Exception)
            {
                return (false, string.Empty);

            }
        }

        public bool ValidateFileExtension(string fileExtension) => !string.IsNullOrEmpty(fileExtension) && _allowedFileTypes.Contains(fileExtension.ToLowerInvariant(), StringComparer.OrdinalIgnoreCase);

        public bool ValidateFileSize(long fileSize) => fileSize > _maxFileSize || fileSize == 0;


    }
}
