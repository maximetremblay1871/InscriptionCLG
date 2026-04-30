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