using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace WarsztatApp.Web.Models
{
    public class Zlecenie
    {
        public int Id { get; set; }

        public string Nazwa { get; set; }

        public string Opis { get; set; }

        public decimal Cena { get; set; }

        public StanZleceniaEnum stanZleceniaEnum { get; set; }

        public DateTime DataPrzyjecią { get; set; } = DateTime.Now;

        public DateTime DataZakończenia { get; set;}

        public List<ZleceniePrzedmiot> ZleceniePrzedmioty { get; set; } = new();

        
        public int WarsztatId { get; set; }

        public  Warsztat Warsztat { get; set; }
    }
}
