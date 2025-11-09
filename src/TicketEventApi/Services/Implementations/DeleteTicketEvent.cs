using Domain.Entities;
using Domain.Interfaces;
using MongoDB.Bson;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class DeleteTicketEvent : IDeleteTicketEvent
    {
        private readonly ITicketEventRepository _ticketEventRepository;

        public DeleteTicketEvent(ITicketEventRepository ticketEventRepository)
        {
            _ticketEventRepository = ticketEventRepository;
        }

        public async Task<bool> deleteTicketEvent(ObjectId ticketEventId)
        {
            try
            {
                return await _ticketEventRepository.deleteByIdAsync(ticketEventId);
            } catch
            {
                return false;
            }
        }
    }
}
