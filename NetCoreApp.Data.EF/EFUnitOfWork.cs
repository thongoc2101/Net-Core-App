using System;
using System.Collections.Generic;
using System.Text;
using NetCoreApp.Infrastructure.Interfaces;

namespace NetCoreApp.Data.EF
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public EfUnitOfWork(AppDbContext context)
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
