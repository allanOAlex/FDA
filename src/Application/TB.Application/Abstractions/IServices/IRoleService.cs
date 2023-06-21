using TB.Shared.Requests.Role;
using TB.Shared.Responses.Role;

namespace TB.Application.Abstractions.IServices
{
    public interface IRoleService
    {
        Task<AddToRoleResponse> AddToRole(AddToRoleRequest request);
    }
}
