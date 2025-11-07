using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tgi.Gegaco.FileUploader.Application.Common.Interfaces;
using Tgi.Gegaco.FileUploader.Domain.Entities;

namespace Tgi.Gegaco.FileUploader.Application.Features.Documentos.Queries.GetDocumentById
{
    public class GetDocumentByIdHandler : IRequestHandler<GetDocumentByIdQuery, Documento>
    {
        private readonly IFileStorageService _fileStorageService;

        public GetDocumentByIdHandler(IFileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }

        public Task<Documento> Handle(GetDocumentByIdQuery request, CancellationToken cancellationToken)
        {
            var documento = _fileStorageService.GetFileByIdAsync(request.Id);
            return documento;
        }
    }
}
