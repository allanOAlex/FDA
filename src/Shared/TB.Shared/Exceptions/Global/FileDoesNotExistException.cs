using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TB.Shared.Exceptions.Global
{
    public class FileDoesNotExistException : CustomException
    {
        public FileDoesNotExistException(string message = "File does not exist", HttpStatusCode StatusCode = HttpStatusCode.InternalServerError) : base(message)
        {
        }
    }
}
