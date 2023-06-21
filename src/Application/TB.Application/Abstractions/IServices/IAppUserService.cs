using TB.Shared.Requests.User;
using TB.Shared.Responses.User;

namespace TB.Application.Abstractions.IServices
{
    public interface IAppUserService
    {
        Task<CreateUserResponse> Create(CreateUserRequest request);
        Task<CreateUserResponse> CreateWithUserManager(CreateUserRequest createUserRequest);
        Task<List<GetAllUsersResponse>> FindAll();
        Task<GetUserByIdResponse> FindById(int Id);
        Task<UpdateUserResponse> Update(UpdateUserRequest request);
        Task<DeleteUserResponse> Delete(DeleteUserRequest request);
    }
}
