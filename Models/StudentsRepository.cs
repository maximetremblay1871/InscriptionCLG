using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class StudentsRepository : Repository<Student>
    {
        public List<int> StudentsYears()
        {
            List<int> years = new List<int>();
            foreach (Student student in ToList().OrderBy(s => s.Year))
            {
                if(years.IndexOf(student.Year) == -1)
                {
                    years.Add(student.Year);
                }
            }

            return years;
        }

        public bool CodeExist(string code)
        {
            return ToList().Where(t => t.Code.ToLower() == code.ToLower()).FirstOrDefault() != null;
        }

        public string GenerateCodeStudent()
        {
            
            Random random = new Random();
            string code;
            do
            {
                code = DateTime.Today.Year.ToString();
                for (int i = 0; i < 6; i++)
                {
                    code += random.Next(0,10).ToString();
                }
            }
            while (CodeExist(code));
            return code;
        }
        public string GenerateEmailStudent(string FirstName, string LastName)
        {
            string email;
            email = FirstName + "." + LastName + "@clg.qc.ca";
            return email;
        }

        public override int Add(Student student)
        {
            student.Code = GenerateCodeStudent();
            student.Email = GenerateEmailStudent(student.FirstName, student.LastName);
            return base.Add(student);
        }
        public override bool Delete(int Id)
        {
            return base.Delete(Id);
        }
        public override bool Update(Student student)
        {
            Student storedStudent = Get(student.Id);
            if (student.Code != storedStudent.Code) // new code
                student.Code = GenerateCodeStudent();
            return base.Update(student);
        }
    }
}