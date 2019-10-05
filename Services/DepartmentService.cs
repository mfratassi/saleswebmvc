using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Models;

namespace SalesWebMVC.Services
{
    public class DepartmentService
    {
        private readonly SalesWebMVCContext _context;
        
        public DepartmentService(SalesWebMVCContext context) 
        {
            _context = context;
        }

        public async Task<List<Department>> FindAllAsync()
        {
            //return _context.Department.OrderBy(d => d.Name).ToList();
            return await _context.Department.OrderBy(d => d.Name).ToListAsync();
        }
    }
}
