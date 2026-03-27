using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Entities
{
    internal class Member : GymUser
    {
        // JoinDate == CreatedAt Of BaseEntity  ===> and i will configure it Using Fluent API
        public string? Photo { get; set; }

        #region Member - HealthRecord
        public HealthRecord HealthRecord { get; set; } = null!;
        #endregion

        #region Member - MemberShips
        public ICollection<MemberShip> MemberShips { get; set; } = null!;
        #endregion

        #region Member - MemberSession
        public ICollection<MemberSession> MemberSessions { get; set; }
        #endregion
    }
}
