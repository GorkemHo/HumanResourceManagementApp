using AutoMapper;
using Ik_Bitirme.Application.Models.DTos.JobDtos;
using Ik_Bitirme.Application.Services.JobService;
using Ik_Bitirme.Domain.Entities;
using Ik_Bitirme.Domain.Enums;
using Ik_Bitirme.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ik_Bitirme.Application.Services.JobServices
{
    public class JobService : IJobService
    {
        private readonly IJobRepo _jobRepository;
        private readonly IMapper _mapper;
        public JobService(IJobRepo jobRepository, IMapper mapper)
        {
            _jobRepository = jobRepository;
            _mapper = mapper;
        }
        public async Task Create(JobDto jobDto)
        {
            var job = _mapper.Map<Job>(jobDto);
            await _jobRepository.CreateAsync(job);

        }

        public async Task Delete(int id)
        {
            var job = await _jobRepository.GetDefault(x => x.Id == id);
            job.Status=Status.Passive;
            await _jobRepository.UpdateAsync(job);
            
        }

        public async Task<List<JobDto>> GetAll()
        {
            var jobs= await _jobRepository.GetDefaults(
                expression:
                x => x.Status != Status.Passive
                
                );
            return _mapper.Map<List<JobDto>>(jobs);
            
        }

        public async Task<JobDto> GetById(string id)
        {
            var job = await _jobRepository.GetDefault(x => x.Id == Convert.ToInt32(id));
            return _mapper.Map<JobDto>(job);
        }

        public async Task Update(UpdateJobDto jobDto)
        {
            var job = _mapper.Map<Job>(jobDto);
            await _jobRepository.UpdateAsync(job);

        }
    }
}
