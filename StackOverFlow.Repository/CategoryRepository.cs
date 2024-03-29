using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackOverFlow.DomainModels;

namespace StackOverFlow.Repository
{
    public interface ICategoryRepository
    {
        void InsertCategory(Category c);
        void UpdateCategory(Category c);
        void DeleteCategory(int cid);
        List<Category> GetAllCategories();
        List<Category> GetCategoryById(int id);
    }
    public class CategoryRepository : ICategoryRepository
    {
        StackOverFlowDbContext db;

        public CategoryRepository()
        {
            db = new StackOverFlowDbContext();
        }
       public void InsertCategory(Category c)
        {
            db.Categories.Add(c);
            db.SaveChanges();
        }
        public void UpdateCategory(Category c)
        {
            Category cat = db.Categories.Where(temp => temp.CategoryID == c.CategoryID).FirstOrDefault();
            if(cat != null)
            {
                cat.CategoryName = c.CategoryName;
                db.SaveChanges();
            }
           
        }
        public void DeleteCategory(int cid)
        {
            Category cat = db.Categories.Where(temp => temp.CategoryID == cid).FirstOrDefault();
            if (cat != null)
            {
                db.Categories.Remove(cat);
                db.SaveChanges();
            }
        }
        public List<Category> GetAllCategories()
        {
            List<Category> cat =  db.Categories.ToList();
            return cat;
        }
        public List<Category> GetCategoryById(int id)
        {
            List<Category> cat = db.Categories.Where(temp => temp.CategoryID == id).ToList();
            return cat;
        }
    }
}
