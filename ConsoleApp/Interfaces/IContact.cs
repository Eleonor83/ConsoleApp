namespace ConsoleApp.Interfaces
{
    public interface IContact
    {
        string Address { get; set; }
        string Email { get; set; }
        string FirstName { get; set; }
        int Id { get; set; }
        string LastName { get; set; }
        string? PhoneNumber { get; set; }
    }
}