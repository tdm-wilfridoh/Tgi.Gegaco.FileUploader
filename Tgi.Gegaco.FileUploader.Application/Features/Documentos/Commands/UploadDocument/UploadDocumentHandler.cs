using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tgi.Gegaco.FileUploader.Application.Common.Interfaces;
using Tgi.Gegaco.FileUploader.Domain.Entities;

namespace Tgi.Gegaco.FileUploader.Application.Features.Documentos.Commands.UploadDocument
{
    public class UploadDocumentHandler : IRequestHandler<UploadDocumentCommand, Documento>
    {
        private readonly IFileStorageService _fileStorageService;
        public UploadDocumentHandler(IFileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }

        public Task<Documento> Handle(UploadDocumentCommand request, CancellationToken cancellationToken)
        {
            var documento = _fileStorageService.UploadFileAsync(request.archivo, cancellationToken);
            return documento;
        }
    }
}
