using System;
using System.Threading.Tasks;
using Fotoquest.Core.IRepository;
using Fotoquest.Data;

namespace Fotoquest.Core.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FotoDbContext _context;
        private IGenericRepository<Foto> _fotos;

        public UnitOfWork(FotoDbContext context)
        {
            _context = context;
        }
        public IGenericRepository<Foto> Fotos => _fotos ??= new GenericRepository<Foto>(_context);

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
