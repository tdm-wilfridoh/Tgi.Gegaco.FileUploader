using MediatR;
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

        public DeleteDocumentCommandHandler(IFileStorageService fileStorageService, IDocumentRepository documentRepository)
        {
            _fileStorageService = fileStorageService;
            _documentRepository = documentRepository;
        }


        public async Task<Result<bool>> Handle(DeleteDocumentCommand request, CancellationToken cancellationToken)
        {
            var documento = await _documentRepository.GetByIdAsync(request.Id);
            
            if (documento == null)
            {
                return Result<bool>.Error("El documento no existe.");
            }

            var borrado = await _fileStorageService.DeleteFileAsync(documento.Ruta);
            if(!borrado)
                return Result<bool>.Error("Error al intentar eliminar el documento.");

            borrado = await _documentRepository.DeleteAsync(request.Id);
            if(!borrado)
                return Result<bool>.Error("Error al intentar eliminar el documento.");

            return Result<bool>.Success(true); ;
        }
    }
}
