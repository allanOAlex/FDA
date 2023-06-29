using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TB.Tests.NUnit.Abstraction.Tests
{
    public interface IDbConnectionFactory
    {
        IDbConnection GetConnection();
    }
}
