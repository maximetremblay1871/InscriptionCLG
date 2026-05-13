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

        public override bool Update(Registration registration)
        {
            return base.Update(registration);
        }
    }
}