using Microsoft.EntityFrameworkCore;
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


        public async Task<Documento> AddAsync(Documento documento)
        {
            await _context.Documentos.AddAsync(documento);
            await _context.SaveChangesAsync();
            return documento;
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

        public async Task<IEnumerable<Documento>> GetAllAsync()
        {
            return await _context.Documentos
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Documento?> GetByIdAsync(Guid id)
        {
            return await _context.Documentos
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == id);
        }
    }
}
