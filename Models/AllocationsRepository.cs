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

        public void DeleteAllocationTeacher(int Id)
        {
            List<int> ToDeleteList = new List<int>();
            foreach (Allocation allocation in DB.Allocations.ToList())
            {
                if (allocation.TeacherId == Id)
                {
                    ToDeleteList.Add(allocation.Id);
                }
            }
            foreach (int id in ToDeleteList)
            {
                DB.Allocations.Delete(id);
            }
        }
        public void DeleteAllocationCourse(int Id)
        {
            List<int> ToDeleteList = new List<int>();
            foreach (Allocation allocation in DB.Allocations.ToList())
            {
                if (allocation.CourseId == Id)
                {
                    ToDeleteList.Add(allocation.Id);
                }
            }
            foreach (int id in ToDeleteList)
            {
                DB.Allocations.Delete(id);
            }
        }

        public override bool Update(Allocation allocation)
        {
            return base.Update(allocation);
        }
    }
}