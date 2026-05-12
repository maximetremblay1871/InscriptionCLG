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
            //ValidateSelectedCategory();
        }

        private void ResetCurrentStudentInfo()
        {
            Session["CurrentId"] = 0;
            Session["CurrentStudentTitle"] = "";
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
                if (DB.Students.HasChanged || forceRefresh)
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

        /*
        public ActionResult Comments(int mediaId, int parentId = 0)
        {
            List<Comment> comments = DB.Comments.ToList().Where(c => c.MediaId == mediaId && c.ParentId == parentId).ToList();
            return PartialView("RenderComments", comments);
        }
        */

        /*
        public ActionResult GetComments(bool forceRefresh = false)
        {
            if (Session["CurrentMediaId"] != null)
            {
                if (DB.Comments.HasChanged ||
                    DB.Commentlikes.HasChanged ||
                    forceRefresh)
                {
                    int mediaId = (int)Session["CurrentMediaId"];

                    List<Comment> comments = DB.Comments.ToList().Where(c => c.MediaId == mediaId && c.ParentId == 0).ToList();
                    return PartialView("RenderComments", comments);
                }
            }
            return null;
        }
        */

        /*
        public ActionResult GetMediasOwnersList(bool forceRefresh = false)
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
        */

        /*
        public ActionResult GetMediaLikes(bool forceRefresh = false)
        {
            try
            {
                InitSessionVariables();

                int mediaId = (int)Session["CurrentMediaId"];
                Media Media = DB.Medias.Get(mediaId);

                if (DB.Likes.HasChanged || forceRefresh)
                {
                    return PartialView(Media);
                }
                return null;
            }
            catch (System.Exception ex)
            {
                return Content("Erreur interne" + ex.Message, "text/html");
            }
        }
        */

        /*
        public ActionResult GetMediaDetails_1(bool forceRefresh = false)
        {
            try
            {
                InitSessionVariables();

                int mediaId = (int)Session["CurrentMediaId"];
                Media Media = DB.Medias.Get(mediaId);
                if (DB.Users.HasChanged || DB.Medias.HasChanged || forceRefresh)
                {
                    return PartialView(Media);
                }
                return null;
            }
            catch (System.Exception ex)
            {
                return Content("Erreur interne" + ex.Message, "text/html");
            }
        }
        */

        /*
        public ActionResult GetMediaDetails_2(bool forceRefresh = false)
        {
            try
            {
                InitSessionVariables();

                int mediaId = (int)Session["CurrentMediaId"];
                Media Media = DB.Medias.Get(mediaId);
                return PartialView(Media);
            }
            catch (System.Exception ex)
            {
                return Content("Erreur interne" + ex.Message, "text/html");
            }
        }
        */



        // This action produce a partial view of Medias
        // It is meant to be called by an AJAX request (from client script)
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

        /*
        public ActionResult SetMediaSortBy(MediaSortBy mediaSortBy)
        {      // /Medias/SetMediasSortBy?mediaSortBy= 
            Session["MediaSortBy"] = mediaSortBy;
            return RedirectToAction("List");
        }
        */
        /*
        public ActionResult ToggleMediaSort()
        {
            int mediaSortBy = (int)Session["MediaSortBy"] + 1;
            if (mediaSortBy >= Enum.GetNames(typeof(MediaSortBy)).Length) mediaSortBy = 0;
            Session["MediaSortBy"] = mediaSortBy;
            return RedirectToAction("List");
        }
        */
        /*
        public ActionResult ToggleSort()
        {
            Session["SortAscending"] = !(bool)Session["SortAscending"];
            return RedirectToAction("List");
        }
        */
        /*
        public ActionResult SortByDate()
        {
            Session["MediaSortBy"] = false;
            return RedirectToAction("List");
        }
        */
        
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


        [UserAccess(Access.Admin)]
        public ActionResult Edit()
        {
            int id = (int)Session["CurrentId"];
            Student student = DB.Students.Get(id);
            if (student != null)
            {
                ViewBag.Registrations = student.NextSessionCoursesToSelectList;
                ViewBag.Courses = DB.Courses.NextSessionToSelectList;
                //ViewBag.Courses = DB.Courses.NextSessionToSelectList;

                return View(DB.Students.Get(id));
            }
            return RedirectToAction("Index");
        }

        [HttpPost ]
        [UserAccess(Access.Admin)]
        public ActionResult Edit(Student student, List<int> selectedCoursesId)
        {
            if (student.IsValid() )
{
                student.Id = (int)Session["CurrentId"];
                student.Code = (string)Session["code"];
                DB.Students.Update(student, selectedCoursesId);
                return RedirectToAction("Details", new { id = student.Id });
            }
            return Redirect("/Accounts/Login?message=Accès illégal! &success=false");
        }

        /*
        [UserAccess(Models.Access.Write)]
        public ActionResult Edit()
        {
            // Note that id is not provided has a parameter.
            // It use the Session["CurrentMediaId"] set within
            // Details(int id) action
            // This way we prevent from malicious requests that could
            // modify or delete programatically the all the Medias

            int id = Session["CurrentMediaId"] != null ? (int)Session["CurrentMediaId"] : 0;
            if (id != 0)
            {
                Media Media = DB.Medias.Get(id);
                if (Media != null)
                {
                    if (Media.OwnerId == Models.User.ConnectedUser.Id || Models.User.ConnectedUser.IsAdmin)
                        return View(Media);
                }
            }
            return Redirect("/Accounts/Login?message=Accès illégal! &success=false");
        }
        */
        /*
        [UserAccess(Models.Access.Write)]
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(Media Media, string sharedCB = "off")
        {
            // Has explained earlier, id of Media is stored server side an not provided in form data
            // passed in the method in order to prever from malicious requests

            int id = Session["CurrentMediaId"] != null ? (int)Session["CurrentMediaId"] : 0;

            // Make sure that the Media of id really exist
            Media storedMedia = DB.Medias.Get(id);
            if (storedMedia != null)
            {
                Media.Id = id; // patch the Id
                Media.Shared = sharedCB == "on";
                Media.OwnerId = storedMedia.OwnerId;
                Media.PublishDate = storedMedia.PublishDate; // keep orignal PublishDate
                DB.Medias.Update(Media);
            }
            return RedirectToAction("Details/" + id);
        }
        */
        /*
        [UserAccess(Models.Access.Write)]
        public ActionResult Delete()
        {
            int id = Session["CurrentMediaId"] != null ? (int)Session["CurrentMediaId"] : 0;
            if (id != 0)
            {
                Media Media = DB.Medias.Get(id);
                if (Media != null)
                {
                    if (Media.OwnerId == Models.User.ConnectedUser.Id || Models.User.ConnectedUser.IsAdmin)
                    {
                        DB.Medias.Delete(id);
                        DB.Events.Add("Delete", Media.Title);
                        return RedirectToAction("List");
                    }

                }
            }
            return Redirect("/Accounts/Login?message=Accès illégal! &success=false");
        }
        */
        /*

        // This action is meant to be called by an AJAX request
        // Return true if there is a name conflict
        // Look into validation.js for more details
        // and also into Views/Medias/MediaForm.cshtml
        public JsonResult CheckConflict(string YoutubeId)
        {
            int id = Session["CurrentMediaId"] != null ? (int)Session["CurrentMediaId"] : 0;
            // Response json value true if name is used in other Medias than the current Media
            return Json(DB.Medias.ToList().Where(c => c.YoutubeId == YoutubeId && c.Id != id).Any(),
                        JsonRequestBehavior.AllowGet /* must have for CORS verification by client browser *//*);
        }
        */
        /*
        public JsonResult CurrentVideoStillAvailable()
        {
            int id = (int)Session["CurrentMediaId"];
            Media currentMedia = DB.Medias.Get(id);
            bool available = false;
            User ConnectedUser = Models.User.ConnectedUser;
            if (currentMedia != null)
            {
                if (ConnectedUser.Access == Access.Admin)
                {
                    available = true;
                }
                else
                {
                    if (currentMedia.Shared)
                    {
                        available = true;
                    }
                    else
                    {
                        if (ConnectedUser.Id == currentMedia.OwnerId)
                            available = true;
                    }
                }
            }
            return Json(available, JsonRequestBehavior.AllowGet /* must have for CORS verification by client browser *//*);
        }
        */

        /*
        public ActionResult ToggleMediaLike(int id)
        {
            User connectedUser = (User)Session["ConnectedUser"];
            DB.Likes.ToggleLike(id, connectedUser.Id);
            Media media = DB.Medias.Get(id);
            media.ResetCountsCalc();
            DB.Events.Add("ToggleMediaLike", media.Title);
            return null;
        }
        */
        /*
        [HttpPost]
        public ActionResult CreateComment(int parentId, string commentText)
        {
            int currentMediaId = (int)Session["CurrentMediaId"];
            if (currentMediaId != 0)
            {
                DB.Comments.Add(new Comment
                {
                    OwnerId = Models.User.ConnectedUser.Id,
                    CreationDate = DateTime.Now,
                    ParentId = parentId,
                    Text = commentText,
                    MediaId = currentMediaId
                });
            }
            return null;
        }
        */
        /*
        [HttpPost]
        public ActionResult UpdateComment(int commentId, string commentText)
        {
            User connectedUser = Models.User.ConnectedUser;
            Comment comment = DB.Comments.Get(commentId);
            if (comment != null && comment.Owner.Id == connectedUser.Id)
            {
                comment.Text = commentText;
                DB.Comments.Update(comment);
            }
            return null;
        }
        */
        /*
        public ActionResult DeleteComment(int id)
        {
            Comment comment = DB.Comments.Get(id);
            if (comment != null)
            {
                User connectedUser = Models.User.ConnectedUser;
                if (connectedUser.IsAdmin || comment.OwnerId == connectedUser.Id)
                {
                    DB.Comments.Delete(id);
                    return null;
                }
                else
                    return Redirect(IllegalAccessUrl);
            }
            return Redirect(IllegalAccessUrl);
        }
        */
        /*
        public ActionResult ToggleCommentLike(int id)
        {
            DB.Commentlikes.ToggleLike(id, Models.User.ConnectedUser.Id);
            return null;
        }
        */
    }
}
