using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using WarsztatApp.Web.Data;
using WarsztatApp.Web.Models;

namespace WarsztatApp.Web.Controllers
{
    public class WarsztatController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<IdentityUser> _userManager;
        public WarsztatController(AppDbContext appDbContext, UserManager<IdentityUser> userManager)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> Home()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return View("Error");

            var warsztat = await _appDbContext.Warsztaty
                .Include(w => w.Zlecenia)
                .FirstOrDefaultAsync(w => w.UserId == user.Id);

            return View(warsztat);
        }

        [HttpGet]
        public async Task<IActionResult> Profil()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return View("Error");
            }

            var warsztat = await _appDbContext.Warsztaty.FirstOrDefaultAsync(w => w.UserId == user.Id);
            if (warsztat == null)
            {
                return View("Error");
            }

            return View(warsztat);
        }
        [HttpPost]
        public async Task<IActionResult> Profil(Warsztat warsztat)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return View("Error");
                }
                var warsztatDb = await _appDbContext.Warsztaty.FirstOrDefaultAsync(w => w.UserId == user.Id);
                warsztatDb.Adres = warsztat.Adres;
                warsztatDb.Nazwa = warsztat.Nazwa;
                await _appDbContext.SaveChangesAsync();
                return RedirectToAction("Home", "Warsztat");
            }
            return View(warsztat);
        }
        [HttpGet]
        public async Task<IActionResult> Magazyn()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return View("Error");
            }
            var warsztat = await _appDbContext.Warsztaty.Include(w => w.Magazyn).ThenInclude(m => m.Przedmioty).FirstOrDefaultAsync(w => w.UserId == user.Id);
            return View(warsztat.Magazyn);
        }
        [HttpPost]
        public async Task<IActionResult> DodajPrzedmiot(Przedmiot przedmiot)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return View("Error");

                }
                var warsztatDb = await _appDbContext.Warsztaty.Include(w => w.Magazyn).ThenInclude(m => m.Przedmioty).FirstOrDefaultAsync(w => w.UserId == user.Id);
                Przedmiot przedmiotDb = new Przedmiot
                {
                    Nazwa = przedmiot.Nazwa,
                    Ilosc = przedmiot.Ilosc,
                    Cena = przedmiot.Cena,
                    MagazynId = warsztatDb.Magazyn.Id,
                    typPrzedmiotu = przedmiot.typPrzedmiotu,
                };
                await _appDbContext.Przedmioty.AddAsync(przedmiotDb);
                
                await _appDbContext.SaveChangesAsync();
                return RedirectToAction("Magazyn");
            }
            return View(przedmiot);
        }
        [HttpGet]
        public async Task<IActionResult> DodajPrzedmiot() 
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return View("Error");
            }
            var warsztat = await _appDbContext.Warsztaty.Include(w => w.Magazyn).ThenInclude(m => m.Przedmioty).FirstOrDefaultAsync(w => w.UserId == user.Id);
            return View(new Przedmiot());
        }
        [HttpGet]
        public async Task<IActionResult> DodajZlecenie()
        {
            var user = await _userManager.GetUserAsync(User);
            var warsztat = await _appDbContext.Warsztaty.Include(w => w.Magazyn).ThenInclude(m => m.Przedmioty).FirstOrDefaultAsync(w => w.UserId == user.Id);
            ViewBag.Przedmioty = warsztat?.Magazyn?.Przedmioty?.ToList() ?? new List<Przedmiot>();
            Console.WriteLine("Magazyn ID: " + warsztat?.Magazyn?.Id);
            Console.WriteLine("Przedmioty count: " + warsztat?.Magazyn?.Przedmioty?.Count);

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DodajZlecenie(Zlecenie zlecenie)
        {
            Console.WriteLine("Kliknąłeś Dodaj");
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var warsztat = await _appDbContext.Warsztaty.Include(w => w.Magazyn).ThenInclude(m => m.Przedmioty).FirstOrDefaultAsync(w => w.UserId == user.Id);
                zlecenie.WarsztatId = warsztat.Id;
                List<ZleceniePrzedmiot> zleconiePrzedmioty = new List<ZleceniePrzedmiot>();
                foreach(var item in warsztat.Magazyn.Przedmioty)
                {
                    var iloscString = Request.Form[$"iloscZuzyta_{item.Id}"];
                    var checkbox = Request.Form[$"przedmiot_{item.Id}"];
                    if (!string.IsNullOrEmpty(checkbox) && int.TryParse(iloscString, out int ilosc) && ilosc > 0)
                    {
                        zleconiePrzedmioty.Add(new ZleceniePrzedmiot
                        {
                            PrzedmiotId = item.Id,
                            IloscZuzyta = ilosc
                        });
                    }
                }
                Zlecenie zlecenieDb = new Zlecenie

                {
                    DataPrzyjecią = DateTime.Now,
                    Nazwa = zlecenie.Nazwa,
                    Opis = zlecenie.Opis,
                    stanZleceniaEnum = StanZleceniaEnum.Przyjęte,
                    ZleceniePrzedmioty = zleconiePrzedmioty,
                    WarsztatId=warsztat.Id
                    
                };
                _appDbContext.Zlecenia.Add(zlecenieDb);
                await _appDbContext.SaveChangesAsync();
                Console.WriteLine("Zlecenia count w ifie: " + warsztat.Zlecenia?.Count);
                return RedirectToAction("Home", "Warsztat");
            }
            Console.WriteLine("Jakiś Błąd wypierdoliło");
            var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
            foreach(var er in errors)
            {
                foreach (var e in er)
                {
                    Console.WriteLine(e.ErrorMessage);
                }
            }
            return View(zlecenie);
        }
        [HttpGet]
        public async Task<IActionResult> Edytuj(int id)
        {
            Console.WriteLine("Przychodzący ID: " + id);

            var zlecenie = await _appDbContext.Zlecenia
                .Include(i => i.ZleceniePrzedmioty)
                .ThenInclude(zp => zp.Przedmiot)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (zlecenie == null)
            {
                Console.WriteLine("Nie znaleziono zlecenia o ID: " + id);
                return NotFound();
            }

            return View(zlecenie);
        }

        [HttpPost]
        public async Task<IActionResult> Edytuj(Zlecenie zlecenie) 
        {
            var zlecenieDb = await _appDbContext.Zlecenia.Include(z => z.ZleceniePrzedmioty).FirstOrDefaultAsync(z => z.Id == zlecenie.Id);
            if (zlecenieDb.ZleceniePrzedmioty != null)
            {
                _appDbContext.ZleceniePrzedmioty.RemoveRange(zlecenieDb.ZleceniePrzedmioty);
            }
            if (zlecenie.ZleceniePrzedmioty != null)
            {
                foreach (var zp in zlecenie.ZleceniePrzedmioty)
                {
                    zp.ZlecenieId = zlecenie.Id; 
                    _appDbContext.ZleceniePrzedmioty.Add(zp);
                }
            }

            // Aktualizuj pozostałe dane
            zlecenieDb.Cena = zlecenie.Cena;
            zlecenieDb.Opis = zlecenie.Opis;
            zlecenieDb.stanZleceniaEnum = zlecenie.stanZleceniaEnum;
            if (zlecenieDb.stanZleceniaEnum == StanZleceniaEnum.Zakończone) 
            {
                zlecenieDb.DataZakończenia = DateTime.Now;
            }

            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("Home", "Warsztat");


        }
        [HttpPost]
        public async Task<IActionResult> Usun(int id)
        {
            var zlecenie = await _appDbContext.Zlecenia.Include(z=>z.ZleceniePrzedmioty).FirstOrDefaultAsync(z=>z.Id==id);
            if (zlecenie != null) 
            {
                _appDbContext.ZleceniePrzedmioty.RemoveRange(zlecenie.ZleceniePrzedmioty);
                _appDbContext.Zlecenia.Remove(zlecenie);
                await _appDbContext.SaveChangesAsync();
            }
            Console.WriteLine(id);
            return Redirect(Request.Headers["Referer".ToString()]);
        }
        [HttpGet]
        public async Task<IActionResult> HistoriaZlecen() 
        {
            var user = await _userManager.GetUserAsync(User);
            var warsztat = await _appDbContext.Warsztaty.FirstOrDefaultAsync(w => w.UserId == user.Id);
            var zlecenia = await _appDbContext.Zlecenia.Where(z => z.WarsztatId == warsztat.Id).ToListAsync();
            return View(zlecenia);
        }

    }
}
