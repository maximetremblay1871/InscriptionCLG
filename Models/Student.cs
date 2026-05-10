using InscriptionCLG.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

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
        [JsonIgnore] public int Year => int.Parse(Code.Substring(0, 4));
        [JsonIgnore] public List<Registration> Registrations => DAL.DB.Registrations.ToList().Where(r => r.StudentId == Id).ToList();
    }
}