using Ex3.Models.Entity;
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
            Models.Entity.SPNEntities spn = new SPNEntities();
            var result = spn.Types.Select(d => new Models.Modelview.Typeview { ID = d.ID, name = d.NameType }).ToList();
            var results = spn.Teachers.Select(d => new Models.Modelview.Teacherview { fullname = d.Fullname });
            ViewBag.listtea = results;
            ViewBag.listtype = result;
            return View();
        }
        public ActionResult ViewCourse()
        {
            Models.Entity.SPNEntities spn = new Models.Entity.SPNEntities();
            var result = spn.Courses.Select(d => new Models.Modelview.Courseview { ID = d.ID, name = d.NameCourse, price = d.Price ?? 0, grade = d.Grade, time = (int)d.Time, material = d.Material, teacher = d.Teacher, type = d.Typecourse, img = d.Img, content = d.CourseContent }).ToList();
            ViewBag.listcourse = result;
            return View();
        }
        public ActionResult EditCourse(int idcourse)
        {
            int id = idcourse;
            Models.Entity.SPNEntities spn = new SPNEntities();
            var result = spn.Courses.Where(d => d.ID == id).Select(d => new Models.Modelview.Courseview { ID = d.ID, name = d.NameCourse, price = d.Price ?? 0, grade = d.Grade, time = (int)d.Time, material = d.Material, teacher = d.Teacher, type = d.Typecourse, img = d.Img, content = d.CourseContent}).ToList();
            var results = spn.Types.Select(d => new Models.Modelview.Typeview { ID = d.ID, name = d.NameType }).ToList();
            var resultss = spn.Teachers.Select(d => new Models.Modelview.Teacherview { fullname = d.Fullname });
            ViewBag.listtea = resultss;
            ViewBag.listtype = results;
            ViewBag.listcourse = result;
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Addcourse(HttpPostedFileBase myimg)
        {
            Models.Entity.SPNEntities spn = new Models.Entity.SPNEntities();
            Course cr = new Course();
            cr.NameCourse = Request.Form["namecourse"];
            cr.Price = double.Parse(Request.Form["price"]);
            cr.Grade = Request.Form["grade"];
            cr.Time = int.Parse(Request.Form["time"]);
            cr.Material = Request.Form["material"];
            cr.Teacher = Request.Form["teacher"];
            cr.Typecourse = Request.Form["type"];
            cr.CourseContent = Request.Form["content"];
            cr.Img = myimg.FileName;
            string path = Server.MapPath("~/Upload/imgcourse/") + cr.Img;
            myimg.SaveAs(path);
            cr.CourseContent = Request.Form["content"];
            spn.Courses.Add(cr);
            spn.SaveChanges();
            return RedirectToAction("Course");
        }
        public ActionResult Updatecourse(HttpPostedFileBase myimg)
        {
            int id = Int32.Parse(Request.Form["idcourse"]);
            Models.Entity.SPNEntities spn = new SPNEntities();
            Course cr = spn.Courses.First(d => d.ID == id);
            cr.NameCourse = Request.Form["namecourse"];
            cr.Price = double.Parse(Request.Form["price"]);
            cr.Grade = Request.Form["grade"];
            cr.Time = int.Parse(Request.Form["time"]);
            cr.Material = Request.Form["material"];
            cr.Teacher = Request.Form["teacher"];
            cr.Typecourse = Request.Form["type"];
            cr.CourseContent = Request.Form["content"];
            if (myimg == null)
            {
                var q = spn.Courses.Where(d => d.ID == id).FirstOrDefault();
                if (q != null)
                {
                    cr.Img = q.Img;
                }
            }
            else
            {
                cr.Img = myimg.FileName;
                string path = Server.MapPath("~/Upload/imgcourse/") + cr.Img;
                myimg.SaveAs(path);
            }
            spn.SaveChanges();
            return RedirectToAction("ViewCourse");
        }
        public ActionResult Removecourse(int idcourse)
        {
            int id = idcourse;
            Models.Entity.SPNEntities spn = new SPNEntities();
            var result = spn.Courses.Where(d => d.ID == id).FirstOrDefault();
            spn.Courses.Remove(result);
            spn.SaveChanges();
            return RedirectToAction("ViewCourse");
        }
        //Exam
        public ActionResult Exam()
        {

            Models.Entity.SPNEntities spn = new SPNEntities();
            var result = spn.Courses.Select(d => new Models.Modelview.Courseview { name = d.NameCourse }).ToList();
            ViewBag.listcourse = result;
            return View();
        }
        public ActionResult ViewExam()
        {
            Models.Entity.SPNEntities spn = new SPNEntities();
            var result = spn.Exams.Select(d => new Models.Modelview.Examview { ID = d.ID, answerA = d.AnswerA, answerB = d.AnswerB, answerC = d.AnswerC, answerD = d.AnswerD, answercorrect = d.Answercorrect, course = d.Course, question = d.Nameofquestion }).ToList();
            ViewBag.listexam = result;
            return View();
        }
        public ActionResult EditExam(int idexam)
        {
            int id = idexam;
            Models.Entity.SPNEntities spn = new SPNEntities();
            var result = spn.Exams.Where(d => d.ID == id).Select(d => new Models.Modelview.Examview { ID = d.ID, answerA = d.AnswerA, answerB = d.AnswerB, answerC = d.AnswerC, answerD = d.AnswerD, answercorrect = d.Answercorrect, course = d.Course, question = d.Nameofquestion }).ToList();
            var results = spn.Courses.Select(d => new Models.Modelview.Courseview { name = d.NameCourse }).ToList();
            ViewBag.listcourse = results;
            ViewBag.listexam = result;
            return View();
        }
        public ActionResult Addexam()
        {
            Models.Entity.SPNEntities spn = new SPNEntities();
            Exam ex = new Exam();
            ex.Nameofquestion = Request.Form["question"];
            ex.AnswerA = Request.Form["answerA"];
            ex.AnswerB = Request.Form["answerB"];
            ex.AnswerC = Request.Form["answerC"];
            ex.AnswerD = Request.Form["answerD"];
            ex.Answercorrect = Request.Form["correct"];
            ex.Course = Request.Form["course"];
            spn.Exams.Add(ex);
            spn.SaveChanges();
            return RedirectToAction("Exam");
        }
        public ActionResult Updateexam()
        {
            int id = Int32.Parse(Request.Form["idexam"]);
            Models.Entity.SPNEntities spn = new SPNEntities();
            Exam ex = spn.Exams.First(d => d.ID == id);
            ex.Nameofquestion = Request.Form["question"];
            ex.AnswerA = Request.Form["answerA"];
            ex.AnswerB = Request.Form["answerB"];
            ex.AnswerC = Request.Form["answerC"];
            ex.AnswerD = Request.Form["answerD"];
            ex.Answercorrect = Request.Form["correct"];
            ex.Course = Request.Form["course"];
            spn.SaveChanges();
            return RedirectToAction("ViewExam");
        }
        public ActionResult Removeexam(int idexam)
        {
            int id = idexam;
            Models.Entity.SPNEntities spn = new SPNEntities();
            var result = spn.Exams.Where(d => d.ID == id).FirstOrDefault();
            spn.Exams.Remove(result);
            spn.SaveChanges();
            return RedirectToAction("ViewExam");
        }

        //Student
        public ActionResult ViewStudent()
        {
            Models.Entity.SPNEntities spn = new SPNEntities();
            var result = spn.UserInfoes.Select(d => new Models.Modelview.Userinfoview { ID = d.ID, fullname = d.FullName, age = d.Age, phone = d.Phone, email = d.Email, gender = d.Gender, address = d.Address, regisdate = d.RegisDate.ToString() }).ToList();
            ViewBag.liststudent = result;
            return View();
        }
        public ActionResult EditStudent(int iduser)
        {
            int id = iduser;
            Models.Entity.SPNEntities spn = new SPNEntities();
            var result = spn.UserInfoes.Where(d => d.ID == id).Select(d => new Models.Modelview.Userinfoview { ID = d.ID, fullname = d.FullName, age = d.Age, phone = d.Phone, email = d.Email, gender = d.Gender, address = d.Address, regisdate = d.RegisDate.ToString() }).ToList();
            ViewBag.liststudent = result;
            return View();
        }
        public ActionResult Updatestudent()
        {
            int id = Int32.Parse(Request.Form["iduser"]);
            Models.Entity.SPNEntities spn = new SPNEntities();
            UserInfo uf = spn.UserInfoes.First(d => d.ID == id);
            uf.FullName = Request.Form["fullname"];
            uf.Email = Request.Form["email"];
            uf.Phone = Request.Form["phone"];
            uf.Age = Request.Form["age"];
            uf.Gender = Request.Form["gender"];
            uf.Address = Request.Form["address"];
            spn.SaveChanges();
            return RedirectToAction("ViewStudent");
        }
        public ActionResult Removestudent(int iduser)
        {
            int id = iduser;
            Models.Entity.SPNEntities spn = new SPNEntities();
            var result = spn.UserInfoes.Where(d => d.ID == id).FirstOrDefault();
            spn.UserInfoes.Remove(result);
            spn.SaveChanges();
            return RedirectToAction("ViewStudent");
        }

        //Teacher
        public ActionResult Teacher()
        {
            return View();
        }
        public ActionResult ViewTeacher()
        {
            Models.Entity.SPNEntities spn = new SPNEntities();
            var result = spn.Teachers.Select(d => new Models.Modelview.Teacherview { ID = d.Idtea, fullname = d.Fullname, age = d.Age, phone = d.Phone, email = d.Email, birthday = d.Birthday.ToString(), gender = d.Gender, img = d.Img, content = d.Content, professtional = d.Professtional }).ToList();
            ViewBag.listtea = result;
            return View();
        }
        public ActionResult EditTeacher(int idtea)
        {
            int id = idtea;
            Models.Entity.SPNEntities spn = new SPNEntities();
            var result = spn.Teachers.Where(d => d.Idtea == id).Select(d => new Models.Modelview.Teacherview { ID = d.Idtea, fullname = d.Fullname, age = d.Age, phone = d.Phone, email = d.Email, birthday = d.Birthday.ToString(), gender = d.Gender, img = d.Img, content = d.Content, professtional = d.Professtional }).ToList();
            ViewBag.listtea = result;
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Addteacher(HttpPostedFileBase myimg)
        {
            Models.Entity.SPNEntities spn = new SPNEntities();
            Teacher tc = new Teacher();
            tc.Fullname = Request.Form["fullname"];
            tc.Age = Request.Form["age"];
            tc.Phone = Request.Form["phone"];
            tc.Email = Request.Form["email"];
            tc.Birthday = DateTime.Parse(Request.Form["birthday"]);
            tc.Gender = Request.Form["gender"];
            tc.Content = Request.Form["content"];
            tc.Img = myimg.FileName;
            tc.Professtional = Request.Form["profess"];
            string path = Server.MapPath("~/Upload/imgteacher/") + tc.Img;
            myimg.SaveAs(path);
            spn.Teachers.Add(tc);
            spn.SaveChanges();
            return RedirectToAction("Teacher");
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Updateteacher(HttpPostedFileBase myimg)
        {
            int id = Int32.Parse(Request.Form["idtea"]);
            Models.Entity.SPNEntities spn = new SPNEntities();
            Teacher tc = spn.Teachers.First(d => d.Idtea == id);
            tc.Fullname = Request.Form["fullname"];
            tc.Age = Request.Form["age"];
            tc.Phone = Request.Form["phone"];
            tc.Email = Request.Form["email"];
            tc.Birthday = DateTime.Parse(Request.Form["birthday"]);
            tc.Gender = Request.Form["gender"];
            tc.Content = Request.Form["content"];
            tc.Professtional = Request.Form["profess"];
            if (myimg == null)
            {
                var q = spn.Teachers.Where(d => d.Idtea == id).FirstOrDefault();
                if (q != null)
                {
                    tc.Img = q.Img;
                }
            }
            else
            {
                tc.Img = myimg.FileName;
                string path = Server.MapPath("~/Upload/imgteacher/") + tc.Img;
                myimg.SaveAs(path);
            }
            spn.SaveChanges();
            return RedirectToAction("ViewTeacher");
        }
        public ActionResult Removeteacher(int idtea)
        {
            int id = idtea;
            Models.Entity.SPNEntities spn = new SPNEntities();
            var result = spn.Teachers.Where(d => d.Idtea == id).FirstOrDefault();
            spn.Teachers.Remove(result);
            spn.SaveChanges();
            return RedirectToAction("ViewTeacher");
        }
        //Feedback
        public ActionResult Feedback()
        {
            Models.Entity.SPNEntities spn = new SPNEntities();
            var result = (from fd in spn.Feedbacks
                          join uf in spn.UserInfoes
                          on fd.UserID equals uf.ID
                          select new Models.Modelview.Feedbackview
                          {
                              ID = fd.ID,
                              fullname = uf.FullName,
                              email = uf.Email,
                              title = fd.Subject,
                              message = fd.Message,
                              receiveddate = fd.ReceivedDate.ToString()

                          }).ToList();
            ViewBag.listfeed = result;
            return View();  
        }
        public ActionResult Removefeedback(int idfeed)
        {
            Models.Entity.SPNEntities spn = new SPNEntities();
            int id = idfeed;
            var result = spn.Feedbacks.Where(d => d.ID == id).FirstOrDefault();
            spn.Feedbacks.Remove(result);
            spn.SaveChanges();
            return RedirectToAction("Feedback");
        }
        //History Course
        public ActionResult HistoryCourse()
        {
            Models.Entity.SPNEntities spn = new SPNEntities();
            var result = (from his in spn.HistoryCourses
                          join uf in spn.UserInfoes
                          on his.IDUser equals uf.ID
                          join cr in spn.Courses
                          on his.IDCourse equals cr.ID
                          select new Models.Modelview.Historycourseview
                          {
                              ID = his.ID,
                              fullname = uf.FullName,
                              age = uf.Age,
                              email = uf.Email,
                              gender = uf.Gender,
                              phone = uf.Phone,
                              course = cr.NameCourse,
                              grade = cr.Grade,
                              price = cr.Price ?? 0,
                              time = (int)cr.Time,
                              startime = his.StartTime.ToString(),
                              endtime = his.EndTime.ToString(),
                              payment = his.Payment ?? false
                          }).ToList();
            ViewBag.listhis = result;
            return View();
        }
        public ActionResult Removehiscourse(int idhis)
        {
            int id = idhis;
            Models.Entity.SPNEntities spn = new SPNEntities();
            var result = spn.HistoryCourses.Where(d => d.ID == id).FirstOrDefault();
            spn.HistoryCourses.Remove(result);
            spn.SaveChanges();
            return RedirectToAction("Historycourse");
        }
        public ActionResult HistoryExam()
        {
            Models.Entity.SPNEntities spn = new SPNEntities();
            var result = (from his in spn.Historyexams
                          join uf in spn.UserInfoes
                          on his.UserID equals uf.ID
                          join cr in spn.Courses
                          on his.CourseID equals cr.ID
                          select new Models.Modelview.Historyexamview
                          {
                              ID = his.ID,
                              fullname = uf.FullName,
                              age = uf.Age,
                              email = uf.Email,
                              gender = uf.Gender,
                              phone = uf.Phone,
                              course = cr.NameCourse,
                              score = his.Score,
                              dateexam = his.Dateexam.ToString()
                          }).ToList();
            ViewBag.listhis = result;
            return View();
        }
        public ActionResult Removehistoryexam(int idhis)
        {
            int id = idhis;
            Models.Entity.SPNEntities spn = new SPNEntities();
            var result = spn.Historyexams.Where(d => d.ID == id).FirstOrDefault();
            spn.Historyexams.Remove(result);
            spn.SaveChanges();
            return RedirectToAction("HistoryExam");
        }
        //Post Admin
        public ActionResult PostAdmin()
        {
            Models.Entity.SPNEntities spn = new SPNEntities();
            var result = spn.Types.Select(d => new Models.Modelview.Typeview { name = d.NameType }).ToList();
            ViewBag.listtype = result;
            return View();
        }
        public ActionResult ViewPostAdmin()
        {
            Models.Entity.SPNEntities spn = new SPNEntities();
            var result = spn.PostAdmins.Select(d => new Models.Modelview.Postadminview { ID = d.ID, title = d.Title, content = d.Content, type = d.Typepost, img = d.Img }).ToList();
            ViewBag.listpost = result;
            return View();
        }
        public ActionResult EditPostAdmin(int idpost)
        {
            int id = idpost;
            Models.Entity.SPNEntities spn = new SPNEntities();
            var result = spn.PostAdmins.Where(d => d.ID == id).Select(d => new Models.Modelview.Postadminview { ID = d.ID, title = d.Title, content = d.Content, type = d.Typepost, img = d.Img }).ToList();
            var results = spn.Types.Select(d => new Models.Modelview.Typeview { name = d.NameType }).ToList();
            ViewBag.listtype = results;
            ViewBag.listpost = result;
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Addpostadmin(HttpPostedFileBase myimg)
        {
            Models.Entity.SPNEntities spn = new SPNEntities();
            PostAdmin ps = new PostAdmin();
            ps.Title = Request.Form["title"];
            ps.Content = Request.Form["content"];
            ps.Typepost = Request.Form["type"];
            ps.Img = myimg.FileName;
            string path = Server.MapPath("~/Upload/imgpost/") + ps.Img;
            myimg.SaveAs(path);
            spn.PostAdmins.Add(ps);
            spn.SaveChanges();
            return RedirectToAction("Postadmin");
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Updatepostadmin(HttpPostedFileBase myimg)
        {
            int id = Int32.Parse(Request.Form["idpost"]);
            Models.Entity.SPNEntities spn = new SPNEntities();
            PostAdmin ps = spn.PostAdmins.First(d => d.ID == id);
            ps.Title = Request.Form["title"];
            ps.Content = Request.Form["content"];
            ps.Typepost = Request.Form["type"];
            if (myimg == null)
            {
                var q = spn.PostAdmins.Where(d => d.ID == id).FirstOrDefault();
                if (q != null)
                {
                    ps.Img = q.Img;
                }
            }
            else
            {
                ps.Img = myimg.FileName;
                string path = Server.MapPath("~/Upload/imgpost/") + ps.Img;
                myimg.SaveAs(path);
            }
            spn.SaveChanges();
            return RedirectToAction("ViewPostAdmin");
        }
        public ActionResult Removepostadmin(int idpost)

        {
            int id = idpost;
            Models.Entity.SPNEntities spn = new SPNEntities();
            var result = spn.PostAdmins.Where(d => d.ID == id).FirstOrDefault();
            spn.PostAdmins.Remove(result);
            spn.SaveChanges(); 
            return RedirectToAction("ViewPostAdmin");
        }
        //FAQ
        public ActionResult FAQ()
        {
            return View();
        }
        public ActionResult ViewFAQ()
        {
            Models.Entity.SPNEntities spn = new SPNEntities();
            var result = spn.Questionfaqs.Select(d => new Models.Modelview.Questionfaqview { ID = d.ID, question = d.Namequestion, answer = d.Answer }).ToList();
            ViewBag.listfaq = result;
            return View();
        }
        public ActionResult EditFAQ(int idfaq)
        {
            int id = idfaq;
            Models.Entity.SPNEntities spn = new SPNEntities();
            var result = spn.Questionfaqs.Where(d => d.ID == id).Select(d => new Models.Modelview.Questionfaqview { ID = d.ID, question = d.Namequestion, answer = d.Answer }).ToList();
            ViewBag.listfaq = result;
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Addfaq()
        {
            Models.Entity.SPNEntities spn = new SPNEntities();
            Questionfaq qs = new Questionfaq();
            qs.Namequestion = Request.Form["question"];
            qs.Answer = Request.Form["content"];
            spn.Questionfaqs.Add(qs);
            spn.SaveChanges();
            return RedirectToAction("FAQ");
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Updatefaq()
        {
            int id = Int32.Parse(Request.Form["idfaq"]);
            Models.Entity.SPNEntities spn = new SPNEntities();
            Questionfaq qs = spn.Questionfaqs.First(d => d.ID == id);
            qs.Namequestion = Request.Form["question"];
            qs.Answer = Request.Form["content"];
            spn.SaveChanges();
            return RedirectToAction("ViewFAQ");
        }
        public ActionResult Removefaq(int idfaq)
        {
            int id = idfaq;
            Models.Entity.SPNEntities spn = new SPNEntities();
            var result = spn.Questionfaqs.Where(d => d.ID == id).FirstOrDefault();
            spn.Questionfaqs.Remove(result);
            spn.SaveChanges();
            return RedirectToAction("ViewFAQ");
        }
        public ActionResult Addtype()
        {
            Models.Entity.SPNEntities spn = new SPNEntities();
            Models.Entity.Type tp = new Models.Entity.Type();
            tp.NameType = Request.Form["type"];
            spn.Types.Add(tp);
            spn.SaveChanges();
            return RedirectToAction("Course");
        }
        public ActionResult Removetype(int idtype)
        {
            int id = idtype;
            Models.Entity.SPNEntities spn = new SPNEntities();
            var result = spn.Types.Where(d => d.ID == id).FirstOrDefault();
            spn.Types.Remove(result);
            spn.SaveChanges();
            return RedirectToAction("Course");
        }
    }
}