using ConsoleApp.Interfaces;

namespace ConsoleApp.Services;

public interface IMenuService
{
    void MainMenu();

}

public class MenuService : IMenuService
{
    private readonly IContactService _contactService = new ContactService();

    /// <summary>
    /// Metod för att visa Menyn med sina val som användaren kan välja utifrån.
    /// </summary>
    public void MainMenu()
    {
        while (true)
        {
            DisplayMenuTitle("MENU OPTIONS");
            Console.WriteLine($"{"1.", -3} Add New Contact");
            Console.WriteLine($"{"2.", -3} View Contact List");
            Console.WriteLine($"{"3.", -3} Display Contact Details");
            Console.WriteLine($"{"4.", -3} Remove Contact");
            Console.WriteLine($"{"5.", -3} Update Contact");
            Console.WriteLine($"{"0.", -3} Exit");
            Console.WriteLine();
            Console.Write("Enter Menu Option: ");
            var option = Console.ReadLine();


            switch ( option )
            {
                case "1":
                    IContact newContact = new Contact();
                    AddContactOption(newContact);
                    break;
                case "2":
                    ListContactOption();
                    break;
                case "3":
                    DisplayContactDetailsOption();
                    break;
                case "4":
                    RemoveContactOption();
                    break;
                case "5":
                    UpdateContactOption();
                    break;
                case "0":
                    ExitApplicationOption();
                    break;
                default: 
                    Console.WriteLine("\n Invalid Option selected. Press any key to try again");
                    break;
            }

            Console.ReadKey();
        }
    }
    /// <summary>
    ///  Metod där man lägger in de olika parametrarna man vill till kontaktinfon.
    /// </summary>
    /// <param name="contact"></param>
    private void AddContactOption(IContact contact)
    {

        DisplayMenuTitle("Add New Contact");

        Console.Write("First Name: ");
        contact.FirstName = Console.ReadLine()!;

        Console.Write("Last Name: ");
        contact.LastName = Console.ReadLine()!;

        Console.Write("Email: ");
        contact.Email = Console.ReadLine()!;

        Console.Write("Phonenumber: ");
        contact.PhoneNumber = Console.ReadLine()!;

        Console.Write("Address : ");
        contact.Address = Console.ReadLine()!;

        var res = _contactService.AddContactToList(contact);

        switch (res.Status)
        {
            case Enums.ServiceStatus.SUCCESSED:
                Console.WriteLine("The contact was added successfully");
            break;

            case Enums.ServiceStatus.ALREADY_EXISTS:
                Console.WriteLine("The contact already exists");
            break;
            
            case Enums.ServiceStatus.FAILED:
                Console.WriteLine("Failed when trying to add the contact to a contactlist.");
                Console.WriteLine("See error message : " + res.Result.ToString());
                break;

        }

        DisplayPressAnyKey();
    }

    /// <summary>
    /// Metod där man genom att ange en mailadress kan få info om den finns i listan eller ej. 
    /// </summary>
    private void ListContactOption()
    {
        DisplayMenuTitle("Contact List");
        Console.WriteLine("Enter email to get details: ");
        var email = Console.ReadLine();

        var contactToRetrieve = new Contact { Email = email };

        var res = _contactService.GetContactFromList(contactToRetrieve);

        if (res.Status == Enums.ServiceStatus.SUCCESSED)
        {
            var contact = res.Result as IContact;

            if (contact != null)
            {
                Console.WriteLine($"Name: {contact.FirstName} {contact.LastName}, Email: {contact.Email}");
            }
            else
            {
                Console.WriteLine("Contact not found.");
            }
        }
        else
        {
            Console.WriteLine("Failed to retrieve contact details.");
            Console.WriteLine("See error message: " + (res.Result is Exception ex ? ex.Message : "Unknown error"));
        }

        DisplayPressAnyKey();
        
    }

    /// <summary>
    /// Metod där man kan se alla kontakter i listan med sin information.
    /// </summary>
    private void DisplayContactDetailsOption()
    {
        DisplayMenuTitle("Contact List");
        var res = _contactService.GetContactsFromList();

        if (res.Status == Enums.ServiceStatus.SUCCESSED)
        {
            var contacts = res.Result as List<IContact>;

            if (contacts != null)
            {
                foreach (var contact in contacts)
                {
                    Console.WriteLine($"ID: {contact.Id}");
                    Console.WriteLine($"First Name: {contact.FirstName}");
                    Console.WriteLine($"Last Name: {contact.LastName}");
                    Console.WriteLine($"Email: {contact.Email}");
                    Console.WriteLine($"Phone Number: {contact.PhoneNumber ?? "N/A"}");
                    Console.WriteLine($"Address: {contact.Address}");
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("Failed to cast contacts. Result is not of type List<IContact>.");
            }
        }
        else
        {
            Console.WriteLine("Failed to retrieve the contact list.");
            Console.WriteLine("See error message: " + res.Result.ToString());
        }

        DisplayPressAnyKey();
    }

    /// <summary>
    /// Metod för att ta bort en kontakt från listan genom att skriva in mailadressen till denne.
    /// </summary>
    private void RemoveContactOption()
    {
        DisplayMenuTitle("Remove Contact");

        Console.Write("Enter the email of the contact to remove: ");
        var email = Console.ReadLine();

        var contactToRemove = new Contact { Email = email! };

        var res = _contactService.DeleteContactFromList(contactToRemove);

        switch (res.Status)
        {
            case Enums.ServiceStatus.SUCCESSED:
                Console.WriteLine("The contact was removed successfully");
                break;

            case Enums.ServiceStatus.NOT_FOUND:
                Console.WriteLine("Contact not found.");
                break;

            case Enums.ServiceStatus.FAILED:
                Console.WriteLine("Failed to remove the contact.");
                Console.WriteLine("See error message: " + res.Result.ToString());
                break;
        }

        DisplayPressAnyKey();
    }

    /// <summary>
    /// Satt med denna och såg sedan att den inte skulle vara med i uppgiften så har inte gjort klart den. Borde ta bort valet i menyn.
    /// </summary>
    //private void UpdateContactOption()
    //{
    //    DisplayMenuTitle("Update Contact");
    //    Console.Write("Enter the email of the contact to update: ");
    //    var email = Console.ReadLine();

    //    var contactToUpdate = new Contact { Email = email! };

    //    var existingContact = _contactService.GetContactFromList(contactToUpdate);

    //}


    /// Metod för att gå ur applikationen. 
    private void ExitApplicationOption()
    {
        Console.Clear();
        Console.WriteLine("Are you sure you want to exit to close this application? (y/n): ");
        var option = Console.ReadLine() ?? "";
        
        if (option.Equals("y", StringComparison.OrdinalIgnoreCase))
            Environment.Exit(0);
    }
    /// <summary>
    /// Metod som visar det valda menyvalets titel.
    /// </summary>
    /// <param name="title"></param>
    private void DisplayMenuTitle(string title)
    {
        Console.Clear();
        Console.WriteLine($"## {title} ##");
        Console.WriteLine();
    }

    /// <summary>
    /// Metod för att be användaren trycka på valfri knapp för att gå vidare.
    /// </summary>
    private void DisplayPressAnyKey()
    {
        Console.WriteLine("Press any key to continue.");
        Console.WriteLine();
    }
}

