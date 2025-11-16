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

        //public IActionResult Add()
        //{
        //    return View();
        //}

        public IActionResult Add(string? letter, string? description)
        {
            if (letter is null && description is null)
                return View(new AddLetterModel());

            if (letter.Length != 1)
                return View(new AddLetterModel(true));

            var newLetter = new LetterModel(letter[0], description);
            lettersDb.Add(newLetter);
            lettersDb.SaveChanges();

            return View("Letter", newLetter);
        }
    }
}
