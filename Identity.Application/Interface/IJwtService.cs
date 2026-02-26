using Identity.Infrastructure.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Interface
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
