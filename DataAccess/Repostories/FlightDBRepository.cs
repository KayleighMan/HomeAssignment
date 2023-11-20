using DataAccess.DataContext;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repostories
{
    public class FlightDBRepository
    {
        private AirlineDBContext _AirlineDBContext;
        public FlightDBRepository(AirlineDBContext airlineDBContext)
        {
            _AirlineDBContext = airlineDBContext;
        }
        public IQueryable<Flight> GetFlights()
        {
            return _AirlineDBContext.Flights;
        }

        public Flight? GetFlight(Guid id)
        {
            return _AirlineDBContext.Flights.SingleOrDefault(x => x.Id == id);
        }

        
    }
}
