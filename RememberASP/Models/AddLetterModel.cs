namespace RememberASP.Models;

public class AddLetterModel
{
    public string ErrorDescription { get; set; } = "";

    public AddLetterModel(bool isError = false)
    {
        if (isError)
            ErrorDescription = "Letter must be a single character.";
    }
}
