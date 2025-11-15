namespace RememberASP.Models;

public class LettersListVM
{
    public LettersDbContext dbContext { get; set; }

    public LettersListVM(LettersDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
}
