using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Entities
{
    internal class MemberSession : BaseEntity
    {
        // BookingDate == CreatedAt Of BaseEntity  ===> and i will configure it Using Fluent API
        public int MemberId { get; set; }
        public Member Member { get; set; } = null!;
        public int SessionId { get; set; }
        public Session Session { get; set; } = null!;
        public bool IsAttended { get; set; }
    }
}
