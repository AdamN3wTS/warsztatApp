using System.ComponentModel.DataAnnotations.Schema;

namespace WarsztatApp.Web.Models
{
    public class ZleceniePrzedmiot
    {
        
        
        public int? ZlecenieId { get; set; }
        public  Zlecenie? Zlecenie { get; set; }

        
        public int? PrzedmiotId { get; set; }
        public  Przedmiot? Przedmiot { get; set; }

        public int? IloscZuzyta { get; set; } 
        

    }
}
