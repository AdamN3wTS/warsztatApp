using System.ComponentModel.DataAnnotations;
namespace WarsztatApp.Web.Models
{
    public enum StanZleceniaEnum
    {
        [Display(Name ="Przyjęte")]
        Przyjęte,
        [Display(Name = "W trakcie")]
        wTrakcie,
        [Display(Name = "Zakończone")]
        Zakończone,
    }
}
