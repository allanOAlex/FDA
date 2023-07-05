using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TB.Application.Abstractions.Interfaces;
using TB.Application.Abstractions.IServices;
using TB.Domain.Models;
using TB.Shared.Exceptions.ModelExceptions;
using TB.Shared.Requests.User;
using TB.Shared.Responses.User;

namespace TB.Infrastructure.Implementations.Services
{
    internal sealed class AppUserService : IAppUserService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly UserManager<AppUser> userManager;

        public AppUserService(IUnitOfWork UnitOfWork, IMapper Mapper, UserManager<AppUser> UserManager)
        {
            unitOfWork = UnitOfWork;
            mapper = Mapper;
            userManager = UserManager;
        }

        public async Task<CreateUserResponse> Create(CreateUserRequest createUserRequest)
        {
            try
            {
                createUserRequest.CreatedOn = DateTime.Now;
                createUserRequest.CreatedBy = 1;

                IEnumerable<AppUser> user = await unitOfWork.AppUser.FindAll();
                var existingUser = user.AsQueryable().Where(row =>
                EF.Functions.Like(row.UserName!, createUserRequest.UserName!) &&
                EF.Functions.Like(row.FirstName!, createUserRequest.FirstName!) &&
                EF.Functions.Like(row.LastName!, createUserRequest.LastName!) &&
                EF.Functions.Like(row.OtherNames!, createUserRequest.OtherNames!) &&
                EF.Functions.Like(row.Email, createUserRequest.Email!) &&
                EF.Functions.Like(row.PhoneNumber, createUserRequest.PhoneNumber!) 
                
                );

                if (existingUser.Any())
                    throw new CreatingDuplicateException("Duplicate User");


                var request = new MapperConfiguration(cfg => cfg.CreateMap<CreateUserRequest, AppUser>());
                var response = new MapperConfiguration(cfg => cfg.CreateMap<AppUser, CreateUserResponse>());

                IMapper requestMap = request.CreateMapper();
                IMapper responseMap = response.CreateMapper();

                var destination = requestMap.Map<CreateUserRequest, AppUser>(createUserRequest);
                var itemToCreate = await Task.FromResult(unitOfWork.AppUser.Create(destination));

                await unitOfWork.CompleteAsync();
                bool Successful = true;

                return Successful == true ? new CreateUserResponse { Successful = true, Message = "User created successfully!", FirstName = destination.FirstName, LastName = destination.LastName } : new CreateUserResponse { Successful = false, Message = "Error Creating User" };
            }
            catch (Exception )
            {
                throw ;
            }
        }

