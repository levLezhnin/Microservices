using Contracts.SupportAgentAssignMessages.Init;
using Contracts.SupportAgentAssignMessages.SupportAgent;
using Contracts.SupportAgentAssignMessages.Ticket;
using Contracts.SupportAgentAssignMessages.TicketDistribution;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Services.Interfaces;

namespace Services.Messaging.Saga
{
    public class TicketSaga : MassTransitStateMachine<TicketSagaState>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<TicketSaga> _logger;

        public TicketSaga(IServiceProvider serviceProvider, ILogger<TicketSaga> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;

            InstanceState(x => x.CurrentState);

            Event(() => StartTicketAssign);

            Event(() => AssignTicketEvent);
            Event(() => SupportAgentChosen);
            Event(() => TicketAssignFailed);

            Event(() => TicketUpdateEvent);
            Event(() => TicketUpdatedEvent);
            Event(() => TicketUpdateFailed);

            Event(() => SupportAgentUpdateEvent);
            Event(() => SupportAgentUpdatedEvent);
            Event(() => SupportAgentUpdateFailed);

            Initially(
                Ignore(AssignTicketEvent),
                Ignore(SupportAgentChosen),
                Ignore(TicketUpdateEvent),
                Ignore(TicketUpdatedEvent),
                Ignore(TicketUpdateFailed),
                Ignore(SupportAgentUpdateEvent),
                Ignore(SupportAgentUpdatedEvent),
                Ignore(SupportAgentUpdateFailed),

                When(StartTicketAssign)
                    .ThenAsync(async context =>
                    {
                        await context.Publish(new AssignTicketEvent
                        {
                            CorrelationId = context.Message.CorrelationId,
                            TicketId = context.Message.TicketId,
                        });
                    }
                    )
                    .TransitionTo(AwaitingAgentAssign)
            );

            During(AwaitingAgentAssign,
                Ignore(StartTicketAssign),
                Ignore(AssignTicketEvent),
                Ignore(TicketUpdateEvent),
                Ignore(TicketUpdatedEvent),
                Ignore(TicketUpdateFailed),
                Ignore(SupportAgentUpdateEvent),
                Ignore(SupportAgentUpdatedEvent),
                Ignore(SupportAgentUpdateFailed),

                When(SupportAgentChosen)
                    .ThenAsync(async context =>
                    {
                        Guid correlationId = context.Message.CorrelationId;
                        Guid ticketId = context.Message.TicketId;
                        Guid supportAgentId = context.Message.AgentId;
                        try
                        {
                            _logger.LogInformation("Обновляю тикет...");
                            using var scope = _serviceProvider.CreateScope();
                            IAssignTicket _assignTicket = scope.ServiceProvider.GetRequiredService<IAssignTicket>();
                            await _assignTicket.assignToSupportAgent(ticketId, supportAgentId);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning($"Что-то пошло не так: {ex.Message}");
                            await context.Publish(new TicketUpdateFailed
                            {
                                CorrelationId = correlationId,
                                TicketId = ticketId,
                                AgentId = supportAgentId,
                                Reason = ex.Message
                            });
                            throw;
                        }

                        await context.Publish(new SupportAgentUpdateEvent
                        {
                            CorrelationId = context.Message.CorrelationId,
                            TicketId = context.Message.TicketId,
                            AgentId = context.Message.AgentId
                        });
                    })
                    .TransitionTo(UpdateAgent),
                When(TicketAssignFailed)
                    .TransitionTo(Initial)
            );

            During(UpdateAgent,
                Ignore(StartTicketAssign),
                Ignore(AssignTicketEvent),
                Ignore(SupportAgentChosen),
                Ignore(TicketUpdateEvent),
                Ignore(TicketUpdatedEvent),
                Ignore(TicketUpdateFailed),
                Ignore(SupportAgentUpdateEvent),

                When(SupportAgentUpdatedEvent)
                    .TransitionTo(Success)
                    .Finalize(),

                When(SupportAgentUpdateFailed)
                    .ThenAsync(async context =>
                    {
                        using var scope = _serviceProvider.CreateScope();
                        IRemoveAgent removeAgent = scope.ServiceProvider.GetService<IRemoveAgent>();
                        await removeAgent.removeAgent(context.Message.TicketId);
                    })
                    .TransitionTo(Initial)
            );
        }

        public Event<StartTicketAssign> StartTicketAssign { get; private set; }

        public Event<AssignTicketEvent> AssignTicketEvent { get; private set; }
        public Event<SupportAgentChosen> SupportAgentChosen { get; private set; }
        public Event<TicketAssignFailed> TicketAssignFailed { get; private set; }

        public Event<TicketUpdateEvent> TicketUpdateEvent { get; private set; }
        public Event<TicketUpdatedEvent> TicketUpdatedEvent { get; private set; }
        public Event<TicketUpdateFailed> TicketUpdateFailed { get; private set; }

        public Event<SupportAgentUpdateEvent> SupportAgentUpdateEvent { get; private set; }
        public Event<SupportAgentUpdatedEvent> SupportAgentUpdatedEvent { get; private set; }
        public Event<SupportAgentUpdateFailed> SupportAgentUpdateFailed { get; private set; }

        public State AwaitingAgentAssign { get; private set; }
        public State UpdateTicket {  get; private set; }
        public State UpdateAgent { get; private set; }
        public State Success { get; private set; }
        public State Failed { get; private set; }
    }
}
