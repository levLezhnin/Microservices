using UserConnectionLib.ConnectionServices.DtoModels.GetUserInfo;

namespace UserConnectionLib.ConnectionServices.Interfaces
{
    public interface IUserConnectionService
    {
        Task<UserInfoDtoResponse> GetUserInfo(UserInfoDtoRequest request);
    }
}
