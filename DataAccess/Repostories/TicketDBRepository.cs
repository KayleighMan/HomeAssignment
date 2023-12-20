using DataAccess.DataContext;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repostories
{
    public class TicketDBRepository
    {
        private AirlineDBContext _AirlineDBContext;
        public TicketDBRepository(AirlineDBContext airlineDBContext) 
        {
            _AirlineDBContext = airlineDBContext;
        }
        public IQueryable<Ticket> GetTickets()
        {
            return _AirlineDBContext.Tickets;
        }

        public Ticket? GetTicket(Guid id)
        {
            return _AirlineDBContext.Tickets.SingleOrDefault(x => x.Id == id);
        }

        public void Book(Ticket ticket)
        {
            _AirlineDBContext.Tickets.Add(ticket);
            _AirlineDBContext.SaveChanges();
        }

        public bool IsSeatBooked(Guid flightId, int row, int column)
        {
            return _AirlineDBContext.Tickets.Any(t => t.FlightIdFK == flightId && t.Row == row && t.Column == column);
        }

        public void CancelTicket(Guid id)
        {
            var ticket = _AirlineDBContext.Tickets.SingleOrDefault(x => x.Id == id);

            if (ticket != null)
            {
                ticket.Cancelled = true;
                _AirlineDBContext.SaveChanges();

                Console.WriteLine($"Ticket {id} canceled successfully. Seat released.");
            }
            else
            {
                Console.WriteLine($"Ticket {id} is not booked.");
            }
        }

    }
}
