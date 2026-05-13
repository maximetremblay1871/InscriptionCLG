using InscriptionCLG.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class Allocation : DAL.Record
    {
        public Allocation()
        {
            Year = NextSession.Year;
        }
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public int CourseId { get; set; }
        public int Year { get; set; }

        [JsonIgnore] public Course Course => DAL.DB.Courses.Get(CourseId);
        [JsonIgnore] public Teacher Teacher => DAL.DB.Teachers.Get(TeacherId);
        [JsonIgnore] public bool IsNextSession => Year == NextSession.Year && NextSession.ValidSessions.Contains(Course.Session);
    }
}