using System.ComponentModel.DataAnnotations;

namespace WarsztatApp.Web.Models
{
    public enum PrzedmiotEnum
    {
        [Display(Name = "Opony")]
        Opony,

        [Display(Name = "Filtr powietrza")]
        FiltrPowietrza,

        [Display(Name = "Filtr kabinowy")]
        FiltrKabinowy,

        [Display(Name = "Filtr oleju")]
        FiltrOlej,

        [Display(Name = "Filtr paliwa")]
        FiltrPaliwa,

        [Display(Name = "Akumulator")]
        Akumulator,

        [Display(Name = "Gadżety")]
        Gadżety,

        [Display(Name = "Olej silnikowy")]
        OlejSilnikowy,

        [Display(Name = "Płyn hamulcowy")]
        PłynHamulcowy,

        [Display(Name = "Płyn chłodniczy")]
        PłynChłodniczy,

        [Display(Name = "Żarówki")]
        Żarówki,

        [Display(Name = "Świece zapłonowe")]
        ŚwieceZapłonowe,

        [Display(Name = "Świece żarowe")]
        ŚwieceŻarowe,

        [Display(Name = "Klocki hamulcowe")]
        KlockiHamulcowe,

        [Display(Name = "Tarcze hamulcowe")]
        TarczeHamulcowe,

        [Display(Name = "Amortyzatory")]
        Amortyzatory,

        [Display(Name = "Pióra wycieraczek")]
        PióraWycieraczek,

        [Display(Name = "Pompa paliwa")]
        PompaPaliwa,

        [Display(Name = "Rozrusznik")]
        Rozrusznik,

        [Display(Name = "Alternator")]
        Alternator,

        [Display(Name = "Łożyska koła")]
        ŁożyskaKoła,

        [Display(Name = "Czujniki ABS")]
        CzujnikiABS,

        [Display(Name = "Pompa wody")]
        PompaWody,

        [Display(Name = "Termostat")]
        Termostat,

        [Display(Name = "Chłodnica")]
        Chłodnica,

        [Display(Name = "Pas rozrządu")]
        PasRozrządu,

        [Display(Name = "Sprzęgło")]
        Sprzęgło,

        [Display(Name = "Pompa hamulcowa")]
        PompaHamulcowa
    }
}
