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

namespace Tgi.Gegaco.FileUploader.Application.Features.Documentos.Queries.GetDocumentById
{
    public class GetDocumentByIdHandler : IRequestHandler<GetDocumentByIdQuery, Result<DocumentoDto>>
    {
        //private readonly IFileStorageService _fileStorageService;


        private readonly IDocumentRepository _documentRepository;
        private readonly ILogger<GetDocumentByIdHandler> _logger;
        private readonly IMapper _mapper;

        public GetDocumentByIdHandler(IDocumentRepository documentRepository, ILogger<GetDocumentByIdHandler> logger, IMapper mapper)
        {
            _documentRepository = documentRepository;
            _logger = logger;
            _mapper = mapper;
            //_fileStorageService = fileStorageService;
        }

        public async Task<Result<DocumentoDto>> Handle(GetDocumentByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Iniciando búsqueda del documento {id}", request.Id);

            var documento = await _documentRepository.GetByIdAsync(request.Id);

            if (documento == null)
            {
                _logger.LogError("El documento {id} no existe en la base de datos.", request.Id);
                return Result<DocumentoDto>.Error($"No se encontró el documento con el Id {request.Id}");
            }
            _logger.LogInformation("Retornando documento {documento} - {id}", documento.Nombre, documento.Id);
            var docDto = _mapper.Map<DocumentoDto>(documento);
            return Result<DocumentoDto>.Success(docDto);
        }
    }
}
