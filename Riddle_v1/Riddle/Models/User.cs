using Riddle.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riddle.Models
{
    public class User
    {
        private int id;
        private string name;
        private string firstName;
        private string userName;
        private string email;
        private string phone;
        private string password;
        private string admin;

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string UserName { get => userName; set => userName = value; }
        public string Email { get => email; set => email = value; }
        public string Phone { get => phone; set => phone = value; }
        public string Password { get => password; set => password = value; }
        public string Admin { get => admin; set => admin = value; }

        public User()
        {

        }

        public User(string userName)
        {
            UserName = userName;
        }

        public User(string name, string firstName, string userName, string email, string phone, string admin)
        {
            Name = name;
            FirstName = firstName;
            UserName = userName;
            Email = email;
            Phone = phone;
            Admin = admin;
        }

        public override string ToString()
        {
            return "Nom : " + Name + "Prénom : " + FirstName + "Pseudo : " + UserName + " - Adresse email : " + Email + " - Téléphone : " + Phone + " Administrateur " + Admin;

        }

        public bool UserExist()
        {
            return DataBase.Instance.UserExistDb(this);
        }

        public bool UserLogin()
        {
            return DataBase.Instance.UserExistDb(Email, Password, this);
        }

        public void AddUser()
        {
            DataBase.Instance.AddUserDb(this);
        }

        public static List<User> GetUserList()
        {
            return DataBase.Instance.GetUserListDb();
        }

    }
}
