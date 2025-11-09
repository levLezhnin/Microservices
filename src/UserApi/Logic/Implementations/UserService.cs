using CoreLib.Common;
using CoreLib.Exceptions;
using CoreLib.Interfaces;
using UserApi.Dal.Interfaces;
using UserApi.Dal.Models;
using UserApi.Logic.Interfaces;
using UserApi.Logic.Models;

namespace UserApi.Logic.Implementations
{
    internal class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISupportMetricsRepository _supportMetricsRepository;
        private readonly ITwoWayMapper<UserDal, UserLogic> _mapper;

        public UserService(IUserRepository userRepository, ISupportMetricsRepository supportMetricsRepository, ITwoWayMapper<UserDal, UserLogic> mapper)
        {
            _userRepository = userRepository;
            _supportMetricsRepository = supportMetricsRepository;
            _mapper = mapper;
        }

        public async Task<UserLogic> insert(UserLogic userLogic)
        {
            try
            {
                await _userRepository.findByEmailOrThrowAsync(userLogic.Email);
                throw new EntityExistsException($"Пользователь с email: {userLogic.Email} уже существует!");
            }
            catch (EntityNotFoundException ex) { }

            if (!Enum.TryParse(userLogic.Role, out UserRoles existingRoles))
            {
                throw new ArgumentException($"Роль с названием: {userLogic.Role} не найдена!");
            }

            UserDal dal = _mapper.mapBackward(userLogic);
            UserLogic created = _mapper.mapForward(await _userRepository.insert(dal));

            if (userLogic.Role.Equals(UserRoles.Support.ToString()))
            {
                await _supportMetricsRepository.insert(new SupportMetricsDal
                {
                    Id = Guid.NewGuid(),
                    SupportId = created.Id,
                    ActiveTickets = 0,
                    TimesRated = 0,
                    Rating = 0
                });
            }

            return created;
        }

        public async Task<UserLogic> update(Guid id, string? firstName, string? lastName, bool? isActive)
        {
            return _mapper.mapForward(await _userRepository.update(id, firstName, lastName, isActive));
        }

        public async Task<UserLogic?> findByIdAsync(Guid id)
        {
            return _mapper.mapForward(await _userRepository.findByIdAsync(id));
        }

        public async Task<UserLogic> findByIdOrThrowAsync(Guid id)
        {
            return _mapper.mapForward(await _userRepository.findByIdOrThrowAsync(id));
        }

        public async Task<UserLogic?> findByEmailAsync(string email)
        {
            return _mapper.mapForward(await _userRepository.findByEmailAsync(email));
        }

        public async Task<UserLogic> findByEmailOrThrowAsync(string email)
        {
            return _mapper.mapForward(await _userRepository.findByEmailOrThrowAsync(email));
        }
    }
}
