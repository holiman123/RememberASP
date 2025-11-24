using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace RememberASP.Models;

[PrimaryKey(nameof(Letter), [])]
public class LetterModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)] // Prevent auto-increment
    public char Letter { get; set; }

    public string Description { get; set; }

    public LetterModel(char letter, string description)
    {
        Letter = letter;
        Description = description;
    }
}
