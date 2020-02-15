using System;
using System.ComponentModel.DataAnnotations.Schema;



namespace MetroWebApi.Models
{
    public class TicketArchive
    {
        public int Id { get; set; }
        public string OwnerId { get; set; }

        [Column(TypeName = "date")]
        public DateTime DepartureDate { get; set; }
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
    }
}
