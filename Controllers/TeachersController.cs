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
    public class TeachersController : Controller
    {
        private void InitSessionVarables()
        {
            if (Session["CurrentId"] == null) Session["CurrentId"] = 0;
            if (Session["Search"] == null) Session["Search"] = false;
            if (Session["SearchString"] == null) Session["SearchString"] = "";
            if (Session["CurrentName"] == null) Session["currentName"] = "";
        }
        private void ResetCurrentTeacherInfo()
        {
            Session["CurrentId"] = 0;
            Session["CurrentName"] = "";

        }

        public ActionResult GetTeachers(bool forceRefresh = false)
        {
            try
            {
                IEnumerable<Teacher> result = null;
                if (DB.Teachers.HasChanged || DB.Students.HasChanged || forceRefresh)
                {
                    InitSessionVarables();
                    bool search = (bool)Session["Search"];
                    string searchString = (string)Session["SearchString"];
                    result = DB.Teachers.ToList();

                    if (search)
                    {
                        result = result.Where(c => c.LastName.ToLower().Contains(searchString)).OrderBy(c=>c.LastName);
                    }
                    else
                    {
                        result = result.OrderBy(c => c.LastName);
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
            ResetCurrentTeacherInfo();
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
            
            Teacher Teacher = DB.Teachers.Get(id);
            if (Teacher != null)
            {
                Session["CurrentName"] = Teacher.FirstName + " "+ Teacher.LastName;
                return View(Teacher);
            }
            return RedirectToAction("List");

        }
        [UserAccess(Models.Access.Write)]
        public ActionResult Create()
        {
            return View(new Teacher());
        }
        [UserAccess(Models.Access.Write)]
        [HttpPost]
        [ValidateAntiForgeryToken()]

        public ActionResult Create(Teacher Teacher)
        {
            DB.Teachers.Add(Teacher);
            return RedirectToAction("List");

        }
        [UserAccess(Models.Access.Write)]
        public ActionResult Edit()
        {
            int id = Session["CurrentId"] != null ? (int)Session["CurrentId"] : 0;
            if (id != 0)
            {
                Teacher Teacher = DB.Teachers.Get(id);
                if (Teacher != null)
                {
                    return View(Teacher);
                }
            }
            return Redirect("/Accounts/Login?message=Accès illégal! &success=false");
        }
        [UserAccess(Models.Access.Write)]
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(Teacher Teacher)
        {
            int id = Session["CurrentId"] != null ? (int)Session["CurrentId"] : 0;
            Teacher storedTeacher = DB.Teachers.Get(id);
            if (storedTeacher != null)
            {
                Teacher.Id = id;
                DB.Teachers.Update(Teacher);
            }
            return RedirectToAction("Details/" + id);
        }
        [UserAccess(Models.Access.Write)]
        public ActionResult Delete()
        {
            int id = Session["CurrentId"] != null ? (int)Session["CurrentId"] : 0;
            if (id != 0)
            {
                Teacher Teacher = DB.Teachers.Get(id);
                if (Teacher != null)
                {
                    DB.Teachers.Delete(id);
                    return RedirectToAction("List");
                }
            }
            return Redirect("/Accounts/Login?message=Accès illégal! &success=false");

        }
        public ActionResult GetTeacherDetails(bool forceRefresh = false)
        {
            try
            {
                InitSessionVarables();

                int teacherId = (int)Session["CurrentId"];
                Teacher teacher = DB.Teachers.Get(teacherId);
                if (DB.Teachers.HasChanged || forceRefresh)
                {
                    return PartialView(teacher);
                }
                return null;
            }
            catch (System.Exception ex)
            {
                return Content("Erreur interne" + ex.Message, "text/html");
            }
        }
        public JsonResult CheckConflict(DateTime StartDate)
        {
           
            // Response json value true if name is used in other Medias than the current Media
            return Json(StartDate.Date >= DateTime.Now.Date,
                        JsonRequestBehavior.AllowGet /* must have for CORS verification by client browser */);
        }

    }
}