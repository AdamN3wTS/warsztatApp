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
        private readonly AppDbContext appDbContext;
        private readonly UserManager<IdentityUser> _userManager;
        public async Task<IActionResult> Warsztat()
        {
            var userId = _userManager.GetUserId(User);
            var warsztat = await appDbContext.Warsztaty.FirstOrDefaultAsync( s=> s.UserId ==userId);
            return View(warsztat);
        }
        [HttpPost]
        public async Task<IActionResult> Warsztat(Warsztat warsztat)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var warsztatFromDb = await appDbContext.Warsztaty.FirstOrDefaultAsync(s => s.UserId==userId);
                warsztatFromDb.Adres=warsztat.Adres;
                warsztatFromDb.Nazwa=warsztat.Nazwa;
                await appDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(warsztat);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
