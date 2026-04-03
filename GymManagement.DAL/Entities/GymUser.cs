using GymManagement.DAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Entities
{
    public abstract class GymUser : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;


        //public string? Phone { get; set; }  // this is nullable tybe
        public string Phone { get; set; } = null!; // this is required type but this symbol "!" Named Null Forgiving Operator,
                                                   // it is used to tell the compiler that we are sure that this property will not be null, even though it is not initialized in the constructor. it is used to avoid the warning of nullable reference types.
       
        public DateOnly DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public Address Address { get; set; } = null!;
    }
}
