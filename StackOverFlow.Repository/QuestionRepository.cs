using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using StackOverFlow.DomainModels;


namespace StackOverFlow.Repository
{
    public interface IQuestionRepository 
    {
        void InsertQuestion(Question q);
        void UpdateQuestion(Question q);
        void UpdateQuestionVotesCount(int qid,int value);
        void UpdateQuestionAnswerCount(int qid, int value);
        void UpdateQuestionViewsCount(int qid, int value);
        void DeleteQuestion(int qid);
        List<Question> GetQuestion();

        List<Question> GetQuestionByID(int qid);

    
    }

    public class QuestionRepository : IQuestionRepository
    {
        StackOverFlowDbContext db;

        public QuestionRepository()
        {
            db = new StackOverFlowDbContext();
        }

        public void InsertQuestion(Question q)
        {
            db.Questions.Add(q);
            db.SaveChanges();
        }

        public void DeleteQuestion(int qid)
        {
           Question q = db.Questions.Where(temp => temp.QuestionID == qid).FirstOrDefault();
            if(q != null)
            {
                db.Questions.Remove(q);
                db.SaveChanges();
            }
        }

        public void UpdateQuestion(Question q)
        {
            Question qt = db.Questions.Where(temp => temp.QuestionID == q.QuestionID).FirstOrDefault();
            if(qt != null)
            {
                qt.QuestionName = q.QuestionName;
                qt.QuestionDateAndTime = q.QuestionDateAndTime;
                qt.CategoryID = q.CategoryID;
                db.SaveChanges();
            }

        }
        public void UpdateQuestionVotesCount(int qid, int value)
        {
            Question qt = db.Questions.Where(temp => temp.QuestionID == qid).FirstOrDefault();
            if(qt != null)
            {
                qt.VotesCount += value;
                db.SaveChanges();
            }
        }

        public void UpdateQuestionAnswerCount(int qid, int value)
        {
            Question qt = db.Questions.Where(temp => temp.QuestionID == qid).FirstOrDefault();
            if (qt != null)
            {
                qt.AnswersCount += value;
                db.SaveChanges();
            }
        }
        public void UpdateQuestionViewsCount(int qid,int value)
        {
            Question qt = db.Questions.Where(temp => temp.QuestionID == qid).FirstOrDefault();
            if (qt != null)
            {
                qt.ViewsCount += value ;
                db.SaveChanges();
            }
        }

        public List<Question> GetQuestion()
        {
            List<Question> q = db.Questions.OrderByDescending(temp => temp.QuestionDateAndTime).ToList();
            return q;
        }

       public List<Question> GetQuestionByID(int qid)
        {
            List<Question> q = db.Questions.Where(temp => temp.QuestionID == qid).ToList();
            return q;
        }
    }
}
