using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tgi.Gegaco.FileUploader.Application.Common.Interfaces;
using Tgi.Gegaco.FileUploader.Application.Common.Models;
using Tgi.Gegaco.FileUploader.Domain.Entities;

namespace Tgi.Gegaco.FileUploader.Application.Features.Documentos.Queries.GetDocumentById
{
    public class GetDocumentByIdHandler : IRequestHandler<GetDocumentByIdQuery, Result<Documento>>
    {
        //private readonly IFileStorageService _fileStorageService;


        private readonly IDocumentRepository _documentRepository;
        public GetDocumentByIdHandler(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
            //_fileStorageService = fileStorageService;
        }

        public async Task<Result<Documento>> Handle(GetDocumentByIdQuery request, CancellationToken cancellationToken)
        {
            var documento = await _documentRepository.GetByIdAsync(request.Id);
            if (documento == null)
                return Result<Documento>.Error($"No se encontró el documento para el Id {request.Id}");
            return Result<Documento>.Success(documento);
        }
    }
}
