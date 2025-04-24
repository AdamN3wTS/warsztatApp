using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> Home()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return View("Error");
            }
            var warsztat = await _appDbContext.Warsztaty.FirstOrDefaultAsync(w => w.UserId == user.Id);
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
    }
}
