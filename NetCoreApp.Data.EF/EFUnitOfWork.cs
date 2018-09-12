using System;
using System.Collections.Generic;
using System.Text;
using NetCoreApp.Infrastructure.Interfaces;

namespace NetCoreApp.Data.EF
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public EfUnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Dispose()
        {
            _context.Dispose();
        }

        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}
