using CoreLib.Interfaces;
using UserApi.Dal.Interfaces;
using UserApi.Dal.Models;
using UserApi.Logic.Interfaces;
using UserApi.Logic.Models;

namespace UserApi.Logic.Implementations
{
    internal class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleRepository _repository;
        private readonly ITwoWayMapper<UserRoleDal, UserRoleLogic> _mapper;

        public UserRoleService(IUserRoleRepository repository, ITwoWayMapper<UserRoleDal, UserRoleLogic> mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<UserRoleLogic?> findByIdAsync(Guid id)
        {
            return _mapper.mapForward(await _repository.findByIdAsync(id));
        }

        public async Task<UserRoleLogic> findByIdOrThrowAsync(Guid id)
        {
            return _mapper.mapForward(await _repository.findByIdOrThrowAsync(id));
        }

        public async Task<UserRoleLogic?> findByRoleNameAsync(string roleName)
        {
            return _mapper.mapForward(await _repository.findByRoleNameAsync(roleName));
        }

        public async Task<UserRoleLogic> findByRoleNameOrThrowAsync(string roleName)
        {
            return _mapper.mapForward(await _repository.findByRoleNameOrThrowAsync(roleName));
        }
    }
}
