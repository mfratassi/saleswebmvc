using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWebMVC.Models;
using Microsoft.EntityFrameworkCore; //Para include 
using SalesWebMVC.Services.Exceptions;

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
            Seller seller = _context.Seller.Find(id);
            _context.Seller.Remove(seller);
            _context.SaveChanges();
        }

        public void Update(Seller seller)
        {
            //Lança exceção se não existe seller
            if (!_context.Seller.Any(s => s.Id == seller.Id))
                throw new NotFoundException(new string("Id não encontrada"));
            try
            {
                _context.Update(seller);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }
    }
}
