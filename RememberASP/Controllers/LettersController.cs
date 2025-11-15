using Microsoft.AspNetCore.Mvc;
using RememberASP.Models;

namespace RememberASP.Controllers
{
    public class LettersController : Controller
    {
        private LettersDbContext lettersDb;

        public LettersController(LettersDbContext lettersDb)
        {
            this.lettersDb = lettersDb;
        }

        public IActionResult Index()
        {
            return View(new LettersListVM(lettersDb));
        }

        public IActionResult Letter(char letter)
        {
            return View(lettersDb.Letters.First(l => l.Letter == letter));
        }
    }
}
