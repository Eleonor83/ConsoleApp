using ConsoleApp.Interfaces;
using ConsoleApp.Services;

namespace ConsoleApp.Tests
{
    public class ContactService_Tests
    {
        [Fact]
        public void AddToListShould_AddOneContactToContactList_ThenReturnTrue()
        {
            //Arrange
            IContact contact = new Contact {FirstName = "Eli", LastName = "Nyberg", Email = "eli@domain.com", Address = "Blabla 15", PhoneNumber = "0120120112" };
            IContactService contactService = new ContactService();

            //Act
            var result = contactService.AddContactToList(contact);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(Enums.ServiceStatus.SUCCESSED, result.Status);
        }
    }
}
