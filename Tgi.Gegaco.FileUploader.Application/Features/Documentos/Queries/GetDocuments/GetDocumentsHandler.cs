using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tgi.Gegaco.FileUploader.Application.Common.Interfaces;
using Tgi.Gegaco.FileUploader.Application.Common.Models;
using Tgi.Gegaco.FileUploader.Domain.Entities;

namespace Tgi.Gegaco.FileUploader.Application.Features.Documentos.Queries.GetDocuments
{
    public class GetDocumentsHandler : IRequestHandler<GetDocumentsQuery, Result<IEnumerable<Documento>>>
    {
        //private readonly IFileStorageService _fileStorageService;

        private readonly IDocumentRepository _documentRepository;
        private readonly ILogger<GetDocumentsHandler> _logger;

        
        public GetDocumentsHandler(IDocumentRepository documentRepository, ILogger<GetDocumentsHandler> logger)
        {
            _documentRepository = documentRepository;
            _logger = logger;
            //_fileStorageService = fileStorageService;
        }


        public async Task<Result<IEnumerable<Documento>>> Handle(GetDocumentsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Iniciando búsqueda de documentos.");

                // var listaDocumentos = _fileStorageService.GetAllFilesAsync();
                var documentos = await _documentRepository.GetAllAsync();

                _logger.LogInformation("Retornando {documentos} documentos.", documentos.Count());
                return Result<IEnumerable<Documento>>.Success(documentos);
            }
            catch (Exception ex) {
                _logger.LogError("Error retornando los documentos desde la base de datos: {error}", ex.Message);
                return Result<IEnumerable<Documento>>.Error("Error durante la consulta de los documentos.");
            }
        }
    }
}
