using Microsoft.EntityFrameworkCore;

namespace RememberASP.Models;

[PrimaryKey(nameof(Letter), [])]
public class LetterModel
{
    public char Letter { get; set; }

    public string Description { get; set; }

    public LetterModel(char letter, string description)
    {
        Letter = letter;
        Description = description;
    }
}
