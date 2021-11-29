using System;
using System.Threading.Tasks;
using Fotoquest.Data;

namespace Fotoquest.Core.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Foto> Fotos { get; }
        Task Save();
    }
}
