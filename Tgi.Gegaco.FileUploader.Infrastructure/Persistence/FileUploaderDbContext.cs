using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tgi.Gegaco.FileUploader.Domain.Entities;

namespace Tgi.Gegaco.FileUploader.Infrastructure.Persistence
{
    public class FileUploaderDbContext : DbContext
    {
        public FileUploaderDbContext(DbContextOptions<FileUploaderDbContext> options) : base(options)
        {
        }


        public DbSet<Documento> Documentos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FileUploaderDbContext).Assembly);
        }


    }
}
