using CoreLib.Common;
using CoreLib.Exceptions;
using CoreLib.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Services.Interfaces;
using UserConnectionLib.ConnectionServices.DtoModels.GetUserInfoWithRole;

namespace Services.Implementations
{
    public class AssignTicket : IAssignTicket
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IGetUserInfoWithRole _getUserInfoWithRole;
        private readonly IMapper<UserInfoWithRoleResponse?, UserInfoWithRole?> _mapper;

        public AssignTicket(ITicketRepository ticketRepository, IGetUserInfoWithRole getUserInfoWithRole, IMapper<UserInfoWithRoleResponse?, UserInfoWithRole?> mapper)
        {
            _ticketRepository = ticketRepository;
            _getUserInfoWithRole = getUserInfoWithRole;
            _mapper = mapper;
        }

        public async Task<Ticket> assignToSupportAgent(Guid ticketId, Guid supportAgentId)
        {
            Ticket ticket = await _ticketRepository.findByIdOrThrowAsync(ticketId);
            UserInfoWithRole? userInfoWithRole = _mapper.map(await _getUserInfoWithRole.getUserInfoWithRole(supportAgentId));
            
            if (userInfoWithRole is null)
            {
                throw new ServiceException($"Не найден агент поддержки с id: {supportAgentId}");
            }

            if (userInfoWithRole.role != UserRoles.Support)
            {
                throw new ServiceException($"Не могу назначить исполнителем пользователя с id: {supportAgentId}, т.к. он не агент поддержки");
            }

            ticket.assignedSupportAgentId = supportAgentId;
            ticket.status = TicketStatuses.assigned;
            ticket.updatedAt = DateTime.UtcNow;

            return await _ticketRepository.update(ticket);
        }
    }
}
