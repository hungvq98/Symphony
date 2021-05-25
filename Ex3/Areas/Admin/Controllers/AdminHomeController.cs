using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ex3.Areas.Admin.Controllers
{
    public class AdminHomeController : Controller
    {
        // GET: Admin/AdminHome
       
        public ActionResult Index()
        {
            return View();
        }
        //Course
        public ActionResult Course()
        {
            return View();
        }
        public ActionResult ViewCourse()
        {
            return View();
        }
        public ActionResult EditCourse()
        {
            return View();
        }
        //Exam
        public ActionResult Exam()
        {
            return View();
        }
        public ActionResult ViewExam()
        {
            return View();
        }
        public ActionResult EditExam()
        {
            return View();
        }
        //Student
        public ActionResult Student()
        {
            return View();
        }
        public ActionResult ViewStudent()
        {
            return View();
        }
        public ActionResult EditStudent()
        {
            return View();
        }
        //Teacher
        public ActionResult Teacher()
        {
            return View();
        }
        public ActionResult ViewTeacher()
        {
            return View();
        }
        public ActionResult EditTeacher()
        {
            return View();
        }
        //Feedback
        public ActionResult Feedback()
        {
            return View();  
        }
        //History Course
        public ActionResult HistoryCourse()
        {
            return View();
        }
        public ActionResult HistoryExam()
        {
            return View();
        }
        //Post Admin
        public ActionResult PostAdmin()
        {
            return View();
        }
        public ActionResult ViewPostAdmin()
        {
            return View();
        }
        public ActionResult EditPostAdmin()
        {
            return View();
        }
        //FAQ
        public ActionResult FAQ()
        {
            return View();
        }
        public ActionResult ViewFAQ()
        {
            return View();
        }
        public ActionResult EditFAQ()
        {
            return View();
        }

    }
}