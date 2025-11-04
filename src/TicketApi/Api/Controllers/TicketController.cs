using Api.Dto;
using Contracts.Messages;
using Domain.Entities;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ICreateTicket _createTicket;
        private readonly IGetTicket _getTicket;
        private readonly IChangeTicketStatus _changeTicketStatus;
        private readonly IAssignTicket _assignTicket;
        private readonly IPublishEndpoint _publishEndpoint;

        public TicketController(ICreateTicket createTicket, IGetTicket getTicket, IChangeTicketStatus changeTicketStatus, IAssignTicket assignTicket, IPublishEndpoint publishEndpoint)
        {
            _createTicket = createTicket;
            _changeTicketStatus = changeTicketStatus;
            _getTicket = getTicket;
            _assignTicket = assignTicket;
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        public async Task CreateTicket([FromBody] CreateTicketRequestDto createTicketRequestDto)
        {
            await _publishEndpoint.Publish(
                new StartTicketCreation
                {
                    CorrelationId = Guid.NewGuid(),
                    CreatorId = createTicketRequestDto.creatorId,
                    Title = createTicketRequestDto.title,
                    Description = createTicketRequestDto.description
                }
            );
        }

        [HttpPut("{id}")]
        public async Task<Ticket> AssignTicket(Guid id, [FromBody] Guid assigneeId)
        {
            return await _assignTicket.assignToSupportAgent(id, assigneeId);
        }

        [HttpGet("{id}")]
        public async Task<Ticket> GetTicket(Guid id)
        {
            return await _getTicket.getTicketById(id);
        }
    }
}
