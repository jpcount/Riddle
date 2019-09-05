using Riddle.Tools;
using System;
using System.Collections.Generic;

namespace Riddle.Models
{
    public class Riddle1
    {
        private int id;
        private string titre;
        private string enonce;
        private string indice;
        private string solution;
        private DateTime datePublication;
        private List<RiddleImage> image;
        private int idCategory;
        private int idUser;

        public int Id { get => id; set => id = value; }
        public string Titre { get => titre; set => titre = value; }
        public string Enonce { get => enonce; set => enonce = value; }
        public string Indice { get => indice; set => indice = value; }
        public string Solution { get => solution; set => solution = value; }
        public DateTime DatePublication { get => datePublication; set => datePublication = value; }
        public List<RiddleImage> Image { get => image; set => image = value; }
        public int IdCategory { get => idCategory; set => idCategory = value; }
        public int IdUser { get => idUser; set => idUser = value; }

        public Riddle1()
        {
            Image = new List<RiddleImage>();
            DatePublication = DateTime.Now;
            Solution = solution;
            Enonce = enonce;
        }

        public void AddRiddle()
        {
            DataBase.Instance.AddRiddleDb(this);
        }

        public static List<Riddle1> RiddleLoad(int? idCategory)
        {
            return DataBase.Instance.RiddleLoadDb(idCategory);
        }

        public static List<Riddle1> GetRiddle(int Id)
        {
            return DataBase.Instance.GetRiddleDb(Id);
        }

        public void DeleteRiddle()
        {
            DataBase.Instance.DeleteRiddleDb(this);
        }

    }
}
