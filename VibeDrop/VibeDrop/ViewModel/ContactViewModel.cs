using VibeDrop.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using VibeDrop.Model;

namespace VibeDrop.ViewModel
{
    class ContactViewModel:INotifyPropertyChanged
    {
        DBFireBase db = new DBFireBase();
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<Contact> _contactList;
        public ObservableCollection<Contact> ContactList
        {
            get { return _contactList; }
            set
            {
                _contactList = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("ContactList"));
                }
            }
        }
        public async Task FetchDataAsync()
        {
            var list = await db.FetchContactAsync();
            ContactList = new ObservableCollection<Contact>(list);
        }
        public ContactViewModel()
        {
            FetchDataAsync();
        }
    }
}
