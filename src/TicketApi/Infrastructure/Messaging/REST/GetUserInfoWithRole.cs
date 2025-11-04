using Domain.Interfaces;
using UserConnectionLib.ConnectionServices.DtoModels.GetUserInfoWithRole;
using UserConnectionLib.ConnectionServices.Interfaces;

namespace Infrastructure.Messaging.REST
{
    public class GetUserInfoWithRole : IGetUserInfoWithRole
    {
        private readonly IUserConnectionService _userConnectionService;

        public GetUserInfoWithRole(IUserConnectionService userConnectionService)
        {
            _userConnectionService = userConnectionService;
        }

        public async Task<UserInfoWithRoleResponse?> getUserInfoWithRole(Guid userId)
        {
            return await _userConnectionService.GetUserInfoWithRole(new UserInfoWithRoleRequest
            {
                userGuid = userId
            });
        }
    }
}
