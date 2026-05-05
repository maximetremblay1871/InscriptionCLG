using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class TeachersRepository : Repository<Teacher>
    {       
        public bool CodeExist(string code)
        {
            return ToList().Where(t => t.Code.ToLower() == code.ToLower()).FirstOrDefault() != null;
        }
 
        public string GenerateCodeTeacher()
        {
            string code;
            Random random = new Random();
            do
            {
               code = "CLG-420-" + random.Next(10000, 99999).ToString();
            }
            while (CodeExist(code));

            return code;
        }
        public string GenerateEmailTeacher(string FirstName, string LastName)
        {
            string email;
            email = FirstName + "." + LastName + "@clg.qc.ca";
            return email;
        }

        public override int Add(Teacher teacher)
        {
            teacher.Code = GenerateCodeTeacher();
            teacher.Email = GenerateEmailTeacher(teacher.FirstName, teacher.LastName);
            return base.Add(teacher);
        }
        public override bool Update(Teacher teacher)
        {
            Teacher storedTeacher = Get(teacher.Id);
            if (teacher.Code != storedTeacher.Code) // new code
                teacher.Code = GenerateCodeTeacher();
            return base.Update(teacher);
        }
        public override bool Delete(int Id)
        {
            return base.Delete(Id);
        }
       
    }
}