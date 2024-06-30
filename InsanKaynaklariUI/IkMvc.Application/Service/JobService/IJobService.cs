using IkMvc.Application.Model.Dto.JobDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IkMvc.Application.Service.JobService
{
    public interface IJobService
    {
        Task<List<JobDto>> GetAllJobs();

    }
}
