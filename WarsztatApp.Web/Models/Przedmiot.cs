using System.ComponentModel.DataAnnotations.Schema;

namespace WarsztatApp.Web.Models
{
    public class Przedmiot
    {
        public string Id { get; set; }
        public string Name { get; set; }

        [ForeignKey("Magazyn")]
        public string MagazynId { get; set; }
        public virtual Magazyn Magazyn { get; set; }

        public TypPrzedmiotu typPrzedmiotu { get; set; }
    }
}
