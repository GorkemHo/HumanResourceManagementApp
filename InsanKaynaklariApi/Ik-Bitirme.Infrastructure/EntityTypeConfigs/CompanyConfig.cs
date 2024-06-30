using Ik_Bitirme.Domain.Entities;
using Ik_Bitirme.Domain.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ik_Bitirme.Infrastructure.EntityTypeConfigs
{
    public class CompanyConfig : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.HasData(
                new Company
                {
                    CompanyId = 1,
                    Name = "A Company",
                    CreateDate = DateTime.Now,
                    Status = Status.Active,
                    
                }, new Company
                {
                    CompanyId = 2,
                    Name = "B Company",
                    CreateDate = DateTime.Now,
                    Status = Status.Active,

                }, new Company
                {
                    CompanyId = 3,
                    Name = "C Company",
                    CreateDate = DateTime.Now,
                    Status = Status.Active,

                }
            );
        }
    }
}
