using Ik_Bitirme.Domain.Entities;
using Ik_Bitirme.Domain.IRepositories;
using Ik_Bitirme.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ik_Bitirme.Infrastructure.Repositories
{
    public class DirectorRepo : BaseRepo<Director>, IDirectorRepo
    {
        public DirectorRepo(AppDbContext context) : base(context)
        {
        }
    }
}
