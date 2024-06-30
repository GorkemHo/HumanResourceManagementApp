using Ik_Bitirme.Application.Models.DTos.UserDtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IkMvc.Application.Service.UserService
{
    public interface IUserService
    {
        Task<string> Login(LoginDto model);
        Task<string> Register(RegisterDto model);
    }
}
