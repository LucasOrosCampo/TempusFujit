using CommunityToolkit.Maui.Alerts;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using TempusFujit.Converters;
using TempusFujit.Infra;
using TempusFujit.Models;

namespace TempusFujit.ViewModels
{
    public class CategoriesVM : INotifyPropertyChanged
    {
        readonly IDbContextFactory<DatabaseContext> dbFactory = Services.DbFactory;
        public event PropertyChangedEventHandler PropertyChanged;

        #region Categories
        List<Category> categories;
        public List<Category> Categories { get => categories; set { categories = value; this.OnPropertyChanged(PropertyChanged); } }

        public Command<int> DeleteCategory { get; set; }

        void loadCategories()
        {
            using var db = dbFactory.CreateDbContext();
            Categories = db.Categories.ToList();
        }
        void deleteCategory(int id)
        {
            using var db = dbFactory.CreateDbContext();
            var categoryToRemove = db.Categories.FirstOrDefault(x => x.Id == id);
            if (categoryToRemove != null)
            {
                db.Remove(categoryToRemove);
            }
            db.SaveChanges();
            loadCategories();
        }
        #endregion

        #region Add Category

        string newCategory;
        public string NewCategory { get => newCategory; set { newCategory = value; this.OnPropertyChanged(PropertyChanged); } }
        public IValueConverter addEnabledConverter { get; } = new StringNotEmptyThenEnabledConverter();
        public Command CreateCategory { get; set; }
        void createCategory()
        {
            if (string.IsNullOrEmpty(NewCategory))
                return;
            using var db = dbFactory.CreateDbContext();
            var newCategory = new Category { Name = NewCategory };
            db.Categories.Add(newCategory);
            db.SaveChanges();
            NewCategory = "";
            Snackbar.Make("Supercategoria añadida", duration: TimeSpan.FromSeconds(2)).Show();
            loadCategories();
        }
        #endregion


        public CategoriesVM()
        {
            loadCategories();
            CreateCategory = new Command(createCategory);
            DeleteCategory = new Command<int>(deleteCategory);
        }
    }


}
