using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuPlan.Infrastruktur
{
    public class ServiceException : Exception
    {
        public ServiceException(string message, Exception innerException) : base (message, innerException)
        {
        }
    }
}
