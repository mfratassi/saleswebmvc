using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWebMVC.Models;
using Microsoft.EntityFrameworkCore; //Para include 

namespace SalesWebMVC.Services
{
    public class SellerService
    {
        private readonly SalesWebMVCContext _context;

        public SellerService(SalesWebMVCContext context)
        {
            _context = context;
        }

        public List<Seller> FindAll()
        {
            return _context.Seller.ToList();
        }

        public Seller FindById(int id)
        {
            return _context.Seller
                .Where(seller => seller.Id == id)
                .Include(seller => seller.Department)
                .FirstOrDefault();
        }

        public void Insert(Seller seller)
        {
            //seller.Department = _context.Department.First(); //Seller já possui Department apropriado
            _context.Add(seller);
            _context.SaveChanges();
        }

        public void Remove(int id)
        {
            Seller _seller = _context.Seller.Find(id);
            _context.Seller.Remove(_seller);
            _context.SaveChanges();
        }
    }
}
