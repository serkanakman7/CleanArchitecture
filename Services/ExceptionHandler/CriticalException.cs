using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.ExceptionHandler
{
    public class CriticalException : Exception
    {
        public CriticalException()
        {
        }

        public CriticalException(string? message) : base(message)
        {
        }
    }
}
