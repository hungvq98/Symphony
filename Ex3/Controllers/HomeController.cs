using Ex3.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ex3.Controllers
{
    public class HomeController : Controller
    {
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Index()
        {
            Models.Entity.SPNEntities spn = new Models.Entity.SPNEntities();
            var result = (from cr in spn.Courses
                          join te in spn.Teachers
                          on cr.Idtea equals te.Idtea
                          where cr.Active == true
                          select new Models.Modelview.Courseuserview
                          {
                              ID = cr.ID,
                              name = cr.NameCourse,
                              content = cr.CourseContent,
                              imgcourse = cr.Img,
                              nametea = te.Fullname,
                              job = te.Professtional,
                              birthday = te.Birthday.ToString(),
                              history = te.Content,
                              imgtea = te.Img,
                              type = cr.Typecourse,
                              time=cr.Time.ToString(),
                              start=cr.StartTime.ToString(),
                              end=cr.EndTime.ToString()

                          }).AsEnumerable().Reverse().ToList();
            var results = (from cr in spn.Courses
                          join te in spn.Teachers
                          on cr.Idtea equals te.Idtea
                          where cr.Active == true && cr.Typecourse=="New"
                          select new Models.Modelview.Courseuserview
                          {
                              ID = cr.ID,
                              name = cr.NameCourse,
                              content = cr.CourseContent,
                              imgcourse = cr.Img,
                              nametea = te.Fullname,
                              job = te.Professtional,
                              birthday = te.Birthday.ToString(),
                              history = te.Content,
                              imgtea = te.Img,
                              type = cr.Typecourse,
                              time = cr.Time.ToString(),
                              start = cr.StartTime.ToString(),
                              end = cr.EndTime.ToString()

                          }).AsEnumerable().Reverse().ToList();
            var resultss = (from cr in spn.Courses
                           join te in spn.Teachers
                           on cr.Idtea equals te.Idtea                          
                           where cr.Active == true && cr.Typecourse == "Popular"                         
                           select new Models.Modelview.Courseuserview
                           {
                               ID = cr.ID,
                               name = cr.NameCourse,
                               content = cr.CourseContent,
                               imgcourse = cr.Img,
                               nametea = te.Fullname,
                               job = te.Professtional,
                               birthday = te.Birthday.ToString(),
                               history = te.Content,
                               imgtea = te.Img,
                               type = cr.Typecourse,
                               time = cr.Time.ToString(),
                               start = cr.StartTime.ToString(),
                               end = cr.EndTime.ToString()

                           }).AsEnumerable().Reverse().ToList();
            var teacher=spn.Teachers.Where(d=>d.Active==true).Select(d=>new Models.Modelview.Teacherview { fullname=d.Fullname,img=d.Img,professtional=d.Professtional,content=d.Content}).AsEnumerable().Reverse().ToList();

            if (Session["login"] != null)
            {
                string user = Session["login"].ToString();
                var q = spn.UserInfoes.Where(d => d.UserName == user).FirstOrDefault();
                int id = q.ID;
                var noti = spn.Notifications.Where(d => d.IDuser == id && d.Active == true).Select(d => new Models.Modelview.Notificationview { news = d.Notifications }).ToList();
                ViewBag.listno = noti;
                var count = spn.Notifications.Where(d => d.IDuser == id && d.Active == true).Select(d => new Models.Modelview.Notificationview { news = d.Notifications }).Count();
               /* if (count > 0)
                {
                    ViewBag.light = "light";
                }*/
            }
            ViewBag.listte = teacher;
            ViewBag.listcourse = result;
            ViewBag.listcoursenew = results;
            ViewBag.listcoursepop = resultss;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            Models.Entity.SPNEntities spn = new Models.Entity.SPNEntities();
            if (Session["login"] != null)
            {
                string user = Session["login"].ToString();
                var q = spn.UserInfoes.Where(d => d.UserName == user).FirstOrDefault();
                int id = q.ID;
                var noti = spn.Notifications.Where(d => d.IDuser == id && d.Active == true).Select(d => new Models.Modelview.Notificationview { news = d.Notifications }).ToList();
                ViewBag.listno = noti;
                var count = spn.Notifications.Where(d => d.IDuser == id && d.Active == true).Select(d => new Models.Modelview.Notificationview { news = d.Notifications }).Count();

            }
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult LoginNRegister()
        {
            return View();
        }
        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Register()
        {
            Models.Entity.SPNEntities spn = new Models.Entity.SPNEntities();
            var q = spn.UserInfoes.Where(d => d.Active == true).FirstOrDefault();
            string name= Request.Form["user"];          
            if (q!= null)
            {
                if (q.UserName==name)
                {
                    TempData["message"] = "Username already used";

                    return RedirectToAction("LoginNRegister");
                }
                else
                {
                    if(Request.Form["pass"] != Request.Form["pass2"])
                    {
                        TempData["message"] = "Password does not match";

                        return RedirectToAction("LoginNRegister");
                    }
                    else
                    {
                        UserInfo uf = new UserInfo();
                        uf.FullName = Request.Form["fullname"];
                        uf.Age = Request.Form["age"];
                        uf.Phone = Request.Form["phone"];
                        uf.Gender = Request.Form["gender"];
                        uf.Email = Request.Form["email"];
                        uf.Address = Request.Form["address"];
                        uf.UserName = Request.Form["user"];
                        uf.UserPassword = Encrypt_Password(Request.Form["pass"]);
                        string date = DateTime.Now.ToString("yyyy-MM-dd");
                        uf.RegisDate = DateTime.Parse(date);
                        uf.Img = "avatardefault.png";
                        uf.Active = true;
                        spn.UserInfoes.Add(uf);
                        Session["login"] = Request.Form["fullname"];                       
                        spn.SaveChanges();
                        return RedirectToAction("Index");
                        
                    }
                }                     
            }
            else
            {
                if (Request.Form["pass"] != Request.Form["pass2"])
                {
                    TempData["message"] = "Password does not match";
                    return RedirectToAction("LoginNRegister");

                }
                else
                {
                    UserInfo uf = new UserInfo();
                    uf.FullName = Request.Form["fullname"];
                    uf.Age = Request.Form["age"];
                    uf.Phone = Request.Form["phone"];
                    uf.Gender = Request.Form["gender"];
                    uf.Email = Request.Form["email"];
                    uf.Address = Request.Form["address"];
                    uf.UserName = Request.Form["user"];
                    uf.UserPassword = Encrypt_Password(Request.Form["pass"]);
                    string date = DateTime.Now.ToString("yyyy-MM-dd");
                    uf.RegisDate = DateTime.Parse(date);
                    uf.Img = "avatardefault.png";
                    uf.Active = true;
                    spn.UserInfoes.Add(uf);
                    spn.SaveChanges();
                    TempData["message"] = "Success";
                    Session["login"]= Request.Form["user"];
                    return RedirectToAction("Index");
                   
                }   
                            
            }          
        }
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Login()
        {
            Models.Entity.SPNEntities spn = new SPNEntities();
            string user = Request.Form["username"];
            string pass = Request.Form["password"];
            var q = spn.UserInfoes.Where(d=>d.Active == true && d.UserName==user).FirstOrDefault();
            if (q != null)
            {
                string password = Decrypt_Password(q.UserPassword);
                if (q.UserName != user)
                {
                    TempData["message"] = "Wrong account or password";
                    return RedirectToAction("LoginNRegister");
                }
                else
                {
                    if (pass != password)
                    {
                        TempData["message"] = "Wrong account or password";
                        return RedirectToAction("LoginNRegister");
                    }
                    else
                    {
                        
                       
                        Session["login"] = Request.Form["username"];
                        
                        TempData["message"] = "Success";
                        return RedirectToAction("Index");
                    }
                }
            }
            else
            {
                TempData["message"] = "Wrong account or password";
                return RedirectToAction("LoginNRegister");
            }
           
           
        }
        public ActionResult AboutUs()
        {
            Models.Entity.SPNEntities spn = new Models.Entity.SPNEntities();
            if (Session["login"] != null)
            {
                string user = Session["login"].ToString();
                var q = spn.UserInfoes.Where(d => d.UserName == user).FirstOrDefault();
                int id = q.ID;
                var noti = spn.Notifications.Where(d => d.IDuser == id && d.Active == true).Select(d => new Models.Modelview.Notificationview { news = d.Notifications }).ToList();
                ViewBag.listno = noti;
                var count = spn.Notifications.Where(d => d.IDuser == id && d.Active == true).Select(d => new Models.Modelview.Notificationview { news = d.Notifications }).Count();
                
            }
            return View();
        }
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Exame(int idcourse)
        {
            int id = idcourse;
            Session["idcourse"] = idcourse;
            Models.Entity.SPNEntities spn = new Models.Entity.SPNEntities();
            var r = new Random();
            var result = spn.Exams.Where(d => d.IDcourse == id && d.Active == true).Select(d => new Models.Modelview.Examview { ID = d.ID, question = d.Nameofquestion, answerA = d.AnswerA, answerB = d.AnswerB, answerC = d.AnswerC, answerD = d.AnswerD, answercorrect = d.Answercorrect, correct = "correct" + d.ID.ToString(), course = d.IDcourse.ToString() }).AsEnumerable().OrderBy(d=> r.Next()).Take(10).ToList();
            ViewBag.listexam = result;
            if(Session["login"] == null)
            {
                return RedirectToAction("LoginNRegister");
            }
            else
            {
                string user = Session["login"].ToString();
                var users = spn.UserInfoes.Where(d => d.UserName == user).FirstOrDefault();              
                var request = spn.RequestCourses.Where(d => d.IDUser == users.ID && d.IDCourse == id && d.Active == true).FirstOrDefault();
                if(request == null)
                {
                    return View();
                }
                else
                {
                    if (request.IDUser == users.ID && request.IDCourse == id && request.Active == true)
                    {
                        TempData["message"] = "The course has been registered";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View();
                    }
                }
               


            }
            
        }
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Score()
        {
            Models.Entity.SPNEntities spn = new SPNEntities();
            Models.Entity.SPNEntities spns = new SPNEntities();
            string user = Session["login"].ToString();
            string id = Session["idcourse"].ToString();
            int idcourse = Int32.Parse(id);
            var users = spn.UserInfoes.Where(d => d.UserName == user).FirstOrDefault();
            Historyexam his = new Historyexam();
            his.CourseID = idcourse;
            his.UserID = users.ID;
            his.Score = Request.Form["total"];
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            his.Dateexam = DateTime.Parse(date);
            his.Active = true;
            spn.Historyexams.Add(his);
            spn.SaveChanges();
            int score = Int32.Parse(Request.Form["total"]);
            if (score <= 6)
            {
                RequestCourse rs = new RequestCourse();
                rs.IDUser = users.ID;
                rs.IDCourse = idcourse;
                rs.Payment = false;
                rs.Active = true;
                spns.RequestCourses.Add(rs);
                spns.SaveChanges();
                TempData["message"] = "You have successfully registered for the course";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "you have not successfully registered for the course";
                return RedirectToAction("Index");
            }
            
        }
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult FAQ()
        {
            Models.Entity.SPNEntities spn = new SPNEntities();
            var result = spn.Questionfaqs.Where(d => d.Active == true).Select(d => new Models.Modelview.Questionfaqview { question = d.Namequestion, answer = d.Answer }).ToList();
            ViewBag.listfaq = result;
            return View();
        }
        public ActionResult Send()
        {
            Models.Entity.SPNEntities spn = new SPNEntities();
            if (Session["login"]==null)
            {

                return RedirectToAction("LoginNRegister");
            }
            else
            {
                string user = Session["login"].ToString();
                var q = spn.UserInfoes.Where(d => d.UserName == user && d.Active == true).FirstOrDefault();
                Feedback fd = new Feedback();
                fd.UserID = q.ID;
                fd.Subject = Request.Form["title"];
                fd.Message = Request.Form["message"];
                string date = DateTime.Now.ToString("yyyy-MM-dd");
                fd.ReceivedDate = DateTime.Parse(date);
                fd.Active = true;
                spn.Feedbacks.Add(fd);
                spn.SaveChanges();
                TempData["message"] = "You have successfully submitted your feedback";
                
                return RedirectToAction("Index");
            }
      
           
        }
        private string Encrypt_Password(string password)
        {
            string pswstr = string.Empty;
            byte[] psw_encode = new byte[password.Length];
            psw_encode = System.Text.Encoding.UTF8.GetBytes(password);
            pswstr = Convert.ToBase64String(psw_encode);
            return pswstr;
        }
        private string Decrypt_Password(string encryptpassword)
        {
            string pswstr = string.Empty;
            System.Text.UTF8Encoding encode_psw = new System.Text.UTF8Encoding();
            System.Text.Decoder Decode = encode_psw.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encryptpassword);
            int charCount = Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            pswstr = new String(decoded_char);
            return pswstr;
        }
        
        public ActionResult Forum()
        {
            Models.Entity.SPNEntities spn = new SPNEntities();
            if (Session["login"] == null)
            {

                return RedirectToAction("LoginNRegister");
            }
            else
            {
                var result = (from ps in spn.PostAdmins
                              join uf in spn.UserInfoes
                              on ps.Idad equals uf.ID
                              where ps.Active == true
                              select new Models.Modelview.Postadminview
                              {
                                  ID=ps.ID,
                                  content = ps.Content,
                                  img = ps.Img,
                                  imgad = uf.Img,
                                  namead = uf.FullName
                              }).AsEnumerable().Reverse().ToList();
                ViewBag.listpost = result;               
                return View();
            }          
        }
       
        public ActionResult AccountSetting()
        {
            Models.Entity.SPNEntities spn = new SPNEntities();
            string user = Session["login"].ToString();
            var q = spn.UserInfoes.Where(d => d.UserName == user && d.Active == true).FirstOrDefault();
            int id = q.ID;
            var results = (from his in spn.HistoryCourses
                          join uf in spn.UserInfoes
                          on his.IDuser equals uf.ID
                          join cr in spn.Courses
                          on his.IDcourse equals cr.ID
                          where his.IDuser == id
                          select new Models.Modelview.Historycourseview
                          {
                              course = cr.NameCourse
                          }).ToList();
            ViewBag.listcourse = results;
            var result = spn.UserInfoes.Where(d => d.UserName == user && d.Active == true).Select(d => new Models.Modelview.Userinfoview {ID=d.ID, fullname = d.FullName,email=d.Email,phone=d.Phone,address=d.Address,gender=d.Gender,img=d.Img }).ToList();
            ViewBag.listuser = result;
            return View();
        }
        public ActionResult Updateuser(HttpPostedFileBase myimg)
        {
            int id = Int32.Parse(Request.Form["id"]);
            Models.Entity.SPNEntities spn = new SPNEntities();
            UserInfo uf = spn.UserInfoes.First(d => d.ID == id);
            uf.FullName = Request.Form["name"];
            uf.Phone = Request.Form["phone"];
            uf.Email = Request.Form["gmail"];
            uf.Gender = Request.Form["gender"];
            uf.Address = Request.Form["add"];
            if (myimg == null)
            {
                var q = spn.UserInfoes.Where(d => d.ID == id).FirstOrDefault();
                if (q != null)
                {
                    uf.Img = q.Img;
                }
            }
            else
            {
                uf.Img = myimg.FileName;
                string path = Server.MapPath("~/Upload/imguser/") + uf.Img;
                myimg.SaveAs(path);
            }
          
            spn.SaveChanges();
            return RedirectToAction("AccountSetting");
        }
        public ActionResult Changepass()
        {
            Models.Entity.SPNEntities spn = new SPNEntities();
            string user= Session["login"].ToString();
            string pass = Request.Form["oldpass"];
            var q = spn.UserInfoes.Where(d => d.UserName == user && d.Active == true).FirstOrDefault();
            string password = Decrypt_Password(q.UserPassword);
            if (pass==password)
            {
                string passnew = Request.Form["newpass"];
                string cfpass = Request.Form["cfpass"];
                if (passnew == cfpass)
                {
                    string passnews = Encrypt_Password(passnew);
                    UserInfo uf = spn.UserInfoes.First(d => d.UserName == user);
                    uf.UserPassword = passnews;
                    spn.SaveChanges();
                    return RedirectToAction("LoginNRegister");
                }
                else
                {
                    TempData["message"] = "Password does not match";
                    return RedirectToAction("AccountSetting");
                }
            }
            else
            {
                TempData["message"] = "Wrong old password";
                return RedirectToAction("AccountSetting");
            }
            
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("LoginNRegister");
        }
      
    }
}