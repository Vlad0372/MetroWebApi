using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace MetroWebApi.Models
{
    public class TicketArchive
    {
        public int Id { get; set; }
        public string OwnerId { get; set; }

        [Column(TypeName = "date")]
        public DateTime DepartureDate { get; set; }
        //public DepartureDate DepartureDate { get; set; }
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
    }
}
