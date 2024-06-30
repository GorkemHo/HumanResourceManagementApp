using Ik_Bitirme.Application.Models.DTos.JobDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ik_Bitirme.Application.Services.JobService
{
    public interface IJobService
    {
        Task Create(JobDto jobDto);
        Task Update(UpdateJobDto jobDto);
        Task Delete(int id);
        Task<JobDto> GetById(string id);
        Task<List<JobDto>> GetAll();
    }
}
