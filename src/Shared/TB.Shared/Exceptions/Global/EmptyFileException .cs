using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TB.Shared.Exceptions.Global
{
    public class EmptyFileException : CustomException
    {
        public EmptyFileException(string message = "File is empty", HttpStatusCode StatusCode = HttpStatusCode.InternalServerError) : base(message)
        {
        }
    }
}
