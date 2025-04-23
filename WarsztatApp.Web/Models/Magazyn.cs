namespace WarsztatApp.Web.Models
{
    public class Magazyn
    {
        public int Id { get; set; }

        public List<Przedmiot> Przedmioty { get; set; } = new();
    }
}
