using Microsoft.AspNetCore.Mvc;
using RememberASP.Models;
using System.Security.Policy;

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

        public IActionResult Add(string? letter, string? description)
        {
            if (letter is null && description is null)
                return View(new AddLetterModel());

            if (letter.Length != 1)
                return BadRequest(new
                {
                    message = "Length must be 1."
                });

            var newLetter = new LetterModel(letter[0], description);
            if (lettersDb.Letters.Contains(newLetter))
            {
                // Update description
                lettersDb.Letters.First(l => l.Letter == newLetter.Letter).Description = newLetter.Description;
            }
            else
            {
                lettersDb.Add(newLetter);
            }
            lettersDb.SaveChanges();

            string? urlStr = Url.Action("Letter", "Letters", new { Letter = newLetter.Letter });
            return Ok(new { url = urlStr });

            //return View("Letter", newLetter);
        }
    }
}
