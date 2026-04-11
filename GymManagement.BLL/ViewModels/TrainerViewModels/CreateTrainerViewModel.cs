using GymManagement.DAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.ViewModels.TrainerViewModels
{
    internal class CreateTrainerViewModel
    {
        [Required(ErrorMessage = "Name is Required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 50")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name must contain letters or spaces only")]
        public string Name { get; set; } = null!;


        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Format")] //App Validation
        [DataType(DataType.EmailAddress)] //UI Hint
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Email must be between 5 and 100")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Phone is Required")]
        [Phone(ErrorMessage = "Invalid Phone Format")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage ="Phone number must be a valid Egyption Numbers")]

        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = "DateofBirth is Required")]
        [DataType(DataType.Date)]

        public DateOnly DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is Required")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Building Number is Required")]
        [Range(1, int.MaxValue, ErrorMessage = "Building Number greater than 0")]

        public int BuildingNumber { get; set; }

        [Required(ErrorMessage = "City is Required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "City between 2 and 100")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "City must contain letters or spaces only")]

        public string City { get; set; } = null!;

        [Required(ErrorMessage = "Street is Required")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "Street between 2 and 150")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Street must contain letters or spaces only")]

        public string Street { get; set; } = null!;


        [Required(ErrorMessage = "Specialization is Required")]
        [EnumDataType(typeof(Specialties))]
        public Specialties Specialities { get; set; }

    }
}
