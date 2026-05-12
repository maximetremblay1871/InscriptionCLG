using DAL;
using InscriptionCLG.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;

namespace Models
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Code { get; set; }

        public DateTime BirthDate { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        [JsonIgnore] public string Fullname => FirstName + " " + LastName;
        [JsonIgnore] public string Caption => Code + " " + Fullname;
        [JsonIgnore] public int Year
        {
            get
            {
                if (string.IsNullOrEmpty(Code) || Code.Length < 4)
                {
                    return 0;
                }
                else
                {
                    return int.Parse(Code.Substring(0, 4));
                }
            }
        }
        [JsonIgnore] public List<Registration> Registrations => DAL.DB.Registrations.ToList().Where(r => r.StudentId == Id).ToList();
        [JsonIgnore] public List<Registration> NextSessionRegistrations => DB.Registrations.ToList().Where(r => r.StudentId == Id && r.IsNextSession).ToList();

        [JsonIgnore]
        public List<Course> Courses
        {
            get
            {
                var courses = new List<Course>();
                foreach (var registration in Registrations.OrderBy(r => r.Course.Code))
                {
                    courses.Add(registration.Course);
                }
                return courses;
            }
        }

        [JsonIgnore]
        public List<Course> NextSessionCourses
        {
            get
            {
                var courses = new List<Course>();
                foreach (var registration in NextSessionRegistrations.OrderBy(r => r.Course.Code))
                {
                    courses.Add(registration.Course);
                }
                return courses;
            }
        }

        [JsonIgnore] 
        public SelectList CoursesSelectList => SelectListUtilities<Course>.Convert(Courses, "Caption");
        
        [JsonIgnore]
        public SelectList NextSessionCoursesToSelectList => SelectListUtilities<Course>.Convert(NextSessionCourses, "Caption");

        public bool IsValid()
        {
            return true; // À REVISITER AVEC PLUS DE DÉTAILS
        }

        public void DeleteAllRegistrations()
        {
            foreach (Registration registration in Registrations)
                DB.Registrations.Delete(registration.Id);
        }
        public void DeleteNextSessionRegistrations()
        {
            foreach (Registration registration in NextSessionRegistrations)
                DB.Registrations.Delete(registration.Id);
        }
        public void UpdateRegistrations(List<int> selectedCoursesId)
        {
            DeleteNextSessionRegistrations();
            if (selectedCoursesId != null)
                foreach (int courseId in selectedCoursesId)
                {
                    DB.Registrations.Add(new Registration { StudentId = Id, CourseId = courseId });
                }
        }

    }
}