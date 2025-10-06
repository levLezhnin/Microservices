using Microsoft.AspNetCore.Mvc;
using UserApi.Api.Dto.Request;
using UserApi.Api.Dto.Response;
using UserApi.Api.Interfaces;

namespace UserApi.Api.Controller
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class UsersController
    {
        private readonly IUserControllerService _userControllerService;

        public UsersController(IUserControllerService userControllerService)
        {
            _userControllerService = userControllerService;
        }

        [HttpPost("create")]
        public async Task<UserResponseDto> registerUser([FromBody] CreateUserRequestDto createUserRequest)
        {
            return await _userControllerService.insert(createUserRequest);
        }

        [HttpPost("update")]
        public async Task<UserResponseDto> updateUser([FromBody] UpdateUserRequestDto updateUserRequestDto)
        {
            return await _userControllerService.update(updateUserRequestDto);
        }

        [HttpGet("{id}")]
        public async Task<UserResponseDto> findById([FromRoute] Guid id)
        {
            return await _userControllerService.findByIdOrThrowAsync(id);
        }

        [HttpGet("")]
        public async Task<UserResponseDto> findByEmail([FromQuery] string email)
        {
            return await _userControllerService.findByEmailOrThrowAsync(email);
        }
    }
}
