using DAL;
using InscriptionCLG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class AllocationsRepository : Repository<Allocation>
    {
        public override int Add(Allocation allocation)
        {
            return base.Add(allocation);
        }

        public override bool Delete(int Id)
        {
            return base.Delete(Id);
        }

        public override bool Update(Allocation allocation)
        {
            return base.Update(allocation);
        }
    }
}