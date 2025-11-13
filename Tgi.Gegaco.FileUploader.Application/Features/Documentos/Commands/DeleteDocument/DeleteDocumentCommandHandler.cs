using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tgi.Gegaco.FileUploader.Application.Common.Interfaces;
using Tgi.Gegaco.FileUploader.Application.Common.Models;

namespace Tgi.Gegaco.FileUploader.Application.Features.Documentos.Commands.DeleteDocument
{
    public class DeleteDocumentCommandHandler : IRequestHandler<DeleteDocumentCommand, Result<bool>>
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly IDocumentRepository _documentRepository;
        private readonly ILogger<DeleteDocumentCommandHandler> _logger;

        public DeleteDocumentCommandHandler(IFileStorageService fileStorageService, IDocumentRepository documentRepository, ILogger<DeleteDocumentCommandHandler> logger)
        {
            _fileStorageService = fileStorageService;
            _documentRepository = documentRepository;
            _logger = logger;
        }


        public async Task<Result<bool>> Handle(DeleteDocumentCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Iniciando proceso de eliminacion del documento: {DocumentId}", request.Id);


            var documento = await _documentRepository.GetByIdAsync(request.Id);
            
            if (documento == null)
            {
                _logger.LogWarning("El documento con Id {doc} no existe.", request.Id);
                return Result<bool>.Error("El documento a eliminar no existe.");
            }

            var borrado = await _fileStorageService.DeleteFileAsync(documento.Ruta);
            if (!borrado)
            {
                _logger.LogWarning("Error al intentar elimininar el documento {documento} - {id} del disco.", documento.Nombre, request.Id);
                return Result<bool>.Error("Error al intentar eliminar el documento.");
            }

            borrado = await _documentRepository.DeleteAsync(request.Id);
            if (!borrado)
            {
                _logger.LogWarning("Error al intentar eliminar el documento {documento} - {id} de la base de datos.", documento.Nombre, request.Id);
                return Result<bool>.Error("Error al intentar eliminar el documento.");
            }

            _logger.LogInformation("Documento {documento} - {id} eliminado.", documento.Nombre, request.Id);
            return Result<bool>.Success(true); ;
        }
    }
}
