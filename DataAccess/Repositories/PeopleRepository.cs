using Application.Abstractions;
using DataAccess.Contexts;
using Domain.Exceptions;
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

        public async Task<Persona> Create(Persona persona)
        {
            var alreadyExists = await _context.Personas.SingleOrDefaultAsync(x => x.Cedula == persona.Cedula);

            if (alreadyExists != null)
            {
                throw new DuplicateRecordException(persona.Cedula);
            }
            await _context.Personas.AddAsync(persona);
            await _context.SaveChangesAsync();

            return persona;
        }

        public async Task Delete(int id)
        {
            var toDelete = await _context.Personas.SingleOrDefaultAsync(x => x.Id == id);
            if (toDelete == null)
            {
                throw new KeyNotFoundException($"Persona con ID {id} no encontrada.");
            }
            _context.Personas.Remove(toDelete);

            await _context.SaveChangesAsync();
        }

        public async Task<Persona> Get(int id)
        {
            var result = await _context.Personas.SingleOrDefaultAsync(x => x.Id == id);

            return result;
        }

        public async Task<ICollection<Persona>> GetAll()
        {
            var result = await _context.Personas.ToListAsync();

            return result;
        }

        public async Task<Persona> Update(Persona persona)
        {
            var toUpdate = await _context.Personas.SingleOrDefaultAsync(x => x.Id == persona.Id);
            if (toUpdate == null)
            {
                throw new KeyNotFoundException($"Persona con ID {persona.Id} no encontrada.");
            }
            if (toUpdate.Celular != persona.Celular)
                toUpdate.Celular = persona.Celular;

            if (toUpdate.Cedula != persona.Cedula)
                toUpdate.Cedula = persona.Cedula;

            if (toUpdate.Apellido != persona.Apellido)
                toUpdate.Apellido = persona.Apellido;

            await _context.SaveChangesAsync();
            return toUpdate;
        }
    }
}
