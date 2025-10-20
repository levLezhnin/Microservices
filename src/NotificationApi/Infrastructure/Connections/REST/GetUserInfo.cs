﻿using NotificationApi.Domain.Interfaces;
using UserConnectionLib.ConnectionServices.DtoModels.GetUserInfo;
using UserConnectionLib.ConnectionServices.Interfaces;

namespace NotificationApi.Infrastructure.Connections.REST
{
    public class GetUserInfo : IGetUserInfo
    {

        private readonly IUserConnectionService _userConnectionService;

        public GetUserInfo(IUserConnectionService userConnectionService)
        {
            _userConnectionService = userConnectionService;
        }

        public async Task<UserInfoDtoResponse> getUserInfo(Guid userId)
        {
            return await _userConnectionService.GetUserInfo(new UserInfoDtoRequest
            {
                userGuid = userId
            });
        }
    }
}
