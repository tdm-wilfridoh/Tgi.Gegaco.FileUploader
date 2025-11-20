using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Tgi.Gegaco.FileUploader.Application.Features.Documentos.Commands.DeleteDocument;
using Tgi.Gegaco.FileUploader.Application.Features.Documentos.Commands.UploadDocument;
using Tgi.Gegaco.FileUploader.Application.Features.Documentos.Queries.GetDocumentById;
using Tgi.Gegaco.FileUploader.Application.Features.Documentos.Queries.GetDocuments;

namespace Tgi.Gegaco.FileUploader.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentosController : ControllerBase
    {
        private readonly IMediator _mediator;
        public DocumentosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadDocument(IFormFile file)
        {
            var command = new UploadDocumentCommand(file);
            var result = await _mediator.Send(command);
            
            if(!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDocuments()
        {
            // Implementation for getting all documents can be added here
            
            var query = new GetDocumentsQuery();
            var documentos = await _mediator.Send(query);

            return Ok(documentos);
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetDocumentById(Guid id)
        {
            var query = new GetDocumentByIdQuery(id);
            var result = await _mediator.Send(query);
            if (!result.IsSuccess)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteDocument(Guid id)
        {
            var command = new DeleteDocumentCommand(id);
            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }


    }
}
