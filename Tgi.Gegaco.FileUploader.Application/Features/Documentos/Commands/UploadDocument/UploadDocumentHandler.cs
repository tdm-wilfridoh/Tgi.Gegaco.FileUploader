using MediatR;
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

        public UploadDocumentHandler(IFileStorageService fileStorageService, IDocumentRepository documentRepository)
        {
            _fileStorageService = fileStorageService;
            _documentRepository = documentRepository;
        }

        public async Task<Result<Documento>> Handle(UploadDocumentCommand request, CancellationToken cancellationToken)
        {
            if (request.archivo == null) return Result<Documento>.Error("No se recibió ningún archivo.");

            var extension = Path.GetExtension(request.archivo.FileName);

            if (_fileStorageService.ValidateFileSize(request.archivo.Length)) 
                return Result<Documento>.Error($"El tamaño del archivo debe ser mayor a 0 y no debe exceder el tamaño máximo de bytes permitido.");


            if (!_fileStorageService.ValidateFileExtension(extension))
                return Result<Documento>.Error($"El tipo de archivo '{extension}' no está permitido.");

            var id = Guid.NewGuid();
            var (guardado, rutaDocumento) = await _fileStorageService.SaveFileAsync(request.archivo, id);
            if (!guardado)
                return Result<Documento>.Error("No se pudo guardar el documento.");

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

            return Result<Documento>.Success(documentoGuardado);
        }
    }
}
