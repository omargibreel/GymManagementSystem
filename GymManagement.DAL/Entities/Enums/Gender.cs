using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Entities.Enums
{
    public enum Gender
    {
        // i set Male to 1 because if don't set it, the default value will be 0,
        // and if we set with the default value zero , it will be set as Male
        Male = 1,
        Female
    }
}
