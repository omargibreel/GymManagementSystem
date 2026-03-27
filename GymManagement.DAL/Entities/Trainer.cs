using GymManagement.DAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Entities
{
    internal class Trainer : GymUser
    {
        // HireDate == CreatedAt Of BaseEntity  ===> and i will configure it Using Fluent API
        public Specialties Specialties { get; set; }

        public ICollection<Session> TrainerSessions { get; set; } = null!;
    }
}
