using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.TrainerViewModels;
using GymManagement.DAL.Entities;
using GymManagement.DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TrainerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public bool CreateTrainer(CreateTrainerViewModel createTrainer)
        {
            try
            {
                {
                    var repo = _unitOfWork.GetRepository<Trainer>();
                    if (IsEmailExist(createTrainer.Email) || IsPhoneExist(createTrainer.Phone))
                        return false;
                    var trainer = new Trainer
                    {
                        Name = createTrainer.Name,
                        Email = createTrainer.Email,
                        Phone = createTrainer.Phone,
                        DateOfBirth = createTrainer.DateOfBirth,
                        Specialties = createTrainer.Specialities,
                        Gender = createTrainer.Gender,
                        Address = new Address
                        {
                            City = createTrainer.City,
                            Street = createTrainer.Street,
                            BuildingNumber = createTrainer.BuildingNumber,
                        }
                    };
                    repo.Add(trainer);
                    return _unitOfWork.SaveChanges() > 0;
                }
            }
            catch
            {
                return false;
            }
        }
        public IEnumerable<TrainerViewModel> GetAllTrainers()
        {
            var trainers = _unitOfWork.GetRepository<Trainer>().GetAll();
            if (trainers is null || !trainers.Any())
                return [];
            return trainers.Select(x => new TrainerViewModel
            {
                Name = x.Name,
                Email = x.Email,
                Phone = x.Phone,
                Specialties = x.Specialties.ToString(),
            });
        }

        public TrainerViewModel? GetTrainer(int id)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(id);
            if (trainer is null)
                return null;

            return new TrainerViewModel
            {
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                Specialties = trainer.Specialties.ToString(),
            };
        }

        public TrainerToUpdateViewModel? GetTrainerToUpdate(int id)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(id);

            if (trainer is null)
                return null;

            return new TrainerToUpdateViewModel
            {
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                DateOfBirth = trainer.DateOfBirth,
                BuildingNumber = trainer.Address.BuildingNumber,
                City = trainer.Address.City,
                Street = trainer.Address.Street,
                Specialities = trainer.Specialties,
                Gender = trainer.Gender,
            };
        }
        public bool UpdateTrainer(UpdateTrainerViewModel updatedTrainer, int id)
        {
            var repo = _unitOfWork.GetRepository<Trainer>();
            var trainerToUpdate = repo.GetById(id);
            if (trainerToUpdate is null || IsEmailExist(updatedTrainer.Email) || IsPhoneExist(updatedTrainer.Phone))
                return false;

            trainerToUpdate.Email = updatedTrainer.Email;
            trainerToUpdate.Phone = updatedTrainer.Phone;
            trainerToUpdate.Address.BuildingNumber = updatedTrainer.BuildingNumber;
            trainerToUpdate.Address.City = updatedTrainer.City;
            trainerToUpdate.Address.Street = updatedTrainer.Street;
            trainerToUpdate.DateOfBirth = updatedTrainer.DateOfBirth;
            trainerToUpdate.UpdatedAt = DateTime.Now;
            trainerToUpdate.Specialties = updatedTrainer.Specialities;

            return _unitOfWork.SaveChanges() > 0;
        }
        public bool RemoveTrainer(int id)
        {
            var repo = _unitOfWork.GetRepository<Trainer>();
            var trainerToDelete = repo.GetById(id);
            if (trainerToDelete is null || HasActiveSesstions(id))
                return false;
            repo.Delete(trainerToDelete);
            return _unitOfWork.SaveChanges() > 0;
        }



        #region HelperMethods
        private bool HasActiveSesstions(int id)
        {
            return _unitOfWork.GetRepository<Session>()
                .GetAll(S => S.TrainerId == id && S.StartDate > DateTime.Now)
                .Any();
        }
        private bool IsEmailExist(string email)
        {
            return _unitOfWork.GetRepository<Trainer>().GetAll(m => m.Email == email).Any();
        }
        private bool IsPhoneExist(string phone)
        {
            return _unitOfWork.GetRepository<Trainer>().GetAll(m => m.Phone == phone).Any();
        }
        #endregion
    }
}
