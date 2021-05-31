using Ex3.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
            var results = spn.Teachers.Where(d=>d.Active==true).Select(d => new Models.Modelview.Teacherview { fullname = d.Fullname }).ToList();
            ViewBag.listtea = results;
            return View();
        }
        public ActionResult ViewCourse()
        {
            Models.Entity.SPNEntities spn = new Models.Entity.SPNEntities();
            var result = (from cr in spn.Courses
                          join te in spn.Teachers
                          on cr.Idtea equals te.Idtea                          
                          where cr.Active == true
                          select new Models.Modelview.Courseview
                          {
                              ID = cr.ID,
                              name = cr.NameCourse,
                              price = cr.Price ?? 0,
                              grade = cr.Grade,
                              content = cr.CourseContent,
                              img = cr.Img,
                              material = cr.Material,
                              time = (int)cr.Time,
                              startime = cr.StartTime.ToString(),
                              endtime = cr.EndTime.ToString(),
                              teacher = te.Fullname,
                              type = cr.Typecourse

                          }).ToList();
            ViewBag.listcourse = result;
            return View();
        }
        public ActionResult EditCourse(int idcourse)
        {
            int id = idcourse;
            Models.Entity.SPNEntities spn = new SPNEntities();
            var result = (from cr in spn.Courses
                          join te in spn.Teachers
                          on cr.Idtea equals te.Idtea
                          where cr.ID == id
                          select new Models.Modelview.Courseview
                          {
                              ID = cr.ID,
                              name = cr.NameCourse,
                              price = cr.Price ?? 0,
                              grade = cr.Grade,
                              content = cr.CourseContent,
                              img = cr.Img,
                              material = cr.Material,
                              time = (int)cr.Time,
                              startime = cr.StartTime.ToString(),
                              endtime = cr.EndTime.ToString(),
                              teacher = te.Fullname,
                              type = cr.Typecourse
                          }).ToList();
            
            var resultss = spn.Teachers.Where(d=>d.Active==true).Select(d => new Models.Modelview.Teacherview { fullname = d.Fullname }).ToList();
            ViewBag.listtea = resultss;
            ViewBag.listcourse = result;
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Addcourse(HttpPostedFileBase myimg)
        {
            Models.Entity.SPNEntities spn = new Models.Entity.SPNEntities();
            Models.Entity.SPNEntities spns = new Models.Entity.SPNEntities();
            Course cr = new Course();
            cr.NameCourse = Request.Form["namecourse"];
            cr.Price = double.Parse(Request.Form["price"]);
            cr.Grade = Request.Form["grade"];
            cr.Time = int.Parse(Request.Form["time"]);
            cr.Material = Request.Form["material"];
            cr.CourseContent = Request.Form["content"];
            cr.StartTime = DateTime.Parse(Request.Form["start"]);
            cr.EndTime = DateTime.Parse(Request.Form["end"]);
            string nametea = Request.Form["teacher"];
            var teacher = spn.Teachers.Where(d => d.Fullname == nametea).FirstOrDefault();
            cr.Idtea = teacher.Idtea;         
            cr.Typecourse = Request.Form["type"];
            cr.Img = myimg.FileName;
            string path = Server.MapPath("~/Upload/imgcourse/") + cr.Img;
            myimg.SaveAs(path);
            cr.CourseContent = Request.Form["content"];
            cr.Active = true;
            spn.Courses.Add(cr);
            spn.SaveChanges();
            var q = spn.UserInfoes.Where(d => d.Active == true).Select(d => new Models.Modelview.Userinfoview { ID = d.ID }).ToList();
            ViewBag.listuser = q;
            foreach(var item in ViewBag.listuser)
            {
                Notification nt = new Notification();
                nt.IDuser = item.ID;
                nt.Notifications= Request.Form["namecourse"];
                nt.Active = true;
                spns.Notifications.Add(nt);
                spns.SaveChanges();
            }
            return RedirectToAction("Course");
        }
        [HttpPost]
        [ValidateInput(false)]
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
            string nametea = Request.Form["teacher"];
            var teacher = spn.Teachers.Where(d => d.Fullname == nametea).FirstOrDefault();
            cr.Idtea = teacher.Idtea;
            cr.Typecourse = Request.Form["type"];
            cr.CourseContent = Request.Form["content"];
            cr.StartTime = DateTime.Parse(Request.Form["start"]);
            cr.EndTime = DateTime.Parse(Request.Form["end"]);
            cr.Active = true;
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
            Course cr = spn.Courses.Where(d => d.ID == id).FirstOrDefault();
            cr.Active = false;
            spn.SaveChanges();
            return RedirectToAction("ViewCourse");
        }
        //Exam
        public ActionResult Exam()
        {

            Models.Entity.SPNEntities spn = new SPNEntities();
            var result = spn.Courses.Where(d=>d.Active==true).Select(d => new Models.Modelview.Courseview { name = d.NameCourse }).ToList();
            ViewBag.listcourse = result;
            return View();
        }
        public ActionResult ViewExam()
        {
            Models.Entity.SPNEntities spn = new SPNEntities();
            var result = (from ex in spn.Exams
                          join cr in spn.Courses
                          on ex.IDcourse equals cr.ID
                          where ex.Active ==true
                          select new Models.Modelview.Examview
                          {
                              ID = ex.ID,
                              question = ex.Nameofquestion,
                              answerA = ex.AnswerA,
                              answerB = ex.AnswerB,
                              answerC = ex.AnswerC,
                              answerD = ex.AnswerD,
                              answercorrect = ex.Answercorrect,
                              course = cr.NameCourse
                          }).ToList();
            ViewBag.listexam = result;
            return View();
        }
        public ActionResult EditExam(int idexam)
        {
            int id = idexam;
            Models.Entity.SPNEntities spn = new SPNEntities();
            var result = (from ex in spn.Exams
                          join cr in spn.Courses
                          on ex.IDcourse equals cr.ID
                          where ex.Active == true
                          select new Models.Modelview.Examview
                          {
                              ID = ex.ID,
                              question = ex.Nameofquestion,
                              answerA = ex.AnswerA,
                              answerB = ex.AnswerB,
                              answerC = ex.AnswerC,
                              answerD = ex.AnswerD,
                              answercorrect = ex.Answercorrect,
                              course = cr.NameCourse
                          }).ToList();
            var results = spn.Courses.Where(d=>d.Active==true).Select(d => new Models.Modelview.Courseview { name = d.NameCourse }).ToList();
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
            string name = Request.Form["course"];
            var course = spn.Courses.Where(d => d.NameCourse == name).FirstOrDefault();
            ex.IDcourse = course.ID;
            ex.Active = true;
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
            string name = Request.Form["course"];
            var course = spn.Courses.Where(d => d.NameCourse == name).FirstOrDefault();
            ex.IDcourse = course.ID;
            ex.Active = true;
            spn.SaveChanges();
            return RedirectToAction("ViewExam");
        }
        public ActionResult Removeexam(int idexam)
        {
            int id = idexam;
            Models.Entity.SPNEntities spn = new SPNEntities();
            Exam ex = spn.Exams.First(d => d.ID == id);
            ex.Active = false;
            spn.SaveChanges();
            return RedirectToAction("ViewExam");
        }

        //Student
        public ActionResult ViewStudent()
        {
            Models.Entity.SPNEntities spn = new SPNEntities();
            var result = spn.UserInfoes.Where(d=>d.Active==true).Select(d => new Models.Modelview.Userinfoview { ID = d.ID, fullname = d.FullName, age = d.Age, phone = d.Phone, email = d.Email, gender = d.Gender, address = d.Address, regisdate = d.RegisDate.ToString() }).ToList();
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
            uf.Active = true;
            spn.SaveChanges();
            return RedirectToAction("ViewStudent");
        }
        public ActionResult Removestudent(int iduser)
        {
            int id = iduser;
            Models.Entity.SPNEntities spn = new SPNEntities();
            UserInfo uf = spn.UserInfoes.First(d => d.ID == id);
            uf.Active = false;
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
            var result = spn.Teachers.Where(d=>d.Active==true).Select(d => new Models.Modelview.Teacherview { ID = d.Idtea, fullname = d.Fullname, age = d.Age, phone = d.Phone, email = d.Email, birthday = d.Birthday.ToString(), gender = d.Gender, img = d.Img, content = d.Content, professtional = d.Professtional }).ToList();
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
            tc.Professtional = Request.Form["profess"];
            tc.Active = true;
            tc.Img = myimg.FileName;
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
            tc.Active = true;
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
            Teacher tc = spn.Teachers.Where(d => d.Idtea == id).FirstOrDefault();
            tc.Active = false;
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
                          where fd.Active==true
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
        public ActionResult ViewFeedback()
        {
            return View();
        }
        public ActionResult Removefeedback(int idfeed)
        {
            Models.Entity.SPNEntities spn = new SPNEntities();
            int id = idfeed;
            Feedback fd = spn.Feedbacks.First(d => d.ID == id);
            fd.Active = false;
            spn.SaveChanges();
            return RedirectToAction("Feedback");
        }
        public ActionResult Sendemail()
        {
            var email = Request.Form["email"];
            var senderEmail = new MailAddress("minhtan.tn11@gmail.com", "SYMPHONY EDUCATION");
            var receiverEmail = new MailAddress(email, "Receiver");
            var password = "thanhnganngo1104";
            var sub = "Sent request success.";
            var body = Request.Form["content-send"];
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail.Address, password)
            };
            using (var mess = new MailMessage(senderEmail, receiverEmail)
            {
                Subject = sub,
                Body = body
            })
            {
                smtp.Send(mess);
            }
            return RedirectToAction("Feedback");
        }
        //History Course
        public ActionResult HistoryCourse()
        {
            Models.Entity.SPNEntities spn = new SPNEntities();
            var result = (from his in spn.HistoryCourses
                          join uf in spn.UserInfoes
                          on his.IDuser equals uf.ID
                          join cr in spn.Courses
                          on his.IDcourse equals cr.ID
                          join re in spn.RequestCourses
                          on his.IDrequest equals re.ID
                          where his.Active == true
                          select new Models.Modelview.Historycourseview
                          {
                              ID = his.ID,
                              fullname = uf.FullName,
                              age = uf.Age,
                              email = uf.Email,
                              gender = uf.Gender,
                              phone = uf.Phone,
                              course = cr.NameCourse,
                              time = (int)cr.Time,
                              grade = cr.Grade,
                              price = cr.Price ?? 0,
                              startime = cr.StartTime.ToString(),
                              endtime = cr.EndTime.ToString(),
                              payment = re.Payment ?? false
                          }).ToList();
            ViewBag.listhis = result;
            return View();
        }
        public ActionResult Removehiscourse(int idhis)
        {
            int id = idhis;
            Models.Entity.SPNEntities spn = new SPNEntities();
            HistoryCourse his = spn.HistoryCourses.First(d => d.ID == id);
            his.Active = false;
            spn.SaveChanges();
            return RedirectToAction("Historycourse");
        }
        ///historyexam
        public ActionResult HistoryExam()
        {
            Models.Entity.SPNEntities spn = new SPNEntities();
            var result = (from his in spn.Historyexams
                          join uf in spn.UserInfoes
                          on his.UserID equals uf.ID
                          join cr in spn.Courses
                          on his.CourseID equals cr.ID
                          where his.Active == true
                          select new Models.Modelview.Historyexamview
                          {
                              ID = his.ID,
                              fullname = uf.FullName,
                              phone = uf.Phone,
                              email = uf.Email,
                              age = uf.Age,
                              gender = uf.Gender,
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
            Historyexam his = spn.Historyexams.First(d => d.ID == id);
            his.Active = false;
            spn.SaveChanges();
            return RedirectToAction("HistoryExam");
        }
        //Post Admin
        public ActionResult PostAdmin()
        {
            Models.Entity.SPNEntities spn = new SPNEntities();
            var result = spn.Types.Where(d => d.Active == true).Select(d => new Models.Modelview.Typeview { name = d.NameType }).ToList();
            ViewBag.listtype = result;
            return View();
        }
        public ActionResult ViewPostAdmin()
        {
            Models.Entity.SPNEntities spn = new SPNEntities();
            var result = (from ps in spn.PostAdmins
                          join tp in spn.Types
                          on ps.Typepost equals tp.ID
                          where ps.Active == true
                          select new Models.Modelview.Postadminview
                          {
                              ID = ps.ID,
                              
                              content = ps.Content,
                              img = ps.Img,
                              type = tp.NameType
                          }).ToList();
            ViewBag.listpost = result;
            return View();
        }
        public ActionResult EditPostAdmin(int idpost)
        {
            int id = idpost;
            Models.Entity.SPNEntities spn = new SPNEntities();
            var result = (from ps in spn.PostAdmins
                          join tp in spn.Types
                          on ps.Typepost equals tp.ID
                          where ps.ID == id
                          select new Models.Modelview.Postadminview
                          {
                              ID = ps.ID,
                              
                              content = ps.Content,
                              img = ps.Img,
                              type = tp.NameType
                          }).ToList();
            var results = spn.Types.Where(d=>d.Active==true).Select(d => new Models.Modelview.Typeview { name = d.NameType }).ToList();
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
            ps.Idad = 1;
            ps.Content = Request.Form["content"];
            ps.Img = myimg.FileName;
            string path = Server.MapPath("~/Upload/imgpost/") + ps.Img;
            myimg.SaveAs(path);
            string name = Request.Form["type"];
            var type = spn.Types.Where(d => d.NameType == name ).FirstOrDefault();
            ps.Typepost = type.ID;
            ps.Active = true;
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
            ps.Content = Request.Form["content"];
            string name = Request.Form["type"];
            var type = spn.Types.Where(d => d.NameType == name).FirstOrDefault();
            ps.Active = true;
            ps.Typepost = type.ID;
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
            PostAdmin ps = spn.PostAdmins.First(d => d.ID == id);
            ps.Active = false;
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
            var result = spn.Questionfaqs.Where(d => d.Active == true).Select(d => new Models.Modelview.Questionfaqview { ID = d.ID, question = d.Namequestion, answer = d.Answer }).ToList();
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
            qs.Active = true;
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
            qs.Active = true;
            spn.SaveChanges();
            return RedirectToAction("ViewFAQ");
        }
        public ActionResult Removefaq(int idfaq)
        {
            int id = idfaq;
            Models.Entity.SPNEntities spn = new SPNEntities();
            Questionfaq qs = spn.Questionfaqs.First(d => d.ID == id);
            qs.Active = false;
            spn.SaveChanges();
            return RedirectToAction("ViewFAQ");
        }
        
        //Register Place
        public ActionResult RegisterCourse()
        {
            Models.Entity.SPNEntities spn = new SPNEntities();
            var result = (from re in spn.RequestCourses
                          join uf in spn.UserInfoes
                          on re.IDUser equals uf.ID
                          join cr in spn.Courses
                          on re.IDCourse equals cr.ID
                          where re.Payment == false && re.Active == true
                          select new Models.Modelview.Requestcourse
                          {
                              ID = re.ID,
                              fullname = uf.FullName,
                              age = uf.Age,
                              phone = uf.Phone,
                              email = uf.Email,
                              gender = uf.Gender,
                              course = cr.NameCourse,
                              grade = cr.Grade,
                              price = cr.Price ?? 0,
                              time = (int)cr.Time,
                              startime = cr.StartTime.ToString(),
                              endtime = cr.EndTime.ToString(),
                              payment = re.Payment ?? false
                          }).ToList();
            ViewBag.listhis = result;
            return View();
        }
        public ActionResult Acceptcourse(int idhis)
        {
            int id = idhis;
            Models.Entity.SPNEntities spn = new SPNEntities();
            RequestCourse rs = spn.RequestCourses.First(d => d.ID == id);
            rs.Payment = true;
            var q = spn.RequestCourses.Where(d => d.ID == id).FirstOrDefault();
            HistoryCourse his = new HistoryCourse();
            his.IDuser = q.IDUser;
            his.IDcourse = q.IDCourse;
            his.IDrequest = id;
            his.Active = true;
            spn.HistoryCourses.Add(his);
            spn.SaveChanges();
            return RedirectToAction("RegisterCourse");
        }
        public ActionResult Denycourse(int idhis)
        {
            int id = idhis;
            Models.Entity.SPNEntities spn = new SPNEntities();
            RequestCourse rs = spn.RequestCourses.First(d => d.ID == id);
            rs.Active = false;
            spn.SaveChanges();
            return RedirectToAction("RegisterCourse");
        }
        //Add Type
        public ActionResult Type()
        {
            Models.Entity.SPNEntities spn = new SPNEntities();
            var result = spn.Types.Where(d => d.Active == true).Select(d => new Models.Modelview.Typeview { ID = d.ID, name = d.NameType }).ToList();
            ViewBag.listtype = result;
            return View();
        }
        public ActionResult Addtype()
        {
            Models.Entity.SPNEntities spn = new SPNEntities();
            Models.Entity.Type tp = new Models.Entity.Type();
            tp.NameType = Request.Form["type"];
            tp.Active = true;
            spn.Types.Add(tp);
            spn.SaveChanges();
            return RedirectToAction("Type");
        }
        public ActionResult Removetype(int idtype)
        {
            int id = idtype;
            Models.Entity.SPNEntities spn = new SPNEntities();
            Models.Entity.Type tp = spn.Types.First(d => d.ID == id);
            tp.Active = false;
            spn.SaveChanges();
            return RedirectToAction("Type");
        }
        public ActionResult RepFeed()
        {
            return View();
        }
    }
}