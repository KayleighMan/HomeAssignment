using DataAccess.Repostories;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
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
            return View();
        }



    }
}


