using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InscriptionCLG.Models
{
    public class StudentsRepository : Repository<Student>
    {
        public bool CodeExist(string code)
        {
            return ToList().Where(t => t.Code.ToLower() == code.ToLower()).FirstOrDefault() != null;
        }

        public string GenerateCodeStudent()
        {
            string code = DateTime.Today.Year.ToString();
            Random random = new Random();
            do
            {
                for (int i = 0; i < 6; i++)
                {
                    code += random.Next(0,10).ToString();
                }
            }
            while (CodeExist(code));
            return code;
        }

        public override int Add(Student student)
        {
            student.Code = GenerateCodeStudent();
            return base.Add(student);
        }
    }
}