using Application.Abstractions;
using DataAccess.Contexts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class PeopleRepository : IPeopleRepository
    {
        private readonly ApplicationDbContext _context;

        public PeopleRepository(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<ICollection<Persona>> GetAll()
        {
            var result = await _context.Personas.ToListAsync();

            return result;
        }
    }
}
