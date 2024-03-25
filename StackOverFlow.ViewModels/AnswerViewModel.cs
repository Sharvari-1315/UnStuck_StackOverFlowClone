using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverFlow.ViewModels
{
    public class AnswerViewModel
    {
        public int AnswerID { get; set; }
        public string AnswerText { get; set; }
        public DateTime AnswerDateAndTime { get; set; }
        public string UserID { get; set; }
        public string QuestionID { get; set; }
        public int VotesCount { get; set; }

        public virtual UserViewModel User { get; set; }
        public virtual List<VoteViewModel> Votes { get; set; }
        public int CurrentUserVoteType { get; set; }
    }
}
