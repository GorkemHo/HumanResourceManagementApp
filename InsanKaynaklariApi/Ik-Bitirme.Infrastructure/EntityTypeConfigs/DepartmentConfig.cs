using Ik_Bitirme.Domain.Entities;
using Ik_Bitirme.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ik_Bitirme.Infrastructure.EntityTypeConfigs
{
    public class DepartmentConfig : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasData(
                new Department { DepartmentId = 1, Name = "Production", Status = Status.Active },
                new Department { DepartmentId = 2, Name = "IT", Status = Status.Active },
                new Department { DepartmentId = 3, Name = "Human Resources", Status = Status.Active },
                new Department { DepartmentId = 4, Name = "Finance", Status = Status.Active }
            );
        }
    }
}
