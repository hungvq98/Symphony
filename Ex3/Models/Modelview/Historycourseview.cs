using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ex3.Models.Modelview
{
    public class Historycourseview
    {
        public int ID { get; set; }
        public string fullname { get; set; }
        public string age { get; set; }
        public string phone { get; set; }
        public string gender { get; set; }
        public string email { get; set; }
        public string course { get; set; }
        public double price { get; set; }
        public string grade { get; set; }
        public int time { get; set; }
        public string startime { get; set; }
        public string endtime { get; set; }
        public bool payment { get; set; }
    }
}