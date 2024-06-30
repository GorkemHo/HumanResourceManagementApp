using AutoMapper;
using Ik_Bitirme.Application.Models.DTos.ErrorLogDtos;
using Ik_Bitirme.Domain.Entities;
using Ik_Bitirme.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ik_Bitirme.Application.Services.ErrorLogServices
{
    public class ErrorLogService : IErrorLogService
    {
        public readonly IMapper _mapper;
        public readonly IErrorLogRepo _repo;

        public ErrorLogService(IMapper mapper, IErrorLogRepo repo)
        {
            _mapper = mapper;
            _repo = repo;
        }

        public async Task Create(CreateErrorLogDto errorLog)
        {
            var createErrorLog = _mapper.Map<ErrorLog>(errorLog);
            await _repo.CreateAsync(createErrorLog);
        }
    }
}
