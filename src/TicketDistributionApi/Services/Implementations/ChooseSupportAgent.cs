using CoreLib.Exceptions;
using Services.Interfaces;
using UserConnectionLib.ConnectionServices.Interfaces;

namespace Services.Implementations
{
    public class ChooseSupportAgent : IChooseSupportAgent
    {
        private readonly IUserConnectionService _userConnectionService;

        public ChooseSupportAgent(IUserConnectionService userConnectionService)
        {
            _userConnectionService = userConnectionService;
        }

        public async Task<Guid> chooseSupportAgent()
        {
            Guid? agentId = await _userConnectionService.findMostFreeSupportAgent();
            if (!agentId.HasValue)
            {
                throw new ServiceException("Не удалось получить самого свободного агента");
            }
            return agentId.Value;
        }
    }
}
