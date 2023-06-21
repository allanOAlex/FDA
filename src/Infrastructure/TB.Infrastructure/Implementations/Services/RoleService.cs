using TB.Application.Abstractions.IServices;
using TB.Shared.Requests.Role;
using TB.Shared.Responses.Role;

namespace TB.Infrastructure.Implementations.Services
{
    internal sealed class RoleService : IRoleService
    {
        public RoleService()
        {
            
        }

        public Task<AddToRoleResponse> AddToRole(AddToRoleRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
