using CoreLib.HttpLogic.Entities;
using CoreLib.HttpServiceV2.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserConnectionLib.ConnectionServices.DtoModels;
using UserConnectionLib.ConnectionServices.DtoModels.GetUserInfo;
using UserConnectionLib.ConnectionServices.DtoModels.GetUserInfoWithRole;
using UserConnectionLib.ConnectionServices.Interfaces;

namespace UserConnectionLib.ConnectionServices
{
    public class UserConnectionService : IUserConnectionService
    {

        private readonly IHttpRequestService _httpRequestService;

        public UserConnectionService(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _httpRequestService = serviceProvider.GetRequiredService<IHttpRequestService>();
        }

        public async Task<bool> addActiveTicket(Guid supportAgentId)
        {
            HttpRequestData requestData = new HttpRequestData()
            {
                Method = HttpMethod.Get,
                Uri = new Uri($"https://localhost:62461/api/v1/SupportMetrics/agent/{supportAgentId}/addTicket")
            };
            var response = await _httpRequestService.SendRequestAsync<SupportMetricsResponseDto>(
                requestData,
                new HttpConnectionData()
                {
                    Timeout = TimeSpan.FromSeconds(3)
                }
            );

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            return true;
        }

        public async Task<Guid?> findMostFreeSupportAgent()
        {
            HttpRequestData requestData = new HttpRequestData()
            {
                Method = HttpMethod.Get,
                Uri = new Uri($"https://localhost:62461/api/v1/SupportMetrics/free")
            };
            var response = await _httpRequestService.SendRequestAsync<Guid?>(
                requestData,
                new HttpConnectionData()
                {
                    Timeout = TimeSpan.FromSeconds(3)
                }
            );

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return response.Body;
        }

        public async Task<UserInfoDtoResponse?> GetUserInfo(UserInfoDtoRequest request)
        {
            HttpRequestData requestData = new HttpRequestData()
            {
                Method = HttpMethod.Get,
                Uri = new Uri($"https://localhost:62461/api/v1/Users/{request.userGuid.ToString()}/info")
            };
            var response = await _httpRequestService.SendRequestAsync<UserInfoDtoResponse?>(
                requestData,
                new HttpConnectionData()
                {
                    Timeout = TimeSpan.FromSeconds(3)
                }
            );

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return response.Body;
        }

        public async Task<UserInfoWithRoleResponse?> GetUserInfoWithRole(UserInfoWithRoleRequest request)
        {
            HttpRequestData requestData = new HttpRequestData()
            {
                Method = HttpMethod.Get,
                Uri = new Uri($"https://localhost:62461/api/v1/Users/{request.userGuid.ToString()}/infoWithRole")
            };
            var response = await _httpRequestService.SendRequestAsync<UserInfoWithRoleResponse?>(
                requestData,
                new HttpConnectionData()
                {
                    Timeout = TimeSpan.FromSeconds(3)
                }
            );

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return response.Body;
        }
    }
}
