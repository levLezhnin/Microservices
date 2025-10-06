using CoreLib.Interfaces;
using UserApi.Api.Dto.Request;
using UserApi.Api.Dto.Response;
using UserApi.Logic.Models;

namespace UserApi.Api.Dto.Mapper
{
    public class CreateUserRequestDtoMapper : IDtoMapper<CreateUserRequestDto, UserLogic, UserResponseDto>
    {
        public UserLogic? toDomain(CreateUserRequestDto request)
        {
            return new UserLogic
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PasswordHash = request.PasswordHash,
                Role = request.Role,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
        }

        public UserResponseDto? toDto(UserLogic domain)
        {
            return new UserResponseDto
            {
                FirstName = domain.FirstName,
                LastName = domain.LastName,
                Email = domain.Email,
                Role = domain.Role
            };
        }
    }
}
