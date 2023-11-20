using DataAccess.DataContext;
using Domain.Models;
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

        /*
        public void Cancel(Ticket ticket) 
        {
            var originalTicket = GetTickets(ticket.Id);
            if (originalTicket != null)
            {
                originalTicket.Row = ticket.Row;
                originalTicket.WholesalePrice = product.WholesalePrice;
                originalTicket.Price = product.Price;
                originalTicket.Name = product.Name;
                originalTicket.Description = product.Description;
                originalTicket.CategoryFK = product.CategoryFK;
                originalTicket.Image = product.Image;
                
            }
        }
        */
    }
}
