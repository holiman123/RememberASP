namespace RememberASP.Models;

public class LettersListVM
{
    public LettersAppDbContext dbContext { get; set; }

    public LettersListVM(LettersAppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
}
