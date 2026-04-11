using GymManagement.BLL.ViewModels.TrainerViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Interfaces
{
    internal interface ITrainerService
    {
        IEnumerable<TrainerViewModel> GetAllTrainers();
        bool CreateTrainer(CreateTrainerViewModel createTrainer);
        TrainerViewModel? GetTrainer(int id);
        TrainerToUpdateViewModel? GetTrainerToUpdate(int id);
        bool UpdateTrainer(UpdateTrainerViewModel updatedTrainer, int id);
        bool RemoveTrainer(int id);

    }
}
