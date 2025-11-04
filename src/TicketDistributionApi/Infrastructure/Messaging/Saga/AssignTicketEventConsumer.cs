using Contracts.SupportAgentAssignMessages.TicketDistribution;
using CoreLib.Exceptions;
using MassTransit;
using Microsoft.Extensions.Logging;
using Services.Interfaces;

namespace Infrastructure.Messaging.Saga
{
    public class AssignTicketEventConsumer : IConsumer<AssignTicketEvent>
    {
        private readonly IChooseSupportAgent _chooseSupportAgent;
        private readonly ILogger<AssignTicketEventConsumer> _logger;

        public AssignTicketEventConsumer(ILogger<AssignTicketEventConsumer> logger, IChooseSupportAgent chooseSupportAgent)
        {
            _chooseSupportAgent = chooseSupportAgent;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<AssignTicketEvent> context)
        {
            Guid? agentId = null;
            try
            {
                _logger.LogInformation("Выбираю агента...");
                agentId = await _chooseSupportAgent.chooseSupportAgent();

                if (!agentId.HasValue)
                {
                    throw new ServiceException("Не удалось получить самого свободного агента");
                }

                _logger.LogInformation("Отправляю событие SupportAgentChosen...");
                await context.Publish(new SupportAgentChosen
                {
                    CorrelationId = context.Message.CorrelationId,
                    TicketId = context.Message.TicketId,
                    AgentId = agentId.Value
                });
            } 
            catch (Exception ex)
            {
                _logger.LogWarning($"Агент не был выбран: {ex.Message}");
                await context.Publish(new TicketAssignFailed
                {
                    CorrelationId = context.Message.CorrelationId,
                    TicketId = context.Message.TicketId
                });
                throw;
            }
        }
    }
}
