using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TB.Shared.Exceptions.Global;

namespace TB.Shared.Exceptions.ModelExceptions
{
    public class FetchFinancialDataException : CustomException
    {
        public FetchFinancialDataException(string message = "Fetch Financial Data Request Failed", HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError) : base(message, null, httpStatusCode)
        {
        }
    }
}
