using System;
using System.Collections.Generic;
using System.Text;
using NetCoreApp.Data.Entities;
using NetCoreApp.Data.IRepositories;

namespace NetCoreApp.Data.EF.Repositories
{
    public class ColorRepository: EfRepository<Color, int>, IColorRepository
    {
        public ColorRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
