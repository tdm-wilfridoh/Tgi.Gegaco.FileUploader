using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tgi.Gegaco.FileUploader.Application.Features.Documentos.Commands.DeleteDocument
{
    public record DeleteDocumentCommand(Guid Id) : IRequest<bool>;
}
