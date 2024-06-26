﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackOverFlow.DomainModels;
using StackOverFlow.ViewModels;
using StackOverFlow.Repository;
using AutoMapper;
using AutoMapper.Configuration;

namespace StackOverFLow.ServiceLayers
{
    public interface IQuestionService
    {
        void Insertquestion(NewQuestionViewModel qvm);
        void UpdateQuestionDetails(EditQuestionViewModel qvm);
        void UpdateQuestionVotesCount(int qid, int value);
        void UpdateQuestionAnswersCount(int qid, int value);
        void UpdateQuestionViewsCount(int qid, int value);
        void DeleteQuestion(int qid);
        List<QuestionViewModel> GetQuestions();
        QuestionViewModel GetQuestionByQuestionID(int QuestionID, int UserID);

    }
    public class QuestionService : IQuestionService
    {
        IQuestionRepository qr;
        public QuestionService()
        {
            qr = new QuestionRepository();
        }

        public void Insertquestion(NewQuestionViewModel qvm)
        {
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<NewQuestionViewModel, Question>(); cfg.IgnoreUnmapped(); });
            IMapper mapper = config.CreateMapper();
            Question q = mapper.Map<NewQuestionViewModel,Question>(qvm);
            qr.InsertQuestion(q);
        }
        public void UpdateQuestionDetails(EditQuestionViewModel qvm)
        {
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<EditQuestionViewModel, Question>(); cfg.IgnoreUnmapped(); });
            IMapper mapper = config.CreateMapper();
            Question q = mapper.Map<EditQuestionViewModel, Question>(qvm);
            qr.UpdateQuestion(q);
        }
        public void UpdateQuestionVotesCount(int qid, int value)
        {
            qr.UpdateQuestionVotesCount(qid, value);
        }
        public void UpdateQuestionAnswersCount(int qid, int value)
        {
            qr.UpdateQuestionAnswerCount(qid, value);
        }
        public void UpdateQuestionViewsCount(int qid, int value)
        {
            qr.UpdateQuestionViewsCount(qid, value);
        }
        public void DeleteQuestion(int qid)
        {
            qr.DeleteQuestion(qid);
        }
        public List<QuestionViewModel> GetQuestions()
        {
            List<Question> q = qr.GetQuestion();
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<Question,QuestionViewModel>();cfg.CreateMap<User,UserViewModel>(); cfg.CreateMap<Category, CategoryViewModel>(); cfg.CreateMap<Answer, AnswerViewModel>(); cfg.CreateMap<Vote, VoteViewModel>(); cfg.IgnoreUnmapped(); });
            IMapper mapper = config.CreateMapper();
            List < QuestionViewModel> qvm = mapper.Map<List<Question>, List<QuestionViewModel>>(q);
            return qvm;
        }
        public QuestionViewModel GetQuestionByQuestionID(int QuestionID, int UserID = 0)
        {
            Question q = qr.GetQuestionByID(QuestionID).FirstOrDefault();
            QuestionViewModel qvm = null;
            if (q != null)
            {
                var config = new MapperConfiguration(cfg => { cfg.CreateMap<Question, QuestionViewModel>(); cfg.CreateMap<User, UserViewModel>(); cfg.CreateMap<Category, CategoryViewModel>(); cfg.CreateMap<Answer, AnswerViewModel>(); cfg.CreateMap<Vote, VoteViewModel>(); cfg.IgnoreUnmapped(); });
                IMapper mapper = config.CreateMapper();
                qvm = mapper.Map<Question, QuestionViewModel>(q);

                foreach(var item in qvm.Answers)
                {
                    item.CurrentUserVoteType = 0;
                    VoteViewModel vote = item.Votes.Where(temp => temp.UserID == UserID).FirstOrDefault();
                    if(vote != null)
                    {
                        item.CurrentUserVoteType = vote.VoteValue;
                    }
                }
            }
            return qvm;
        }

    }
}
