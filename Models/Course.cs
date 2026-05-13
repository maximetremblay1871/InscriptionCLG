using DAL;
using InscriptionCLG.Models;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public int Session { get; set; } = 1;

        [JsonIgnore]
        public string Caption => $"[{Session}] {Code} {Title}";
        [JsonIgnore] public List<Allocation> Allocations => DAL.DB.Allocations.ToList().Where(r => r.CourseId== Id).ToList();
        [JsonIgnore] public List<Student> Students => DAL.DB.Students.ToList().Where(s => !StudentsRegistered.Any(r => r.Id == s.Id)).OrderBy(s => s.Code).ToList();
        [JsonIgnore] public List<Registration> Registrations => DAL.DB.Registrations.ToList().Where(r => r.CourseId == Id).ToList();
        [JsonIgnore] public List<Registration> NextSessionRegistrations => DB.Registrations.ToList().Where(r => r.CourseId == Id && r.IsNextSession).ToList();
        [JsonIgnore]
        public List<Student> StudentsRegistered
        {
            get
            {
                var students = new List<Student>();
                foreach (var registration in Registrations.OrderBy(s => s.Student.Code))
                {
                    students.Add(registration.Student);
                }
                return students;
            }
        }
        [JsonIgnore]
        public Teacher Teacher
        {
            get
            {
                Teacher Teacher = new Teacher();
                if (Allocations.Count == 0)
                {
                    return null;
                }
                else
                {
                    foreach (var teacher in Allocations)
                    {
                        Teacher = teacher.Teacher;
                    }
                    
                    
                }
                return Teacher;
            }
        }
        [JsonIgnore]
        public SelectList StudentSelectList => SelectListUtilities<Student>.Convert(Students, "Caption");

        [JsonIgnore]
        public SelectList RegisteredStudentSelectList => SelectListUtilities<Student>.Convert(StudentsRegistered, "Caption");
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
                foreach (int studentId in selectedCoursesId)
                {
                    DB.Registrations.Add(new Registration { CourseId = Id, StudentId = studentId });
                }
        }
    }
}