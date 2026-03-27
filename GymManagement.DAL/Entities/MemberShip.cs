using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Entities
{
    internal class MemberShip : BaseEntity
    {

        // StartDate == CreatedAt Of BaseEntity 

        public int MemberId { get; set; }
        public Member Member { get; set; } = null!;

        public int PlanId { get; set; }
        public Plan Plan { get; set; } = null!;
        public DateTime EndDate { get; set; }

        // read only property it is computed used to check if the membership is active or not
        public string Status
        {
            get
            {
                if (EndDate > DateTime.UtcNow)
                    return "Expired";
                else return "Active";
            }
        }
    }
}
