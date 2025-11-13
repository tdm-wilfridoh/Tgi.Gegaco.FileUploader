using Azure.Core;
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
using Tgi.Gegaco.FileUploader.Infrastructure.Models;
using Tgi.Gegaco.FileUploader.Infrastructure.Persistence;

namespace Tgi.Gegaco.FileUploader.Infrastructure.Services.FileStorage
{
    public class FileStorageService : IFileStorageService
    {
        //private readonly string _documentSettings.StoragePath;
        //private readonly long _documentSettings.MaxFileSize;
        //private readonly List<string>  _documentSettings.AllowedExtensions;
        private readonly DocumentSettings _documentSettings = new();

        public FileStorageService(IOptions<DocumentSettings> documentSettings, IDocumentRepository documentRepository)
        {
            var settings = documentSettings.Value;
            //_documentSettings.StoragePath = configuration["DocumentSettings:StoragePath"] ?? throw new Exception("No se encuentra definida la ruta de carga de los documentos.");
            //_documentSettings.MaxFileSize = long.Parse(configuration["DocumentSettings:MaxFileSize"] ?? throw new Exception("No se estableció el tamaño máximo permitido por documento."));
            // _documentSettings.AllowedExtensions = configuration["DocumentSettings:AllowedExtensions"]?.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList() ?? throw new Exception("No se definieron las extensiones permitidas por documento.");
            _documentSettings.StoragePath = settings.StoragePath;
            _documentSettings.MaxFileSize = settings.MaxFileSize;
            _documentSettings.AllowedExtensions = settings.AllowedExtensions;


            if (!Directory.Exists(_documentSettings.StoragePath))
            {
                Directory.CreateDirectory(_documentSettings.StoragePath);
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

        public async Task<(bool Succes, string FilePath, string errorMessage)> SaveFileAsync(IFormFile archivo, Guid id)
        {
            try
            {
                var extension = Path.GetExtension(archivo.FileName);

                var filePath = Path.Combine(_documentSettings.StoragePath, $"{id}{extension}");

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await archivo.CopyToAsync(stream);
                }

                return (true, filePath, string.Empty);
            }
            catch (Exception ex)
            {
                return (false, string.Empty, ex.Message);

            }
        }

        public bool ValidateFileExtension(string fileExtension) => !string.IsNullOrEmpty(fileExtension) &&  _documentSettings.AllowedExtensions.Contains(fileExtension.ToLowerInvariant(), StringComparer.OrdinalIgnoreCase);

        public bool ValidateFileSize(long fileSize) => fileSize > _documentSettings.MaxFileSize || fileSize == 0;


    }
}
