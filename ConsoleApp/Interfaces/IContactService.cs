using ConsoleApp.Models.Responses;

namespace ConsoleApp.Interfaces
{
    public interface IContactService
    {
        IServiceResult AddContactToList(IContact contact);

        IServiceResult GetContactFromList(IContact contact);
        IServiceResult GetContactsFromList();
        IServiceResult UpdateContactInList(IContact contact);
        IServiceResult DeleteContactFromList(IContact contact);

    }

}
