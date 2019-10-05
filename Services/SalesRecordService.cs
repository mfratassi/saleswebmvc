using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Models;

namespace SalesWebMVC.Services
{
    public class SalesRecordService
    {
        private readonly SalesWebMVCContext _context;

        public SalesRecordService(SalesWebMVCContext context)
        {
            _context = context;
        }

        public async Task <List<SalesRecord>> FindByDateAsync(DateTime? min, DateTime? max)
        {
            IQueryable<SalesRecord> result = from obj in _context.SalesRecord select obj;

            if (min.HasValue)
                result = result
                    .Where(sr => sr.Date >= min.Value);
            if (max.HasValue)
                result = result
                    .Where(sr => sr.Date <= max.Value);

            return await result
                .Include(sr => sr.Seller)
                .Include(sr => sr.Seller.Department)
                .OrderByDescending(sr => sr.Date)
                .ToListAsync();
        }


    }
}
