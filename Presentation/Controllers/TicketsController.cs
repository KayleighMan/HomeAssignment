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
            try
            {
                IQueryable<Flight> flightsQuery = _flightDBRepository.GetFlights();

                //To obtain the current date
                DateTime currentDateTime = DateTime.Now;

                var futureFlights = _flightDBRepository.GetFlights()
                .Where(flight => flight.DepartureDate > currentDateTime)
                .OrderBy(flight => flight.DepartureDate)
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
                                  CountryTo = Flight.CountryTo,
                                  retailPrice = Flight.WholesalePrice * Flight.CommissionRate
                              };
                return View(display);
            }
            catch(Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index","Home");
            }
        }

        [HttpGet]
        public IActionResult Book(Guid Id) 
        {
            var flights = _flightDBRepository.GetFlights().ToList();

            var selectedSeat = flights.FirstOrDefault(f => f.Id == Id);

            var booking = new BookAFlightTicketViewModel(_flightDBRepository)
            {
                Seats = selectedSeat

            };




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

                if (booking.Row <= 0 || booking.Column <= 0)
                {
                    TempData["error"] = "Invalid seat or row selection.";

                    var flights = _flightDBRepository.GetFlights().ToList();

                    var selectedSeat = flights.FirstOrDefault(f => f.Id == Id);

                    booking.Seats = selectedSeat;
                    return View(booking);
                }
                if (_ticketDBRepository.IsSeatBooked(Id, booking.Row, booking.Column))
                {
                    TempData["error"] = "The selected seat is already booked.";

                    var flights = _flightDBRepository.GetFlights().ToList();

                    var selectedSeat = flights.FirstOrDefault(f => f.Id == Id);

                    booking.Seats = selectedSeat;
                    return View(booking);
                }

                var passportCheck = _ticketDBRepository.GetTickets().SingleOrDefault(p =>p.PassportNo == booking.PassportNo);
                if (passportCheck == null)
                {
                    // Book the seat
                    _ticketDBRepository.Book(new Ticket()
                    {
                        Row = booking.Row,
                        Column = booking.Column,
                        FlightIdFK = Id,
                        Passport = relativePath,
                        PricePaid = calcPrice,
                        PassportNo = booking.PassportNo
                    });

                    TempData["message"] = "Ticket Booked Successfully";
                    return RedirectToAction("Index");
                }
                else 
                {
                    TempData["error"] = "Passport is already being used";
                    var flights = _flightDBRepository.GetFlights().ToList();
                    var selectedSeat = flights.FirstOrDefault(f => f.Id == Id);
                    booking.Seats = selectedSeat;
                    return View(booking);
                }
            }
            catch (Exception ex)
            {
                //booking.Flights = _flightDBRepository.GetFlights();
                TempData["error"] = "Ticket was not booked successfully";
                var flights = _flightDBRepository.GetFlights().ToList();
                var selectedSeat = flights.FirstOrDefault(f => f.Id == Id);
                booking.Seats = selectedSeat;
                return View(booking);
            }
        }



    }
}
