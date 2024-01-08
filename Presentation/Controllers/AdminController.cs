using DataAccess.Repostories;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models.ViewModels;

namespace Presentation.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private FlightDBRepository _flightDBRepository;
        private TicketDBRepository _ticketDBRepository;

        public AdminController(FlightDBRepository flightDBRepository, TicketDBRepository ticketDBRepository)
        {
            _flightDBRepository = flightDBRepository;
            _ticketDBRepository = ticketDBRepository;
        }
        public IActionResult Index()
        {
            var flights = _flightDBRepository.GetFlights();
            return View(flights);
        }

        public IActionResult GetTicket(Guid flightId)
        {
            var tickets = _ticketDBRepository.GetTickets().Where(t => t.Id == flightId).ToList();
            return View(tickets);
        }

        public IActionResult ViewTicket()
        {
            var tickets = _ticketDBRepository.GetTickets().ToList();
            var viewTicket = tickets.Select(t => new ListTicketsAdminViewModel
            {
                Id = t.Id,
                Row = t.Row,
                Column = t.Column,
                FlightIdFK = t.FlightIdFK,
                Cancelled = t.Cancelled,
                PassportNo = t.PassportNo,
                PricePaid = t.PricePaid


            }).ToList();

            return View(viewTicket);
        }

        public IActionResult ViewFlight()
        {
            var flights = _flightDBRepository.GetFlights().ToList();
            var viewFlight = flights.Select(Flight => new ListFlightsAdminViewModel
            {
                Id = Flight.Id,
                Rows = Flight.Rows,
                Columns = Flight.Columns,
                DepartureDate = Flight.DepartureDate,
                ArrivalDate = Flight.ArrivalDate,
                CountryFrom = Flight.CountryFrom,
                CountryTo = Flight.CountryTo,
                WholesalePrice = Flight.WholesalePrice,
                CommissionRate = Flight.CommissionRate,
                retailPrice = Flight.WholesalePrice * Flight.CommissionRate

            }).ToList();

            return View(viewFlight);
        }



    }
}


