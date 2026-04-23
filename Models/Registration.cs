using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InscriptionCLG.Models
{
    public class Registration
    {
        public int Id { get; set; }
        public int StudentId { get; set; }

        public int CourseId { get; set; }

        public int Year { get; set; }
    }
}