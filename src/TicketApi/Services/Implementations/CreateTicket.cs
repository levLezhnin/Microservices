using Contracts.Messages;
using CoreLib.Common;
using CoreLib.Exceptions;
using CoreLib.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using MassTransit;
using Services.Interfaces;
using UserConnectionLib.ConnectionServices.DtoModels.GetUserInfoWithRole;

namespace Services.Implementations
{
    public class CreateTicket : ICreateTicket
    {
        private readonly IMapper<UserInfoWithRoleResponse?, UserInfoWithRole?> _mapper;
        private readonly IGetUserInfoWithRole _getUserInfo;
        private readonly ITicketRepository _ticketRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public CreateTicket(ITicketRepository ticketRepository, IPublishEndpoint publishEndpoint,IGetUserInfoWithRole getUserInfo, IMapper<UserInfoWithRoleResponse?, UserInfoWithRole?> mapper)
        {
            _ticketRepository = ticketRepository;
            _getUserInfo = getUserInfo;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Ticket> createAsync(Guid creatorId, string title, string? description)
        {
            UserInfoWithRole? info = _mapper.map(await _getUserInfo.getUserInfoWithRole(creatorId));

            if (info is null)
            {
                throw new ServiceException($"Пользователь с id: {creatorId} не найден!");
            }

            if (info.role != UserRoles.User)
            {
                throw new ServiceException($"Не могу создать тикет пользователю с id: {creatorId} т.к. он не пользователь.");
            }

            Ticket ticket = new()
            {
                creatorId = creatorId,
                title = title,
                description = description,
                status = TicketStatuses.pending,
                createdAt = DateTime.UtcNow
            };

            ticket = await _ticketRepository.insert(ticket);
            return ticket;
        }
    }
}
