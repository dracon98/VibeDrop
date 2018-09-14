using Firebase.Database;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VibeDrop.Model;
using System;
using Firebase.Database.Query;

namespace VibeDrop.Data
{
    public class DBFireBase
    {
        FirebaseClient client;
        Contact nContact = new Contact();
        public DBFireBase()
        {
            client = new FirebaseClient("https://vibedrop-b4589.firebaseio.com/");
            //Manual Entry

        }
        public async Task<List<Contact>> FetchContactAsync()
        {
            var response = (await client
                .Child("Contacts")
                .OnceAsync<Contact>())
                .Select(item => new Contact
                {
                    ID = item.Object.ID,
                    Name = item.Object.Name
                }).ToList();
            return response;
        }
    }
}
