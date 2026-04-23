using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class CoursesRepository : Repository<Course>
    {
        public bool CodeExist(string code)
        {
            return ToList().Where(t => t.Code.ToLower() == code.ToLower()).FirstOrDefault() != null;
        }
        public override int Add(Course data)
        {
            return base.Add(data);
        }
        public override bool Delete(int Id)
        {
            return base.Delete(Id);
        }
        public override bool Update(Course data)
        {
            return base.Update(data);
        }
    }
}