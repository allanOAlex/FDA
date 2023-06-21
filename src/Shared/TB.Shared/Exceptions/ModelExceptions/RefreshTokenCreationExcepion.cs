using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TB.Shared.Exceptions.Global;

namespace TB.Shared.Exceptions.ModelExceptions
{
    public class RefreshTokenCreationExcepion : CustomException
    {
        public RefreshTokenCreationExcepion(string message = $"Could not create Refresh token.") : base(message: message)
        {

        }
    }
}
