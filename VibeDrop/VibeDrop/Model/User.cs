//Create by Tammy Le, 14/9/2018
using System;
namespace VibeDrop.Model
{
    public class User
    {
        public int ID { get; set; }
        public String username { get; set; }
        public String password { get; set; }
        //empty constructor
        public User(){}
        //constructor
        public User(string Username, string Password )
        {
            this.username = Username;
            this.password = Password;
        }

        //validate the fields
        public bool CheckInfo()
        {
            return true;
        }
    }
}