        public async Task<CreateUserResponse> CreateWithUserManager(CreateUserRequest createUserRequest)
        {
            try
            {
                createUserRequest.CreatedOn = DateTime.Now;
                createUserRequest.CreatedBy = 1;

                PasswordHasher<AppUser> ph = new PasswordHasher<AppUser>();

                var usernameExists = await userManager.FindByNameAsync(createUserRequest.UserName);
                var emailExists = await userManager.FindByEmailAsync(createUserRequest.Email);

                if (usernameExists != null || emailExists != null) { return new CreateUserResponse { Successful = false, Message = "Error|User already exists!" }; }

                var request = new MapperConfiguration(cfg => cfg.CreateMap<CreateUserRequest, AppUser>());
                var response = new MapperConfiguration(cfg => cfg.CreateMap<AppUser, CreateUserResponse>());

                IMapper requestMap = request.CreateMapper();
                IMapper responseMap = response.CreateMapper();

                var destination = requestMap.Map<CreateUserRequest, AppUser>(createUserRequest);
                destination.PasswordHash = ph.HashPassword(destination, destination.Password);
                destination.Password = destination.PasswordHash;
                AppUser userToCreate = await unitOfWork.AppUser.CreateWithUserManager(destination);
                var result = responseMap.Map<AppUser, CreateUserResponse>(userToCreate);

                await unitOfWork.CompleteAsync();
                result.Successful = true;
                await userManager.AddToRoleAsync(userToCreate, "User");

                return result.Successful == true ? new CreateUserResponse { Successful = true, Message = "User created successfully!" } : new CreateUserResponse { Successful = false, Message = "Error|Creating user failed" };

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DeleteUserResponse> Delete(DeleteUserRequest deleteUserRequest)
        {
            try
            {
                deleteUserRequest.IsDeleted = true;
                deleteUserRequest.DeletedOn = DateTime.Now;
                deleteUserRequest.DeletedBy = 1;

                var request = new MapperConfiguration(cfg => cfg.CreateMap<DeleteUserRequest, AppUser>());
                var response = new MapperConfiguration(cfg => cfg.CreateMap<AppUser, DeleteUserResponse>());

                IMapper requestMap = request.CreateMapper();
                IMapper responseMap = response.CreateMapper();

                var destination = requestMap.Map<DeleteUserRequest, AppUser>(deleteUserRequest);
                var eventToCreate = await Task.FromResult(unitOfWork.AppUser.Update(destination));

                try
                {
                    await unitOfWork.CompleteAsync();
                    bool Successful = true;
                    return Successful == true ? new DeleteUserResponse() { Successful = true, Message = "User deleted successfully!" } : new DeleteUserResponse() { Successful = false, Message = "Error deleting user" };
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    throw ex;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<GetAllUsersResponse>> FindAll()
        {
            try
            {
                List<GetAllUsersResponse> usersList = new();
                var users = await unitOfWork.AppUser.FindAll();
                if (users.Any())
                {
                    foreach (var item in users)
                    {
                        var listItem = new GetAllUsersResponse
                        {
                            Id = item.Id,
                            FirstName = item.FirstName,
                            LastName = item.LastName,
                            Email = item.Email,
                            Phone = item.PhoneNumber,
                        };

                        usersList.Add(listItem);
                    }

                    return usersList;
                }

                return usersList;
            }
            catch (Exception )
            {
                throw ;
            }
        }

        public async Task<GetUserByIdResponse> FindById(int Id)
        {
            try
            {
                var userToFind = await unitOfWork.AppUser.FindByCondition(e => e.Id == Id);
                if (userToFind.Any())
                {
                    var response = from e in userToFind
                                   select new GetUserByIdResponse
                                   {
                                       Successful = true,
                                       Message = $"User with Id {Id} found"
                                   };

                    return response.FirstOrDefault()!;
                }

                return new GetUserByIdResponse() { Successful = false, Message = $"User with Id {Id} not found." };
            }
            catch (Exception )
            {
                throw ;
            }
        }

        public async Task<UpdateUserResponse> Update(UpdateUserRequest updateUserRequest)
        {
            try
            {
                updateUserRequest.UpdatedOn = DateTime.Now;
                updateUserRequest.UpdatedBy = updateUserRequest.UpdatedBy;

                var entityResponse = await Task.FromResult(unitOfWork.AppUser.FindByCondition(e => e.Id == updateUserRequest.Id).Result.AsQueryable());

                var entity = entityResponse.AsNoTracking().FirstOrDefault();

                var request = new MapperConfiguration(cfg => cfg.CreateMap<UpdateUserRequest, AppUser>().ForMember(dest => dest.Id, opt => opt.Ignore()).ForAllMembers(opts => opts.Condition((src, dest, srcMember, destMember) => srcMember != null && !srcMember.Equals(destMember))));
                var response = new MapperConfiguration(cfg => cfg.CreateMap<AppUser, UpdateUserResponse>());

                IMapper requestMap = request.CreateMapper();
                IMapper responseMap = response.CreateMapper();

                var destination = requestMap.Map(updateUserRequest, entity);
                var userToUpdate = await Task.FromResult(unitOfWork.AppUser.Update(destination!));

                try
                {
                    await unitOfWork.CompleteAsync();
                    bool Successful = true;
                    return Successful == true ? new UpdateUserResponse() { Successful = true, Message = "User details updated successfully!" } : new UpdateUserResponse() { Successful = false, Message = "Error updating user" };
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var databaseValues = entry.GetDatabaseValues();
                    var clientValues = entry.CurrentValues;

                    if (databaseValues == null)
                    {
                        // The entity has been deleted from the database
                        // Handle this situation as appropriate
                    }
                    else
                    {
                        // The entity has been modified in the database
                        var databaseEntity = databaseValues.ToObject();
                        var clientEntity = clientValues.ToObject();

                        // Update the entity properties with the database values
                        foreach (var property in clientValues.Properties)
                        {
                            var databaseValue = databaseValues[property];
                            var clientValue = clientValues[property];

                            if (databaseValue != null && !databaseValue.Equals(clientValue))
                            {
                                clientValues[property] = databaseValue;
                            }
                        }

                        // Retry the update operation
                        await unitOfWork.CompleteAsync();
                    }
                    throw ex;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


    }


}
