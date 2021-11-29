using System;
using System.Threading.Tasks;
using Fotoquest.Core.DTOs;

namespace Fotoquest.Core.Services
{
    public interface IAuthManager
    {
        Task<bool> ValidateUser(LoginUserDTO userDTO);
        Task<string> CreateToken();
    }
}
