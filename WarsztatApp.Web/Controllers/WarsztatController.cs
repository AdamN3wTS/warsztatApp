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
            return View(new Zlecenie());
        }
        [HttpPost]
        public async Task<IActionResult> DodajZlecenie(Zlecenie zlecenie)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var warsztat = await _appDbContext.Warsztaty.Include(w => w.Magazyn).ThenInclude(m => m.Przedmioty).FirstOrDefaultAsync(w => w.UserId == user.Id);
                
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
                    WarsztatId = warsztat.Id,
                    stanZleceniaEnum = StanZleceniaEnum.Przyjęte,
                    ZleceniePrzedmioty = zleconiePrzedmioty,
                    
                };
                _appDbContext.Zlecenia.Add(zlecenieDb);
                await _appDbContext.SaveChangesAsync();
                Console.WriteLine("Zlecenia count w ifie: " + warsztat.Zlecenia?.Count);
                return RedirectToAction("Home");
            }
            var userFallback = await _userManager.GetUserAsync(User);
            var warsztatFallback = await _appDbContext.Warsztaty
                .Include(w => w.Magazyn)
                .ThenInclude(m => m.Przedmioty)
                .FirstOrDefaultAsync(w => w.UserId == userFallback.Id);

            ViewBag.Przedmioty = warsztatFallback?.Magazyn?.Przedmioty?.ToList() ?? new List<Przedmiot>();
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
            var zlecenieDb= await _appDbContext.Zlecenia.FindAsync(zlecenie.Id);

            _appDbContext.ZleceniePrzedmioty.RemoveRange(zlecenieDb.ZleceniePrzedmioty);
            
            zlecenieDb.ZleceniePrzedmioty = zlecenie.ZleceniePrzedmioty;
            zlecenieDb.Cena = zlecenie.Cena;
            zlecenieDb.Opis=zlecenie.Opis;
            zlecenieDb.stanZleceniaEnum = zlecenie.stanZleceniaEnum;
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Home");
            
            
        }

    }
}
