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

        public async Task<List<Seller>> FindAllAsync()
        {
            //return _context.Seller.ToList();
            return await _context.Seller.ToListAsync();
        }

        public async Task<Seller> FindByIdAsync(int id)
        {
            return await _context.Seller
                .Where(seller => seller.Id == id)
                .Include(seller => seller.Department)
                .FirstOrDefaultAsync().ConfigureAwait(false);
        }

        public async Task InsertAsync(Seller seller)
        {
            //seller.Department = _context.Department.First(); //Seller já possui Department apropriado
            _context.Add(seller);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task RemoveAsync(int id)
        {
            try
            {
                Seller seller = await _context.Seller.FindAsync(id);
                _context.Seller.Remove(seller);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {

                throw new IntegrityException("Can\'t delete seller because he/she has sales");
            }
            
        }

        public async Task UpdateAsync(Seller seller)
        {
            //Lança exceção se não existe seller
            if (!await _context.Seller.AnyAsync(s => s.Id == seller.Id))
                throw new NotFoundException(new string("Id não encontrada"));
            try
            {
                _context.Update(seller);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }
    }
}
