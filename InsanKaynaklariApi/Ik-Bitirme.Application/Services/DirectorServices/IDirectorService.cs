using Ik_Bitirme.Application.Models.DTos.DirectorDtos;
using Ik_Bitirme.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ik_Bitirme.Application.Services.DirectorService
{
    public interface IDirectorService
    {
        Task<IdentityResult> Register(RegisterDirectorDto model);
        Task<bool> UpdateDirectorStatus(string userName, string status);
        Task UpdateDirector(UpdateDirectorDto model);
        Task<DirectorDto> GetByUserName(string userName);
        Task<IEnumerable<DirectorDto>> GetDirectors();
        Task<List<Director>> GetAllByCompany(int id);


    }
}
