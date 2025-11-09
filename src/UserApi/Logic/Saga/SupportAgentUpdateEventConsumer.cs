using Contracts.SupportAgentAssignMessages.SupportAgent;
using MassTransit;
using Microsoft.Extensions.Logging;
using UserApi.Logic.Interfaces;

namespace UserApi.Logic.Saga
{
    public class SupportAgentUpdateEventConsumer : IConsumer<SupportAgentUpdateEvent>
    {
        private readonly ISupportMetricsService _supportMetricsService;
        private readonly ILogger<SupportAgentUpdateEventConsumer> _logger;

        public SupportAgentUpdateEventConsumer(ILogger<SupportAgentUpdateEventConsumer> logger, ISupportMetricsService supportMetricsService)
        {
            _supportMetricsService = supportMetricsService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<SupportAgentUpdateEvent> context)
        {
            Guid agentId = context.Message.AgentId;
            try
            {
                _logger.LogInformation("Назначаю тикет агенту...");
                await _supportMetricsService.addActiveTicket(agentId);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Тикет не был назначен: {ex.Message}");

                await context.Publish(new SupportAgentUpdateFailed
                {
                    CorrelationId = context.Message.CorrelationId,
                    AgentId = agentId,
                    TicketId = context.Message.TicketId,
                    Reason = ex.Message
                });
                throw;
            }

            try
            {
                _logger.LogInformation("Отправляю событие SupportAgentUpdatedEvent...");

                await context.Publish(new SupportAgentUpdatedEvent
                {
                    CorrelationId = context.Message.CorrelationId,
                    AgentId = agentId,
                    TicketId = context.Message.TicketId
                });
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Не смог отправить событие: {ex.Message}");

                _logger.LogInformation("Откатываю назначение тикета");
                await _supportMetricsService.freeActiveTicket(agentId);

                await context.Publish(new SupportAgentUpdateFailed
                {
                    CorrelationId = context.Message.CorrelationId,
                    AgentId = agentId,
                    TicketId = context.Message.TicketId,
                    Reason = ex.Message
                });
                throw;
            }
        }
    }
}
