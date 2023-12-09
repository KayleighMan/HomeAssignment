using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentation.Models.ViewModels
{
    public class ListOfFlightsViewModel
    {
        public Guid Id { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }

        [Display(Name = "Departure Date & Time")]
        public DateTime DepartureDate { get; set; }

        [Display(Name = "Arrival Date & Time")]
        public DateTime ArrivalDate { get; set; }

        [Display(Name = "Country From")]
        public string CountryFrom { get; set; }

        [Display(Name = "Country Going To")]
        public string CountryTo { get; set; }
        public double WholesalePrice { get; set; }
        public double CommissionRate { get; set; }

        [Display(Name = "Retail Price")]
        public double retailPrice { get; set; }
    }
}
