//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ex3.Models.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class Course
    {
        public int ID { get; set; }
        public string NameCourse { get; set; }
        public string CourseContent { get; set; }
        public Nullable<double> Price { get; set; }
        public string Grade { get; set; }
        public string Img { get; set; }
        public Nullable<int> Time { get; set; }
        public string Material { get; set; }
        public Nullable<int> Idtea { get; set; }
        public string Typecourse { get; set; }
        public Nullable<System.DateTime> StartTime { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
        public Nullable<bool> Active { get; set; }
    }
}
