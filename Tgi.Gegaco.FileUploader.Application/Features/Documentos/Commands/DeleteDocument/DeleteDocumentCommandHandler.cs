using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tgi.Gegaco.FileUploader.Application.Common.Interfaces;

namespace Tgi.Gegaco.FileUploader.Application.Features.Documentos.Commands.DeleteDocument
{
    public class DeleteDocumentCommandHandler : IRequestHandler<DeleteDocumentCommand, bool>
    {
        private readonly IFileStorageService _fileStorageService;

        public DeleteDocumentCommandHandler(IFileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }


        public Task<bool> Handle(DeleteDocumentCommand request, CancellationToken cancellationToken)
        {
            var resultado = _fileStorageService.DeleteFileAsync(request.Id);
            return resultado;
        }
    }
}
