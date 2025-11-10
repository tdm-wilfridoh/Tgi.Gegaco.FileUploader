using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tgi.Gegaco.FileUploader.Application.Common.Interfaces;
using Tgi.Gegaco.FileUploader.Domain.Entities;

namespace Tgi.Gegaco.FileUploader.Application.Features.Documentos.Queries.GetDocuments
{
    public class GetDocumentsHandler : IRequestHandler<GetDocumentsQuery, IEnumerable<Documento>>
    {
        //private readonly IFileStorageService _fileStorageService;

        private readonly IDocumentRepository _documentRepository;
        
        public GetDocumentsHandler(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
            //_fileStorageService = fileStorageService;
        }


        public async Task<IEnumerable<Documento>> Handle(GetDocumentsQuery request, CancellationToken cancellationToken)
        {
            // var listaDocumentos = _fileStorageService.GetAllFilesAsync();
            var documentos = await _documentRepository.GetAllAsync();

            return documentos;
        }
    }
}
