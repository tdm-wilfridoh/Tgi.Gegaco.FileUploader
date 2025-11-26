using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tgi.Gegaco.FileUploader.Application.Common.Dtos;
using Tgi.Gegaco.FileUploader.Application.Common.Interfaces;
using Tgi.Gegaco.FileUploader.Application.Common.Models;
using Tgi.Gegaco.FileUploader.Domain.Entities;

namespace Tgi.Gegaco.FileUploader.Application.Features.Documentos.Queries.GetDocuments
{
    public class GetDocumentsHandler : IRequestHandler<GetDocumentsQuery, Result<IEnumerable<DocumentoDto>>>
    {
        //private readonly IFileStorageService _fileStorageService;

        private readonly IDocumentRepository _documentRepository;
        private readonly ILogger<GetDocumentsHandler> _logger;
        private readonly IMapper _mapper;


        public GetDocumentsHandler(IDocumentRepository documentRepository, ILogger<GetDocumentsHandler> logger, IMapper mapper)
        {
            _documentRepository = documentRepository;
            _logger = logger;
            _mapper = mapper;
            //_fileStorageService = fileStorageService;
        }


        public async Task<Result<IEnumerable<DocumentoDto>>> Handle(GetDocumentsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Iniciando búsqueda de documentos.");

                // var listaDocumentos = _fileStorageService.GetAllFilesAsync();
                var documentos = await _documentRepository.GetAllAsync();
                var docDto = _mapper.Map<IEnumerable<DocumentoDto>>(documentos);
                _logger.LogInformation("Retornando {documentos} documentos.", documentos.Count());
                return Result<IEnumerable<DocumentoDto>>.Success(docDto);
            }
            catch (Exception ex) {
                _logger.LogError("Error retornando los documentos desde la base de datos: {error}", ex.Message);
                return Result<IEnumerable<DocumentoDto>>.Error("Error durante la consulta de los documentos.");
            }
        }
    }
}
