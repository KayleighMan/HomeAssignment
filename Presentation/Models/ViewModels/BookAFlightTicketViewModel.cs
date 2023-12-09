using DataAccess.Repostories;
using Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentation.Models.ViewModels
{
    public class BookAFlightTicketViewModel
    {
        public BookAFlightTicketViewModel()
        {}
        public BookAFlightTicketViewModel(FlightDBRepository flightDBRepository)
        {
            Flights = flightDBRepository.GetFlights(); 
        }
        public Guid Id { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }

        public IQueryable<Flight> Flights { get; set; }
        public int FlightIdFK { get; set; }
        public IFormFile Passport { get; set; }
        public double PricePaid { get; set; }
        public bool Cancelled { get; set; }

        public Flight Seats { get; set; }

    }
}
