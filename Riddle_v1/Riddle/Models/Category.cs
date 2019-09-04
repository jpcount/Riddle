using Riddle.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riddle.Models
{
    public class Category
    {
        private int id;
        private string titre;

        public int Id { get => id; set => id = value; }
        public string Titre { get => titre; set => titre = value; }

        public void AddCategory()
        {
            DataBase.Instance.AddCategoryDb(this);
        }

        public void DeleteCategory()
        {
            DataBase.Instance.DeleteCategoryDb(this);
        }


        public static List<Category> LoadCategory()
        {
            return DataBase.Instance.LoadCategoryDb();
        }

        public bool ExistCategory()
        {
            return DataBase.Instance.LookCategory(this);
        }
    }
}
