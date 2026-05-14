using DAL;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using static Controllers.AccessControl;

namespace Controllers
{
    [UserAccess(Models.Access.View)]
    public class StudentsController : Controller
    {
        const string IllegalAccessUrl = "/Accounts/Login?message=Tentative d'accès illégal!&success=false";

        private void InitSessionVariables()
        {
            // Session is a dictionary that hold keys values specific to a session
            // Each user of this web application have their own Session
            // A Session has a default time out of 20 minutes, after time out it is cleared

            if (Session["CurrentId"] == null) Session["CurrentId"] = 0;
            if (Session["CurrentStudentTitle"] == null) Session["CurrentStudentTitle"] = "";
            if (Session["Search"] == null) Session["Search"] = false;
            if (Session["SearchString"] == null) Session["SearchString"] = "";
            if (Session["SortAscending"] == null) Session["SortAscending"] = false;
            if (Session["code"] == null) Session["code"] = "";
            if (Session["CurrentName"] == null) Session["currentName"] = "";
            //ValidateSelectedCategory();
        }

        private void ResetCurrentStudentInfo()
        {
            Session["CurrentId"] = 0;
            Session["CurrentStudentTitle"] = "";
            Session["CurrentName"] = "";
        }
        
        /*
        private void ValidateSelectedCategory()
        {
            if (Session["SelectedCategory"] != null)
            {
                var selectedCategory = (string)Session["SelectedCategory"];
                var Medias = DB.Medias.ToList().Where(c => c.Category == selectedCategory);
                if (Medias.Count() == 0)
                    Session["SelectedCategory"] = "";
            }
        }
        */


        public ActionResult GetSessionYearsList(bool forceRefresh = false)
        {
            try
            {
                InitSessionVariables();

                bool search = (bool)Session["Search"];

                if (search)
                {
                    return PartialView();
                }
                return null;
            }
            catch (System.Exception ex)
            {
                return Content("Erreur interne" + ex.Message, "text/html");
            }
        }

        public ActionResult GetStudentDetails(bool forceRefresh = false)
        {
            try
            {
                InitSessionVariables();

                int studentId = (int)Session["CurrentId"];
                Student student = DB.Students.Get(studentId);
                if (DB.Students.HasChanged || DB.Registrations.HasChanged || forceRefresh)
                {
                    return PartialView(student);
                }
                return null;
            }
            catch (System.Exception ex)
            {
                return Content("Erreur interne" + ex.Message, "text/html");
            }
        }
        public ActionResult GetStudentDetails_2(bool forceRefresh = false)
        {
            InitSessionVariables();
            int studentId = (int)Session["CurrentId"];
            Student student = DB.Students.Get(studentId);
            if (DB.Courses.HasChanged || DB.Registrations.HasChanged ||DB.Students.HasChanged || forceRefresh)
            {
                return PartialView(student);
            }
            return null;
        }
        public ActionResult GetStudents(bool forceRefresh = false)
        {
            /*
             * resultPage = (from p in context.Posts
                      orderby p.PostId
                      select p)
                     .Skip(position)
                     .Take(pageSize)
                     .ToList();
            */
            try
            {
                IEnumerable<Student> result = null;

                if (DB.Users.HasChanged ||
                    DB.Students.HasChanged ||
                    forceRefresh)
                {
                    InitSessionVariables();
                    Session["StudentYearsList"] = DAL.DB.Students.StudentsYears();
                    bool search = (bool)Session["Search"];
                    string searchString = (string)Session["SearchString"];

                    result = DB.Students.ToList();

                    if (search)
                    {
                        // String search
                        result = result.Where(s =>  s.FirstName.ToLower().Contains(searchString) || 
                                                    s.LastName.ToLower().Contains(searchString) ||
                                                    s.Code.ToLower().Contains(searchString));

                        // Year search
                        int SelectedYear = (int)Session["SelectedYear"];
                        if (SelectedYear != 0)
                            result = result.Where(c => c.Year == SelectedYear);
                    }
                    return PartialView(result);
                }
                return null;
            }
            catch (System.Exception ex)
            {
                return Content("Erreur interne" + ex.Message, "text/html");
            }
        }
        

        
        public ActionResult List()
        {
            ResetCurrentStudentInfo();
            return View();
        }
        
        public ActionResult ToggleSearch()
        {
            if (Session["Search"] == null) Session["Search"] = false;
            Session["Search"] = !(bool)Session["Search"];
            return RedirectToAction("List");
        }

       
        
        public ActionResult SetSearchString(string value)
        {
            Session["SearchString"] = value.ToLower();
            return RedirectToAction("List");
        }
        
        
        public ActionResult SetSearchYear(int value)
        {
            Session["SelectedYear"] = value;
            return RedirectToAction("List");
        }
        
        /*
        public ActionResult SetSearchMediasOwner(int value)
        {
            Session["SelectedMediasOwner"] = value;
            return RedirectToAction("List");
        }
        */
        /*
        public ActionResult About()
        {
            return View();
        }
        */
        
        

        
        public ActionResult Details(int id)
        {
            Session["CurrentId"] = id;
            
            Student student = DB.Students.Get(id);
            if (student != null)
            {
                //if (Media.Shared || isOwner)
                Session["CurrentName"] = student.Fullname;
                return View(student);
                //return Redirect("/Accounts/Login?message=Accès illégal! &success=false");
            }
            return RedirectToAction("List");
        }
        
        
        [UserAccess(Models.Access.Write)]
        public ActionResult Create()
        {
            return View(new Student());
        }
        
        
        [UserAccess(Models.Access.Write)]
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Create(Student student)
        {
            DB.Students.Add(student);
            DB.Events.Add("Create", student.LastName + ", " + student.FirstName);
            return RedirectToAction("List");
        }


        [UserAccess(Access.Write)]
        public ActionResult Edit()
        {
            int id = Session["CurrentId"] != null ? (int)Session["CurrentId"] : 0;

            if (id != 0)
            {
                Student student = DB.Students.Get(id);
                ViewBag.Registrations = student.NextSessionCoursesToSelectList;
                ViewBag.Courses = DB.Courses.NextSessionToSelectList;
                
                return View(student);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [UserAccess(Access.Write)]
        public ActionResult Edit(Student student, List<int> selectedCoursesId)
        {
            int id = Session["CurrentId"] != null ? (int)Session["CurrentId"] : 0;
            Student storedStudent = DB.Students.Get(id);
            if (storedStudent != null )
{
                storedStudent.FirstName = student.FirstName;
                storedStudent.LastName = student.LastName;
                storedStudent.BirthDate = student.BirthDate;
                storedStudent.Email = student.Email;
                storedStudent.Phone = student.Phone;
                DB.Students.Update(storedStudent, selectedCoursesId);
                return RedirectToAction("Details/" + id);
            }
            return Redirect("/Accounts/Login?message=Accès illégal! &success=false");
        }
        [UserAccess(Models.Access.Write)]
        public ActionResult Delete()
        {
            int id = Session["CurrentId"] != null ? (int)Session["CurrentId"] : 0;
            if (id != 0)
            {
                Student Student = DB.Students.Get(id);
                if (Student != null)
                {
                    DB.Students.Delete(id);
                    DB.Registrations.DeleteRegistrationStudent(id);
                    return RedirectToAction("List");
                }
            }
            return Redirect("/Accounts/Login?message=Accès illégal! &success=false");

        }


    }
}
