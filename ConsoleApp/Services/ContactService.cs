using ConsoleApp.Interfaces;
using ConsoleApp.Models.Responses;
using Newtonsoft.Json;
using System.Diagnostics;

namespace ConsoleApp.Services
{

    public class ContactService : IContactService

    {
        private readonly FileService _fileService = new FileService(@"C:\EC_Education\Csharp-Projects\ConsoleApp\content.json");
        private List<IContact> _contacts = new List<IContact>();

        /// <summary>
        /// Metod som lägger till en kontakt i lista och dubbelkollar så att mailadressen inte redan finns registrerad.
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>        
        public IServiceResult AddContactToList(IContact contact)
        {
            IServiceResult response = new ServiceResult();
            try
            {
                if (!_contacts.Any(x => x.Email == contact.Email))
                {
                    _contacts.Add(contact);
                    _fileService.SaveContentToFile(JsonConvert.SerializeObject(_contacts));
                    response.Status = Enums.ServiceStatus.SUCCESSED;
                }

                else
                {
                    response.Status = Enums.ServiceStatus.ALREADY_EXISTS;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                response.Status = Enums.ServiceStatus.FAILED;
                response.Result = ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Metod som raderar en kontakt från listan genom att jämföra mailadress med en existerande från listan och sedan tar bort den.
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        public IServiceResult DeleteContactFromList(IContact contact)
        {
            var response = new ServiceResult();
            try
            {
                var content = _fileService.GetContentFromFile();

                if (!string.IsNullOrEmpty(content))
                {
                    var deserializedContacts = JsonConvert.DeserializeObject<List<Contact>>(content);

                    if (deserializedContacts != null)
                    {
                        _contacts = deserializedContacts.Cast<IContact>().ToList();
                    }
                }

                var contactToDelete = _contacts.FirstOrDefault(x => x.Email == contact.Email);

                if (contactToDelete != null)
                {
                    _contacts.Remove(contactToDelete);
                    response.Status = Enums.ServiceStatus.SUCCESSED;
                }
                else
                {
                    response.Status = Enums.ServiceStatus.NOT_FOUND;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                response.Status = Enums.ServiceStatus.FAILED;
                response.Result = ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Metod som visar uppgifter till en specifik kontakt när man jämfört med deras emailadress om den finns i listan.
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        public IServiceResult GetContactFromList(IContact contact)
        {
            var response = new ServiceResult();
            try
            {
                var content = _fileService.GetContentFromFile();

                if (!string.IsNullOrEmpty(content))
                {
                    _contacts = JsonConvert.DeserializeObject<List<IContact>>(content)!;
                }

                var specificContact = _contacts.FirstOrDefault(c => c.Email == contact.Email);

                if (specificContact != null)
                {
                    response.Status = Enums.ServiceStatus.SUCCESSED;
                    response.Result = specificContact;
                }
                else
                {
                    response.Status = Enums.ServiceStatus.NOT_FOUND;
                    response.Result = null!;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                response.Status = Enums.ServiceStatus.FAILED;
                response.Result = ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Metod som läser in listan med kontakter som sedan kan användas för att visa upp listan.
        /// </summary>
        /// <returns></returns>
        public IServiceResult GetContactsFromList()
        {
            var response = new ServiceResult();
            try
            {
                var content = _fileService.GetContentFromFile();

                if (!string.IsNullOrEmpty(content))
                {
                    var deserializedContacts = JsonConvert.DeserializeObject<List<Contact>>(content);

                    if (deserializedContacts != null)
                    {
                        _contacts = deserializedContacts.Cast<IContact>().ToList();
                    }
                }

                response.Status = Enums.ServiceStatus.SUCCESSED;
                response.Result = _contacts.ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                response.Status = Enums.ServiceStatus.FAILED;
                response.Result = $"Failed to retrieve contacts. Error: {ex.Message}";
            }

            return response;
        }

        /// <summary>
        /// Metod som tillåter att man updaterar uppgifter i en specifik kontakt baserad på deras emailadress. 
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        public IServiceResult UpdateContactInList(IContact contact)
        {
            var response = new ServiceResult();
            try
            {
                var content = _fileService.GetContentFromFile();

                if (!string.IsNullOrEmpty(content))
                {
                    var deserializedContacts = JsonConvert.DeserializeObject<List<Contact>>(content);

                    if (deserializedContacts != null)
                    {
                        _contacts = deserializedContacts.Cast<IContact>().ToList();
                    }
                }

                var existingContact = _contacts.FirstOrDefault(x => x.Email == contact.Email);

                if (existingContact != null)
                {
                    existingContact.FirstName = contact.FirstName;
                    existingContact.LastName = contact.LastName;
                    existingContact.PhoneNumber = contact.PhoneNumber;
                    existingContact.Address = contact.Address;

                    _fileService.SaveContentToFile(JsonConvert.SerializeObject(_contacts));

                    response.Status = Enums.ServiceStatus.SUCCESSED;
                }
                else
                {
                    response.Status = Enums.ServiceStatus.NOT_FOUND;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                response.Status = Enums.ServiceStatus.FAILED;
                response.Result = ex.Message;
            }

            return response;
        }


    }


}
