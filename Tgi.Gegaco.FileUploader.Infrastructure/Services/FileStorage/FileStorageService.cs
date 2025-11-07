using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private readonly FileUploaderDbContext _context;

        public FileStorageService(IConfiguration configuration, FileUploaderDbContext context)
        {
            _storagePath = configuration["DocumentSettings:StoragePath"] ?? throw new Exception("No se encuentra definida la ruta de carga de los documentos.");
            _maxFileSize = long.Parse(configuration["DocumentSettings:MaxFileSize"] ?? throw new Exception("No se estableció el tamaño máximo permitido por documento."));
            _allowedFileTypes = configuration["DocumentSettings:AllowedExtensions"]?.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList() ?? throw new Exception("No se definieron las extensiones permitidas por documento.");
            _context = context;

            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
            }
        }

        public async Task<bool> DeleteFileAsync(Guid id)
        {
            var documento = await _context.Documentos.FindAsync(id);
            if (documento == null)
            {
                return false;
            }

            var filePath = Path.Combine(documento.Ruta);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            _context.Documentos.Remove(documento);
            await _context.SaveChangesAsync();
            return true;
        }

        public Task<IEnumerable<Documento>> GetAllFilesAsync()
        {
            var documentos = _context.Documentos.AsEnumerable();
            return Task.FromResult(documentos);
        }

        public async Task<Documento> GetFileByIdAsync(Guid id)
        {
            var documento = await _context.Documentos.FindAsync(id);
            return documento!;
        }

        public async Task<Documento> UploadFileAsync(IFormFile archivo, CancellationToken ct)
        {
            if (archivo == null) throw new InvalidOperationException("No se recibió ningún archivo.");
            if(archivo.Length > _maxFileSize | archivo.Length == 0) throw new InvalidOperationException($"El tamaño del archivo debe ser mayor a 0 y no debe exceder de {_maxFileSize} bytes.");

            var extension = Path.GetExtension(archivo.FileName);
            
            if (string.IsNullOrEmpty(extension) || !_allowedFileTypes.Contains(extension.ToLowerInvariant(), StringComparer.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException($"El tipo de archivo '{extension}' no está permitido.");
            }

            var documento = new Documento
            {
                Id = Guid.NewGuid(),
                Nombre = Path.GetFileNameWithoutExtension(archivo.FileName),
                Extension = extension,
                Tamaño = archivo.Length,
                FechaCreacion = DateTime.UtcNow
            };
            
            var filePath = Path.Combine(_storagePath, $"{documento.Id}{documento.Extension}");

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                archivo.CopyTo(stream);
            }

            documento.Ruta = filePath;

            _context.Documentos.Add(documento);
            await _context.SaveChangesAsync(ct);
            return documento;
        }
    }
}
