using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using Tgi.Gegaco.FileUploader.Application.Common.Interfaces;
using Tgi.Gegaco.FileUploader.Application.Common.Models;
using Tgi.Gegaco.FileUploader.Domain.Entities;

namespace Tgi.Gegaco.FileUploader.Application.Features.Documentos.Commands.UploadDocument
{
    public class UploadDocumentHandler : IRequestHandler<UploadDocumentCommand, Result<Documento>>
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly IDocumentRepository _documentRepository;
        private readonly ILogger<UploadDocumentHandler> _logger;

        public UploadDocumentHandler(IFileStorageService fileStorageService, IDocumentRepository documentRepository, ILogger<UploadDocumentHandler> logger)
        {
            _fileStorageService = fileStorageService;
            _documentRepository = documentRepository;
            _logger = logger;
        }

        public async Task<Result<Documento>> Handle(UploadDocumentCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Iniciando proceso de carga del documento: {documento}", request.archivo?.FileName);

            if (request.archivo == null)
            {
                _logger.LogWarning("No se recibió archivo.");
                return Result<Documento>.Error("No se recibió archivo para cargar.");
            }


            if (_fileStorageService.ValidateFileSize(request.archivo.Length))
            {
                _logger.LogWarning("Carga documental rechazada: archivo de {filesize} bytes fuera del rango permitido.", request.archivo.Length);
                return Result<Documento>.Error("El tamaño del archivo debe ser mayor a 0 y no debe exceder el tamaño máximo de bytes permitido.");
            }

            var extension = Path.GetExtension(request.archivo.FileName);

            if (!_fileStorageService.ValidateFileExtension(extension))
            {
                _logger.LogWarning("Carga documental rechazada: extensión {extension} no permitida", extension);
                return Result<Documento>.Error($"Tipo de archivo con extensión '{extension}' no permitido.");
            }
            try
            {
                var id = Guid.NewGuid();
                var (guardado, rutaDocumento, error) = await _fileStorageService.SaveFileAsync(request.archivo, id);
                if (!guardado)
                {
                    _logger.LogError("No se pudo guardar el documento en disco: {error}", error);
                    return Result<Documento>.Error("No se pudo guardar el documento.");
                }

                var documento = new Documento()
                {
                    Id = id,
                    Nombre = Path.GetFileNameWithoutExtension(request.archivo.FileName),
                    Extension = extension,
                    Ruta = rutaDocumento,
                    Tamaño = request.archivo.Length,
                    FechaCreacion = DateTime.Now
                };

                var documentoGuardado = await _documentRepository.AddAsync(documento);

                _logger.LogWarning("Documento {documento} - {id} guardado.", documento.Nombre, documento.Id);
                return Result<Documento>.Success(documentoGuardado);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error inesperado durante la carga del documento {documento}: {error}", request.archivo.FileName, ex.Message);
                return Result<Documento>.Error("Error durante la carga del documento.");
            }
        }
    }
}
