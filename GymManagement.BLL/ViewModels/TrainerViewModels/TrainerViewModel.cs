using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.ViewModels.TrainerViewModels
{
    internal class TrainerViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string Specialties { get; set; } = null!;

        public string? DateOfBirth { get; set; }

        public string? Gender { get; set; }

        public string? Address { get; set; }
    }
}
