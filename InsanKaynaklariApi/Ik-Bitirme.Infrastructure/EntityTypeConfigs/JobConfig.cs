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

    public class JobConfig : IEntityTypeConfiguration<Job>
    {
        public void Configure(EntityTypeBuilder<Job> builder)
        {
            builder.HasData(
            new Job { Id = 1, Name = "Software Developer", Description = "Responsible for developing software applications and systems.", Status = Status.Active },
            new Job { Id = 2, Name = "Janitor", Description = "Responsible for cleaning and maintaining the cleanliness of the workplace.", Status = Status.Active },
            new Job { Id = 3, Name = "DevOps Engineer", Description = "Responsible for managing the infrastructure and deployment pipelines.", Status = Status.Active },
            new Job { Id = 4, Name = "Scrum Master", Description = "Responsible for facilitating Agile development teams and processes.", Status = Status.Active },
            new Job { Id = 5, Name = "HR Manager", Description = "Responsible for overseeing human resources activities and policies.", Status = Status.Active },
            new Job { Id = 6, Name = "Financial Analyst", Description = "Responsible for analyzing financial data and trends to provide insights.", Status = Status.Active },
            new Job { Id = 7, Name = "Customer Support Representative", Description = "Responsible for assisting customers with product inquiries and issues.", Status = Status.Active },
            new Job { Id = 8, Name = "Marketing Specialist", Description = "Responsible for developing and implementing marketing strategies.", Status = Status.Active }

            );
        }
    }
}
