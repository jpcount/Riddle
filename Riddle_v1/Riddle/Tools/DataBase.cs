using Riddle.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Riddle.Tools
{
    public class DataBase
    {
        private static DataBase _instance = null;
        private static object _lock = new object();

        public static DataBase Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new DataBase();
                    }
                    return _instance;
                }
            }
        }

        private DataBase()
        {

        }

        //USER
        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public bool UserExistDb(User u)
        {
            bool retour;
            if (u.Email != null)
            {
                SqlCommand command = new SqlCommand("SELECT * FROM User2 WHERE Email=@Email", Connection.Instance);
                command.Parameters.Add(new SqlParameter("@Email", u.Email));
                Connection.Instance.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    u.Id = reader.GetInt32(0);
                    retour = true;
                }
                else
                {
                    retour = false;
                }
                reader.Close();
                command.Dispose();
                Connection.Instance.Close();
            }
            else
            {
                retour = false;
            }
            return retour;
        }

        public bool UserExistDb(string email, string password, User u)
        {
            bool retour = false;
            if (email != null)
            {
                MD5 md5Hash = MD5.Create();
                string passwordHash = GetMd5Hash(md5Hash, password);
                SqlCommand command = new SqlCommand("SELECT Id, Name, FirstName, UserName FROM User2 WHERE Email=@Email and Password=@Password", Connection.Instance);
                command.Parameters.Add(new SqlParameter("@Email", email));
                command.Parameters.Add(new SqlParameter("@Password", passwordHash));
                Connection.Instance.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    u.Id = reader.GetInt32(0);
                    u.Name = reader.GetString(1);
                    u.FirstName = reader.GetString(2);
                    u.UserName = reader.GetString(3);
                    retour = true;
                }
                else
                {
                    retour = false;
                }
                reader.Close();
                command.Dispose();
                Connection.Instance.Close();
            }
            else
            {
                retour = false;
            }
            return retour;
        }

        public void AddUserDb(User u)
        {
            SqlCommand command = new SqlCommand("INSERT INTO User2 (Name,FirstName,UserName,Email,Phone,Password) OUTPUT INSERTED.ID VALUES (@Name,@FirstName,@UserName,@Email,@Phone,@Password)", Connection.Instance);
            MD5 md5Hash = MD5.Create();
            string passwordHash = GetMd5Hash(md5Hash, u.Password);
            command.Parameters.Add(new SqlParameter("@Name", u.Name));
            command.Parameters.Add(new SqlParameter("@FirstName", u.FirstName));
            command.Parameters.Add(new SqlParameter("@UserName", u.UserName));
            command.Parameters.Add(new SqlParameter("@Email", u.Email));
            command.Parameters.Add(new SqlParameter("@Phone", u.Phone));
            command.Parameters.Add(new SqlParameter("@Password", passwordHash));
            Connection.Instance.Open();
            u.Id = (int)command.ExecuteScalar();
            command.Dispose();
            Connection.Instance.Close();
        }

        public List<User> GetUserListDb()
        {
            List<User> liste = new List<User>();
            SqlCommand command = new SqlCommand("SELECT * FROM User2", Connection.Instance);
            Connection.Instance.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                User u = new User { Id = reader.GetInt32(0), Name = reader.GetString(1), FirstName = reader.GetString(2), UserName = reader.GetString(3), Email = reader.GetString(4), Phone = reader.GetString(5), Admin = reader.GetString(7) };
                liste.Add(u);
            }
            reader.Close();
            command.Dispose();
            Connection.Instance.Close();
            return liste;
        }

        //CATEGORY
        public List<Category> LoadCategoryDb()
        {
            List<Category> liste = new List<Category>();
            SqlCommand command = new SqlCommand("SELECT * FROM Category", Connection.Instance);
            Connection.Instance.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Category c = new Category { Id = reader.GetInt32(0), Titre = reader.GetString(1) };
                liste.Add(c);
            }
            reader.Close();
            command.Dispose();
            Connection.Instance.Close();
            return liste;
        }

        public void AddCategoryDb(Category c)
        {
            SqlCommand command = new SqlCommand("INSERT INTO Category(Titre) OUTPUT INSERTED.ID VALUES(@Titre)", Connection.Instance);
            command.Parameters.Add(new SqlParameter("@Titre", c.Titre));
            Connection.Instance.Open();
            c.Id = (int)command.ExecuteScalar();
            command.Dispose();
            Connection.Instance.Close();
        }

        public void DeleteCategoryDb(Category c)
        {
            SqlCommand command = new SqlCommand("DELETE FROM Category WHERE Id = @id", Connection.Instance);
            command.Parameters.Add(new SqlParameter("@id", c.Id));
            Connection.Instance.Open();
            command.ExecuteNonQuery();
            Connection.Instance.Close();
        }

        public bool LookCategory(Category c)
        {
            bool retour = false;
            if (c.Titre != null)
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Category where Titre=@Titre", Connection.Instance);
                command.Parameters.Add(new SqlParameter("@Titre", c.Titre));
                Connection.Instance.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    c.Id = reader.GetInt32(0);
                    retour = true;
                }
                else
                {
                    retour = false;
                }
                reader.Close();
                command.Dispose();
                Connection.Instance.Close();
            }
            else
            {
                retour = false;
            }

            return retour;
        }

        //RIDDLE

        public void AddRiddleDb(Riddle1 r)
        {
            SqlCommand command = new SqlCommand("INSERT INTO Riddle (Titre, Enonce, Indice, Solution, DatePublication, IdCategory, IdUser) OUTPUT INSERTED.ID VALUES (@Titre, @Enonce, @Indice, @Solution, @DatePublication, @IdCategory, @IdUser)", Connection.Instance);
            command.Parameters.Add(new SqlParameter("@Titre", r.Titre));
            command.Parameters.Add(new SqlParameter("@Enonce", r.Enonce));
            command.Parameters.Add(new SqlParameter("@Indice", r.Indice));
            command.Parameters.Add(new SqlParameter("@Solution", r.Solution));
            command.Parameters.Add(new SqlParameter("@DatePublication", r.DatePublication));
            command.Parameters.Add(new SqlParameter("@IdCategory", r.IdCategory));
            command.Parameters.Add(new SqlParameter("@IdUser", r.IdUser));
            Connection.Instance.Open();
            r.Id = (int)command.ExecuteScalar();
            command.Dispose();
            foreach (RiddleImage img in r.Image)
            {
                command = new SqlCommand("INSERT INTO RiddleImage (Url,idRiddle) values(@Url,@idRiddle)", Connection.Instance);
                command.Parameters.Add(new SqlParameter("@Url", img.Url));
                command.Parameters.Add(new SqlParameter("@idRiddle", r.Id));
                command.ExecuteNonQuery();
                command.Dispose();
            }
            Connection.Instance.Close();
        }

        public List<Riddle1> RiddleLoadDb(int? idCategory)
        {
            List<Riddle1> liste = new List<Riddle1>();
            SqlCommand command;
            if (idCategory == null)
            {
                command = new SqlCommand("SELECT Id, Titre, Enonce, Indice, Solution, DatePublication, IdCategory, IdUser From Riddle", Connection.Instance);
            }
            else
            {
                command = new SqlCommand("SELECT Id, Titre, Enonce, Indice, Solution, DatePublication, IdCategory, IdUser From Riddle WHERE IdCategory = @idcategory", Connection.Instance);
                command.Parameters.Add(new SqlParameter("@idcategory", idCategory));
            }
            Connection.Instance.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Riddle1 r = new Riddle1 { Id = reader.GetInt32(0), Titre = reader.GetString(1), Enonce = reader.GetString(2), Indice = reader.GetString(3), Solution = reader.GetString(4), DatePublication = reader.GetDateTime(5), IdCategory = reader.GetInt32(6), IdUser = reader.GetInt32(7) };
                liste.Add(r);
            }

            reader.Close();
            command.Dispose();
            for (int i = 0; i < liste.Count; i++)
            {
                command = new SqlCommand("SELECT Id, Url from RiddleImage WHERE idRiddle = @idRiddle", Connection.Instance);
                command.Parameters.Add(new SqlParameter("@idRiddle", liste[i].Id));
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    liste[i].Image.Add(new RiddleImage { Id = reader.GetInt32(0), Url = reader.GetString(1) });
                }
                reader.Close();
                command.Dispose();
            }
            Connection.Instance.Close();
            return liste;
        }

        public List<Riddle1> GetRiddleDb(int Id)
        {

            List<Riddle1> liste = new List<Riddle1>();
            SqlCommand command = new SqlCommand("SELECT * FROM Riddle WHERE Id=@Id", Connection.Instance);
            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = Id });
            Connection.Instance.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Riddle1 r = new Riddle1 { Id = reader.GetInt32(0), Titre = reader.GetString(1), Enonce = reader.GetString(2), Indice = reader.GetString(3), Solution = reader.GetString(4) };
                liste.Add(r);
            }
            reader.Close();
            command.Dispose();
            for (int i = 0; i < liste.Count; i++)
            {
                command = new SqlCommand("SELECT Id, Url from RiddleImage WHERE idRiddle = @Id", Connection.Instance);
                command.Parameters.Add(new SqlParameter("@Id", liste[i].Id));
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    liste[i].Image.Add(new RiddleImage { Id = reader.GetInt32(0), Url = reader.GetString(1) });
                }
                reader.Close();
                command.Dispose();

            }

            Connection.Instance.Close();
            return liste;
        }

        public void DeleteRiddleDb(Riddle1 r)
        {
            SqlCommand command = new SqlCommand("DELETE FROM Riddle WHERE Id = @id", Connection.Instance);
            command.Parameters.Add(new SqlParameter("@id", r.Id));
            Connection.Instance.Open();
            command.ExecuteNonQuery();
            Connection.Instance.Close();
        }
    }
}
