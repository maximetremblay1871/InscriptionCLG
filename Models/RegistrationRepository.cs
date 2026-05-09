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
        public int GetStudentYear(int studentId)
        {
            int result = 0;
            foreach(Registration registration in ToList().Where(r => r.StudentId == studentId))
            {
                if(result == 0 || result > registration.Year)
                {
                    result = registration.Year;
                }
            }
            return result;
        }

        public override int Add(Registration registration)
        {
            DAL.DB.Students.Get(registration.StudentId).CalculateYear();
            return base.Add(registration);
        }
        
        public override bool Delete(int Id)
        {
            Registration reg = DAL.DB.Registrations.Get(Id);
            Student student = DAL.DB.Students.Get(reg.StudentId);
            
            bool result = base.Delete(Id);

            student.CalculateYear();

            return result; 
        }

        public override bool Update(Registration registration)
        {
            Student student = DAL.DB.Students.Get(registration.StudentId);      // Get initial student

            bool result = base.Update(registration);                            // Update

            student.CalculateYear();                                            // Calculate year

            return result;
        }
    }
}