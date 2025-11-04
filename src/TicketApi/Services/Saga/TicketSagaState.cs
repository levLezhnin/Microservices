using MassTransit;

namespace Services.Messaging.Saga
{
    public class TicketSagaState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public Guid TicketId { get; set; }
        public string CurrentState { get; set; }
        public string? FailReason { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
