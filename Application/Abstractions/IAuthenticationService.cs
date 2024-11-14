using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    /// <summary>
    /// Interface for auth services
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Get bearer token
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        string GenerateToken(string username);
    }
}
