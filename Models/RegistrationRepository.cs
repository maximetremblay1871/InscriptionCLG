using DAL;
using InscriptionCLG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class RegistrationRepository : Repository<Registration>
    {
        public override int Add(Registration registration)
        {
            return base.Add(registration);
        }
        
        public override bool Delete(int Id)
        {
            
            return base.Delete(Id); 
        }
        public void DeleteRegistrationStudent(int Id)
        {
            List<int> ToDeleteList = new List<int>();
            foreach (Registration registration in DB.Registrations.ToList())
            {
                if (registration.StudentId == Id)
                {
                    ToDeleteList.Add(registration.Id);
                }
            }
            foreach (int id in ToDeleteList)
            {
                DB.Registrations.Delete(id);
            }
        }
        public void DeleteRegistrationCourse(int Id)
        {
            List<int> ToDeleteList = new List<int>();
            foreach (Registration registration in DB.Registrations.ToList())
            {
                if (registration.CourseId == Id)
                {
                    ToDeleteList.Add(registration.Id);
                }
            }
            foreach (int id in ToDeleteList)
            {
                DB.Registrations.Delete(id);
            }
        }

        public override bool Update(Registration registration)
        {
            return base.Update(registration);
        }
    }
}