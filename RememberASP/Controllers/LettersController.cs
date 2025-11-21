using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RememberASP.Models;
using System.Security.Claims;
using System.Security.Policy;

namespace RememberASP.Controllers
{
    public class LettersController : Controller
    {
        private LettersAppDbContext lettersDb;
        private LettersListVM model;
        private ILogger<LettersController> logger;

        public LettersController(LettersAppDbContext lettersDb, ILogger<LettersController> logger)
        {
            this.lettersDb = lettersDb;
            model = new LettersListVM(lettersDb);
            this.logger = logger;
        }

        public IActionResult Index()
        {
            return View(model);
        }

        public IActionResult Letter(char letter)
        {
            return View(lettersDb.Letters.First(l => l.Letter == letter));
        }

        [Authorize]
        public IActionResult Add(string? letter, string? description)
        {
            if (letter is null || description is null)
            {
                if (letter is not null)
                {
                    ViewData["letter"] = letter;
                    ViewData["desc"] = lettersDb.Letters.First(l => l.Letter == letter[0]).Description;
                }
                return View(new AddLetterModel());
            }

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

                // Log update
                logger.LogInformation($"User {User.Identity.Name} updated letter '{newLetter.Letter}' description to: \"{newLetter.Description}\".");
            }
            else
            {
                lettersDb.Add(newLetter);

                logger.LogInformation($"User {User.Identity.Name} added new letter: '{newLetter.Letter}' - {newLetter.Description}.");
            }
            lettersDb.SaveChanges();

            string? urlStr = Url.Action("Letter", "Letters", new { Letter = newLetter.Letter });
            return Ok(new { url = urlStr });
        }

        [Authorize]
        public IActionResult Remove(char letter)
        {
            lettersDb.Remove(lettersDb.Letters.First(l => l.Letter == letter));
            lettersDb.SaveChanges();

            logger.LogInformation($"User {User.Identity.Name} removed new letter: '{letter}'.");

            return View("Index", model);
        }
    }
}
