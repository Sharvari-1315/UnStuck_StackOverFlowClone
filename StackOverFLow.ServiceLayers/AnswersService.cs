using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackOverFlow.DomainModels;
using StackOverFlow.ViewModels;
using StackOverFlow.Repository;
using AutoMapper;
using AutoMapper.Configuration;
using System.Security.Cryptography;

namespace StackOverFLow.ServiceLayers
{
    public interface IAnswersService
    {
        void InsertAnswer(NewAnswerViewModel avm);
        void UpdateAnswer(EditAnswerViewModel avm);
        void UpdateAnswerVotesCount(int aid, int uid,int value);
        void DeleteAnswer(int aid);
        List<AnswerViewModel> GetAnswerByQuestionID(int qid);
        AnswerViewModel GetAnswerById(int AnswerID);
    }
    public class AnswersService : IAnswersService
    {
        AnswerRepository ar;
        public AnswersService()
        {
           ar = new AnswerRepository();
        }

        public void InsertAnswer(NewAnswerViewModel avm)
        {
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<NewAnswerViewModel, Answer>(); cfg.IgnoreUnmapped(); });
            IMapper mapper = config.CreateMapper();
            Answer a = mapper.Map<NewAnswerViewModel, Answer>(avm);
            ar.InsertAnswer(a);
        }

        public void UpdateAnswer(EditAnswerViewModel avm)
        {
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<EditAnswerViewModel, Answer>(); cfg.IgnoreUnmapped(); });
            IMapper mapper = config.CreateMapper();
            Answer a = mapper.Map<EditAnswerViewModel, Answer>(avm);
            ar.UpdateAnswer(a);
        }

        public void UpdateAnswerVotesCount(int aid, int uid, int value)
        {
            ar.UpdateAnswerVotesCount(aid, uid, value);
        }

        public void DeleteAnswer(int aid)
        {
            ar.DeleteAnswer(aid);
        }
        public List<AnswerViewModel> GetAnswerByQuestionID(int qid)
        {
            List<Answer> a= ar.GetAnswersByquestionID(qid);
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<Answer,AnswerViewModel>(); cfg.IgnoreUnmapped(); });
            IMapper mapper = config.CreateMapper();
            List<AnswerViewModel> avm = mapper.Map<List<Answer>,List<AnswerViewModel>>(a);
            return avm;
        }
        public AnswerViewModel GetAnswerById(int AnswerID)
        {
            Answer a = ar.GetAnswersByAnswerID(AnswerID).FirstOrDefault();
            AnswerViewModel avm = null;
            if (a != null)
            {
                var config = new MapperConfiguration(cfg => { cfg.CreateMap<Answer, AnswerViewModel>(); cfg.IgnoreUnmapped(); });
                IMapper mapper = config.CreateMapper();
                avm = mapper.Map<Answer, AnswerViewModel>(a);
            }
            return avm;
        }
    }
}
