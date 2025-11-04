namespace Services.Interfaces
{
    public interface IRemoveAgent
    {
        public Task removeAgent(Guid ticketId);
    }
}
