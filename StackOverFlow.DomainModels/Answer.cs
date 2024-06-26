﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverFlow.DomainModels
{
    public class Answer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AnswerID { get; set; }
        public string AnswerText { get; set; }
        public DateTime answerDateAndTime { get; set; } 
        public int UserID { get; set; }
        public int  QuestionID { get; set; }
        public int VotesCount { get; set; }
        [ForeignKey("UserID")]
        public virtual User User { get; set; }
   
        public virtual List<Vote> Votes { get; set; }

    }
}
