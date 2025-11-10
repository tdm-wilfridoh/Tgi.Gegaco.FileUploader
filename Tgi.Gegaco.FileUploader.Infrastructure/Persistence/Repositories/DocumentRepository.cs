using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tgi.Gegaco.FileUploader.Application.Common.Interfaces;
using Tgi.Gegaco.FileUploader.Domain.Entities;

namespace Tgi.Gegaco.FileUploader.Infrastructure.Persistence.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly FileUploaderDbContext _context;

        public DocumentRepository(FileUploaderDbContext context)
        {
            _context = context;
        }


        public Task<Documento> AddAsync(Documento documento)
        {
            _context.Documentos.Add(documento);
            _context.SaveChanges();
            return Task.FromResult(documento);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var documento = await _context.Documentos.FindAsync(id);

            if (documento == null)
            {
                return false;
            }
            _context.Documentos.Remove(documento);
            await _context.SaveChangesAsync();
            return true;
        }

        public Task<IEnumerable<Documento>> GetAllAsync()
        {
            return Task.FromResult(_context.Documentos.AsEnumerable());
        }

        public async Task<Documento> GetByIdAsync(Guid id)
        {
            var documento = await _context.Documentos.FindAsync(id);
            return documento!;
        }
    }
}
