namespace ConsoleApp.Interfaces
{
    public class Contact : IContact
    {
        //ID räknas från nr 1 och uppåt när det skapas en kontakt.
        private static int _idCounter = 1;

        public Contact()
        {
            // ger ett unikt ID för varje ny användare. Går ett nummer upp.
            Id = _idCounter++;
        }

        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? PhoneNumber { get; set; }

        public string Address { get; set; } = null!;
    }
}
