using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarsztatApp.Web.Models
{
    public class Warsztat
    {
        
        public int Id { get; set; }

        [Required]
        public string? Nazwa { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string? Adres { get; set; }

        public string UserId {get; set; }
        public IdentityUser User { get; set; }

        
        public int? MagazynId { get; set; }

        public Magazyn? Magazyn { get; set; }
    }
}
