using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    /// <summary>
    /// Interact with db
    /// </summary>
    public interface IPeopleRepository
    {
        Task<ICollection<Persona>> GetAll();
        Task<Persona> Get(int id);
        Task Delete(int id);

        Task<Persona> Create(Persona persona);
        Task<Persona> Update(Persona persona);
    }
}
