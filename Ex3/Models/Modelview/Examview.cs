using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ex3.Models.Modelview
{
    public class Examview
    {
        public int ID { get; set; }
        public string question { get; set; }
        public string answerA { get; set; }
        public string answerB { get; set; }
        public string answerC { get; set; }
        public string answerD { get; set; }
        public string answercorrect { get; set; }
        public string course { get; set; }
    }
}