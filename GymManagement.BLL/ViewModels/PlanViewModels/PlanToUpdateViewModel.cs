using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.ViewModels.PlanViewModels
{
    internal class PlanToUpdateViewModel
    {
        public string PlanName { get; set; } = null!;

        [StringLength(200, MinimumLength = 5, ErrorMessage = "Description Must be Between 5 and 200 Char")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Duration Days Is Required")]
        [Range(0, 365, ErrorMessage = "Duration Days Must Be Between 1 and 365")]
        public int DurationDays { get; set; }

        [Required(ErrorMessage = "Price Is Required")]
        [Range(0.1, 10000, ErrorMessage = "Price Must Be Between 0.1 and 10000")]
        public decimal Price { get; set; }
    }
}
