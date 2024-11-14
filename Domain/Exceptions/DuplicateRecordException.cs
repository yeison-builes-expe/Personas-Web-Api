using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class DuplicateRecordException :Exception
    {
        public DuplicateRecordException(string cedula)
            : base($"La cédula {cedula} ya está registrada en el sistema.")
        {
        }
    }
}
