using Ik_Bitirme.Application.Models.DTos.DepartmentDto;
using Ik_Bitirme.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ik_Bitirme.Application.Services.DepartmentService
{
    public interface IDepartmentService
    {
        Task Create(CreateDepartmentDto departmentDto);
        Task Delete(int id);
        Task<List<DepartmentDto>> GetAll();
        Task<DepartmentDto> GetById(int id);
        Task Update(UpdateDepartmentDto departmentDto);
    }
}
