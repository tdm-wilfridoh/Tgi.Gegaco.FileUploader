using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tgi.Gegaco.FileUploader.Application.Common.Models;

namespace Tgi.Gegaco.FileUploader.Application.Features.Documentos.Commands.DeleteDocument
{
    public record DeleteDocumentCommand(Guid Id) : IRequest<Result<bool>>;
}
