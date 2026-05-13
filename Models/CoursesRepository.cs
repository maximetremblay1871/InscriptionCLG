using DAL;
using InscriptionCLG.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Models
{
    public class CoursesRepository : Repository<Course>
    {
        [JsonIgnore]
        public List<Course> CoursesNextSession => ToList().Where(c => NextSession.ValidSessions.Contains(c.Session)).ToList();

        [JsonIgnore]
        public SelectList CoursesToSelectList => SelectListUtilities<Course>.Convert(ToList().ToList(), "Caption");
        [JsonIgnore]
        public SelectList NextSessionToSelectList => SelectListUtilities<Course>.Convert(CoursesNextSession, "Caption");
        [JsonIgnore]
        public List<Course> CoursesNextSessionTeacher => ToList().Where(c => c.Allocations.Count == 0).
        Where(c => NextSession.ValidSessions.Contains(c.Session)).ToList();
        [JsonIgnore]
        public SelectList NextSessionToSelectListTeacher => SelectListUtilities<Course>.Convert(CoursesNextSessionTeacher, "Caption");

        public bool CodeExist(string code)
        {
            return ToList().Where(t => t.Code.ToLower() == code.ToLower()).FirstOrDefault() != null;
        }

        public override int Add(Course data)
        {
            return base.Add(data);
        }
        public override bool Delete(int Id)
        {
            return base.Delete(Id);
        }
        public override bool Update(Course data)
        {
            return base.Update(data);
        }
        public  bool Update(Course course, List<int> selectedCoursesId)
        {
            course.UpdateRegistrations(selectedCoursesId);
            return Update(course);
        }
    }
}