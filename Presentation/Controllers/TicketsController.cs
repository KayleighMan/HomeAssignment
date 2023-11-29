using DataAccess.Repostories;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using Presentation.Models.ViewModels;

namespace Presentation.Controllers
{
    public class TicketsController : Controller
    {
        private FlightDBRepository _flightDBRepository;
        private TicketDBRepository _ticketDBRepository;
        public TicketsController(FlightDBRepository flightDBRepository , TicketDBRepository ticketDBRepository) 
        {
            _flightDBRepository = flightDBRepository;
            _ticketDBRepository = ticketDBRepository;

        }

        public IActionResult Index()
        {
            IQueryable<Flight> flightsQuery = _flightDBRepository.GetFlights();

            //To obtain the current date
            DateTime currentDateTime = DateTime.Now;

            var futureFlights = _flightDBRepository.GetFlights()
            .Where(flight => flight.DepartureDate > currentDateTime)
            .OrderBy(flight=> flight.DepartureDate)
            .ToList();

            var display = from Flight in futureFlights
                          select new ListOfFlightsViewModel()
                          {
                              Id = Flight.Id,
                              Rows = Flight.Rows,
                              Columns = Flight.Columns,
                              DepartureDate = Flight.DepartureDate,
                              ArrivalDate = Flight.ArrivalDate,
                              CountryFrom = Flight.CountryFrom,
                              CountryTo = Flight.CountryTo
                          };
            return View(display);
        }

        [HttpGet]
        public IActionResult Book() 
        {
            var flights = _flightDBRepository.GetFlights().ToList();
            BookAFlightTicketViewModel booking = new BookAFlightTicketViewModel(_flightDBRepository);

            return View(booking);
        }

        [HttpPost]
        public IActionResult Book(BookAFlightTicketViewModel booking, [FromServices] IWebHostEnvironment host, [FromRoute] Guid Id)
        {
            string relativePath = "";
            try
            {

                //Add validation 
                string filename = Guid.NewGuid() + System.IO.Path.GetExtension(booking.Passport.FileName);

                string absolutePath = host.WebRootPath + @"\images\" + filename;

                relativePath = @"/images/" + filename;

                using (FileStream fs = new FileStream(absolutePath, FileMode.CreateNew))
                {
                    booking.Passport.CopyTo(fs);
                    fs.Flush();
                    fs.Close();
                }

                var calcPricePaid = _flightDBRepository.GetFlight(Id);
                double commissionRate = calcPricePaid.CommissionRate;

                double wholeSalePricePaid = calcPricePaid.WholesalePrice;
                double calcPrice = wholeSalePricePaid + (wholeSalePricePaid * commissionRate);

                if (_ticketDBRepository.GetTickets().Where(x => x.Id == booking.Id).Count() == 0)
                {
                    _ticketDBRepository.Book(new Ticket()
                    {
                        Row = booking.Row,
                        Column = booking.Column,
                        FlightIdFK = Id,
                        Passport = relativePath,
                        PricePaid = calcPrice
                    });

                    TempData["message"] = "Ticket Booked Successfully";
                }
                else 
                {
                    return RedirectToAction("Index");
                }
            }

            catch 
            {
                booking.Flights = _flightDBRepository.GetFlights();
                TempData["Error"] = "Ticket was not booked successfully";
                return View(booking);
            }
            return View(booking);
        }



    }
}
