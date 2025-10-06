using UserApi.Api.Dto.Request;
using UserApi.Api.Dto.Response;

namespace UserApi.Api.Interfaces
{
    public interface IUserControllerService
    {
        Task<UserResponseDto> insert(CreateUserRequestDto createUserRequestDto);

        Task<UserResponseDto> update(UpdateUserRequestDto updateUserRequestDto);

        Task<UserResponseDto?> findByIdAsync(Guid id);
        Task<UserResponseDto> findByIdOrThrowAsync(Guid id);

        Task<UserResponseDto?> findByEmailAsync(string email);
        Task<UserResponseDto> findByEmailOrThrowAsync(string email);
    }
}
