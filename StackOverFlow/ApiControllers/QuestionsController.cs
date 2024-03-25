using StackOverFLow.ServiceLayers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StackOverFlow.ApiControllers
{
    public class QuestionsController : ApiController
    {

        IQuestionService qs;
        IAnswersService asr;

        public QuestionsController(IQuestionService qs, IAnswersService asr)
        {
            this.qs = qs;
            this.asr = asr;
        }   

        public void Post(int AnswerID,int UserID,int value)
        {
            this.asr.UpdateAnswerVotesCount(AnswerID, UserID, value);
        }
    }
}
