using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class ResponseDTO
    {
        public object? data { get; set; }
        public int status { get; set; }
        public string? message { get; set; }
    }
}
