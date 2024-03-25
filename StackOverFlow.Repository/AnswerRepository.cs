using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using StackOverFlow.DomainModels;

namespace StackOverFlow.Repository
{
    public interface IAnswerRepository 
    {
        void InsertAnswer(Answer a);
        void UpdateAnswer(Answer a);
        void UpdateAnswerVotesCount(int aid, int uid, int value);
        void DeleteAnswer(int aid);
        List<Answer> GetAnswersByquestionID(int qid);
        List<Answer> GetAnswersByAnswerID(int answerID);
      
    }

    public class AnswerRepository : IAnswerRepository
    {
        StackOverFlowDbContext db;
        IQuestionRepository qr;
        IVotesRepository vr;
        public AnswerRepository()
        {
            db = new StackOverFlowDbContext();
            qr = new QuestionRepository();
            vr = new VotesRepository();
        }

        public void InsertAnswer(Answer a)
        {
            db.Answers.Add(a);
            db.SaveChanges();
            qr.UpdateQuestionAnswerCount(a.QuestionID, 1);
        }

        public void UpdateAnswer(Answer a)
        {
            Answer ans = db.Answers.Where(temp => temp.AnswerID == a.AnswerID).FirstOrDefault();
            if (ans != null)
            {
                ans.AnswerText = a.AnswerText;
                db.SaveChanges();
            }
        }

        public void UpdateAnswerVotesCount(int aid, int uid, int value)
        {
            Answer ans = db.Answers.Where(temp => temp.AnswerID == aid).FirstOrDefault();
            if (ans != null)
            {
                ans.VotesCount += value;
                db.SaveChanges();
                qr.UpdateQuestionVotesCount(ans.QuestionID,value);
                vr.UpdateVote(aid,uid,value);
            }
        }

        public void DeleteAnswer(int aid)
        {
            Answer ans = db.Answers.Where(temp => temp.AnswerID == aid).FirstOrDefault();
            if(ans != null)
            {
                db.Answers.Remove(ans);
                db.SaveChanges();
            }

        }

        public List<Answer> GetAnswersByquestionID(int qid)
        {
            List<Answer> ans = db.Answers.Where(temp => temp.QuestionID == qid).OrderByDescending(temp => temp.answerDateAndTime).ToList();
            return ans;
        }

        public List<Answer> GetAnswersByAnswerID(int answerID)
        {
            List<Answer> ans = db.Answers.Where(temp => temp.AnswerID == answerID).OrderByDescending(temp => temp.answerDateAndTime).ToList();
            return ans;
        }
    }
}
