using UserApi.Api.Dto.Request;
using UserApi.Api.Dto.Response;
using UserApi.Api.Interfaces;
using UserApi.Logic.Interfaces;
using UserApi.Logic.Models;
using CoreLib.Interfaces;

namespace UserApi.Api.Implementations
{
    public class UserControllerService : IUserControllerService
    {
        private readonly IUserService _userService;
        private readonly IDtoMapper<CreateUserRequestDto, UserLogic, UserResponseDto> _mapper;

        public UserControllerService(IUserService userService, IDtoMapper<CreateUserRequestDto, UserLogic, UserResponseDto> mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<UserResponseDto> insert(CreateUserRequestDto createUserRequestDto)
        {
            UserLogic logic = _mapper.toDomain(createUserRequestDto);
            return _mapper.toDto(await _userService.insert(logic));
        }

        public async Task<UserResponseDto> update(UpdateUserRequestDto updateUserRequestDto)
        {
            UserLogic updated = await _userService.update(
                updateUserRequestDto.id, 
                updateUserRequestDto.FirstName, 
                updateUserRequestDto.LastName, 
                updateUserRequestDto.isActive
            );
            return _mapper.toDto(updated);
        }

        public async Task<UserResponseDto?> findByEmailAsync(string email)
        {
            return _mapper.toDto(await _userService.findByEmailAsync(email));
        }

        public async Task<UserResponseDto> findByEmailOrThrowAsync(string email)
        {
            return _mapper.toDto(await _userService.findByEmailOrThrowAsync(email));
        }

        public async Task<UserResponseDto?> findByIdAsync(Guid id)
        {
            return _mapper.toDto(await _userService.findByIdAsync(id));
        }

        public async Task<UserResponseDto> findByIdOrThrowAsync(Guid id)
        {
            return _mapper.toDto(await _userService.findByIdOrThrowAsync(id));
        }
    }
}
