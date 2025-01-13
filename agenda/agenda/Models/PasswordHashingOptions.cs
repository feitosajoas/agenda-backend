namespace agenda.Models;

public class PasswordHashingOptions
{
    public int SaltSize { get; set; }
    public int HashSize { get; set; }
    public int Iterations { get; set; }
}
