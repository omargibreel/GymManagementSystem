using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Entities
{
    [Owned]  // This attribute indicates that this class is an owned entity type in Entity Framework Core.
             // and if any entity has a property of this type,
             // it will be treated as a part of the owning entity and will be stored in the same table as the owning entity.


    internal class Address
    {
        public int BuildingNumber { get; set; }
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
    }
}
