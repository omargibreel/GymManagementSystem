using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.ViewModels.MemberViewModels
{
    public class HealthRecordViewModel
    {
        [Required(ErrorMessage = "Height Is Required")]
        [Range(0.1, 300, ErrorMessage = "Hieght between 0.1 and 300 cm")]
        public decimal Height { get; set; }
        
        [Required(ErrorMessage = "Weight Is Required")]
        [Range(1, 350, ErrorMessage = "Weight between 1 and 500 kg")]
        public decimal Weight { get; set; }
        
        [Required(ErrorMessage = "Blood Type is Required")]
        [StringLength(3, ErrorMessage = "Blood Type is max 3 Char Or Less")]
        public string BloodType { get; set; } = null!;
        public string? Note { get; set; }

    }
}
