namespace QuickNotes.Business.DTOs.User;

public class LoginUserRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public bool Persistent { get; set; }
    public bool Lock { get; set; }
}