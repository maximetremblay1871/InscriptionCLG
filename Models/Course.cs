using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InscriptionCLG.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Session { get; set; }

    }
}