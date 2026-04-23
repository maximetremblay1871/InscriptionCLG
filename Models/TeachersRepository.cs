using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InscriptionCLG.Models
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

        public override int Add(Teacher teacher)
        {
            teacher.Code = GenerateCodeTeacher();
            return base.Add(teacher);
        }
    }
}