using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;
using DAL;
using System.Web.Mvc;
using static Controllers.AccessControl;
namespace Controllers
{
    [UserAccess(Models.Access.View)]
    public class CoursesController : Controller
    {
        private void InitSessionVarables()
        {
            if (Session["CurrentId"] == null) Session["CurrentId"] = 0;
            if (Session["Search"] == null) Session["Search"] = false;
            if (Session["SearchString"] == null) Session["SearchString"] = "";
            if (Session["CurrentName"] == null) Session["currentName"] = "";
        }
        private void ResetCurrentCourseInfo()
        {
            Session["CurrentId"] = 0;
            Session["CurrentName"] = "";

        }

        public ActionResult GetCourses(bool forceRefresh = false)
        {
            try
            {
                IEnumerable<Course> result = null;
                if (DB.Teachers.HasChanged || DB.Students.HasChanged || DB.Courses.HasChanged || forceRefresh)
                {
                    InitSessionVarables();
                    bool search = (bool)Session["Search"];
                    string searchString = (string)Session["SearchString"];
                    result = DB.Courses.ToList();

                    if (search)
                    {
                        result = result.Where(c => c.Title.ToLower().Contains(searchString)).OrderBy(c => c.Session);
                    }
                    else
                    {
                        result = result.OrderBy(c => c.Session);
                    }
                    return PartialView(result);

                }
                return null;

            }
            catch (Exception ex)
            {
                return Content("Erreur : " + ex.Message);
            }

        }
        public ActionResult List()
        {
            ResetCurrentCourseInfo();
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
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Details(int id)
        {
            Session["CurrentId"] = id;

            Course Course = DB.Courses.Get(id);
            if (Course != null)
            {
                Session["CurrentName"] = Course.Title;
                return View(Course);
            }
            return RedirectToAction("List");

        }
        [UserAccess(Models.Access.Write)]
        public ActionResult Create()
        {
            return View(new Course());
        }
      
        [UserAccess(Models.Access.Write)]
        [HttpPost]
        [ValidateAntiForgeryToken()]

        public ActionResult Create(Course Course)
        {
            DB.Courses.Add(Course);
            return RedirectToAction("List");

        }
        [UserAccess(Models.Access.Write)]
        public ActionResult Edit()
        {
            int id = Session["CurrentId"] != null ? (int)Session["CurrentId"] : 0;
            if (id != 0)
            {
                Course Course = DB.Courses.Get(id);
                if (Course != null)
                {
                    return View(Course);
                }
            }
            return Redirect("/Accounts/Login?message=Accès illégal! &success=false");
        }
        [UserAccess(Models.Access.Write)]
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(Course Course)
        {
            int id = Session["CurrentId"] != null ? (int)Session["CurrentId"] : 0;
            Course storedCourse = DB.Courses.Get(id);
            if (storedCourse != null)
            {
                Course.Id = id;
                DB.Courses.Update(Course);
            }
            return RedirectToAction("Details/" + id);
        }
        [UserAccess(Models.Access.Write)]
        public ActionResult Delete()
        {
            int id = Session["CurrentId"] != null ? (int)Session["CurrentId"] : 0;
            if (id != 0)
            {
                Course Course = DB.Courses.Get(id);
                if (Course != null)
                {
                    DB.Courses.Delete(id);
                    return RedirectToAction("List");
                }
            }
            return Redirect("/Accounts/Login?message=Accès illégal! &success=false");

        }
        public ActionResult GetCourseDetails(bool forceRefresh = false)
        {
            try
            {
                InitSessionVarables();

                int courseId = (int)Session["CurrentId"];
                Course course = DB.Courses.Get(courseId);
                if (DB.Courses.HasChanged || forceRefresh)
                {
                    return PartialView(course);
                }
                return null;
            }
            catch (System.Exception ex)
            {
                return Content("Erreur interne" + ex.Message, "text/html");
            }
        }
        public JsonResult CheckConflict(string Code)
        {
            int id = Session["CurrentId"] != null ? (int)Session["CurrentId"] : 0;
            // Response json value true if name is used in other Medias than the current Media
            return Json(DB.Courses.ToList().Where(c => c.Code == Code && c.Id != id).Any(),
                        JsonRequestBehavior.AllowGet /* must have for CORS verification by client browser */);
        }
        public ActionResult SessionChange()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult SessionChange(string SessionSession, int SessionYear)
        {
            Session["CurrentYear"] = SessionYear;
            Session["CurrentSession"] = SessionSession;
            return RedirectToAction("List", "Students");
        }

    }
}