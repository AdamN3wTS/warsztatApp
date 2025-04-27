using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WarsztatApp.Web.Models;

namespace WarsztatApp.Web.Models
{
    public class Przedmiot
    {
        public int Id { get; set; }
        public string? Nazwa { get; set; }

        public decimal? Cena { get; set; }

        public int? Ilosc { get; set; }

        
        public int? MagazynId { get; set; }
        public  Magazyn? Magazyn { get; set; }

        [Display(Name ="Typ przedmiotu")]
        public PrzedmiotEnum? typPrzedmiotu { get; set; }
        public List<ZleceniePrzedmiot>? ZleceniePrzedmioty { get; set; } = new();
    }
}
