using Ik_Bitirme.Application.Models.DTos.ErrorLogDtos;
using Ik_Bitirme.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ik_Bitirme.Application.Services.ErrorLogServices
{
    public interface IErrorLogService
    {
        public Task Create(CreateErrorLogDto errorLog);
    }
}
