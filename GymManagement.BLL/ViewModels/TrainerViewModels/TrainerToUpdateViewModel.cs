using GymManagement.DAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.ViewModels.TrainerViewModels
{
    public class TrainerToUpdateViewModel
    {
        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public DateOnly DateOfBirth { get; set; }

        public Gender Gender { get; set; }

        public int BuildingNumber { get; set; }

        public string City { get; set; } = null!;

        public string Street { get; set; } = null!;

        public Specialties Specialties { get; set; }
    }
}
