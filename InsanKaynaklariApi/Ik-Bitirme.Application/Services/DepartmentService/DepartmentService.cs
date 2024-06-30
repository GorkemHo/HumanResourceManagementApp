using AutoMapper;
using Ik_Bitirme.Application.Models.DTos.DepartmentDto;
using Ik_Bitirme.Domain.Entities;
using Ik_Bitirme.Domain.IRepositories;
using Ik_Bitirme.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ik_Bitirme.Application.Services.DepartmentService
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepo _departmentRepo;
        private readonly IMapper _mapper;

        public DepartmentService(IDepartmentRepo departmentRepository, IMapper mapper)
        {
            _departmentRepo = departmentRepository;
            _mapper = mapper;
        }



        public async Task Create(CreateDepartmentDto departmentDto)
        {
            var department = _mapper.Map<Department>(departmentDto);
            await _departmentRepo.CreateAsync(department);
        }

        public async Task Delete(int id)
        {
            var department = await _departmentRepo.GetDefault(d => d.DepartmentId.Equals(id));
            if (department != null)
            {
                department.Status = Domain.Enums.Status.Passive;
                department.DeleteDate = DateTime.Now;
                await _departmentRepo.DeleteAsync(department);
            }
        }

        public async Task<List<DepartmentDto>> GetAll()
        {
            var departments = await _departmentRepo.GetDefaults(x=>true);
            return _mapper.Map<List<DepartmentDto>>(departments);
        }

        public async Task<DepartmentDto> GetById(int id)
        {
            var department = await _departmentRepo.GetDefault(d => d.DepartmentId == id);
            return _mapper.Map<DepartmentDto>(department);
        }

        public async Task Update(UpdateDepartmentDto departmentDto)
        {
            var existingDepartment = await _departmentRepo.GetDefault(d => d.DepartmentId.Equals(departmentDto.Id));

            existingDepartment.Name = departmentDto.Name;
            existingDepartment.UpdateDate = departmentDto.UpdateDate;
            existingDepartment.Status = departmentDto.Status;

            await _departmentRepo.UpdateAsync(existingDepartment);
        }
    }
}
