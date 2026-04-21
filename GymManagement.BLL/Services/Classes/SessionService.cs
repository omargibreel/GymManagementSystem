using AutoMapper;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.SessionViewModels;
using GymManagement.DAL.Entities;
using GymManagement.DAL.UnitOfWork;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Quic;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public bool CreateSession(CreateSessionViewModel createdSession)
        {
            try
            {
                // Check if Trainer Exists
                if (!IsCategoryExists(createdSession.CategoryId)) return false;
                // Check if Category Exists
                if (!IsTrainerExists(createdSession.TrainerId)) return false;
                // Check if StartDate is before EndDate
                if (!IsDateTimeValid(createdSession.StartDate, createdSession.EndDate)) return false;
                // Check Capacity is Less Than 25 and Greater Than 0
                if (createdSession.Capacity > 25 || createdSession.Capacity < 0) return false;

                var SessionEnitiy = _mapper.Map<CreateSessionViewModel, Session>(createdSession);
                _unitOfWork.GetRepository<Session>().Add(SessionEnitiy);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Create Session Failed : {ex}");
                return false;
            }
        }

        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            var sessions = _unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategory();
            if (!sessions.Any())
                return [];

            //return sessions.Select(s => new SessionViewModel
            //{
            //    Id = s.Id,
            //    Description = s.Description,
            //    StartDate = s.StartDate,
            //    EndDate = s.EndDate,
            //    Capacity = s.Capacity,
            //    TrainerName = s.SessionTrainer.Name, // Related Data not loaded => SessionTrainer is not included in the query, so this will be null => use eager loading in the repository to include SessionTrainer
            //    CategoryName = s.SessionCategory.CategoryName, // Related Data not loaded
            //    //AvailableSlots-- Computed[QuicStreamCapacityChangedArgs - Count Of Booking For Session]
            //    AvailableSlots = s.Capacity - _unitOfWork.SessionRepository.GetCountOfBookingSlots(s.Id),

            //}).ToList();

            var mappedSessions = _mapper.Map<IEnumerable<Session>, IEnumerable<SessionViewModel>>(sessions);
            foreach (var session in mappedSessions)
                session.AvailableSlots = session.Capacity - _unitOfWork.SessionRepository.GetCountOfBookingSlots(session.Id);

            return mappedSessions;
        }

        public SessionViewModel? GetSessionById(int id)
        {
            var session = _unitOfWork.SessionRepository.GetSessionWithTrainerAndCategory(id);
            if (session is null) return null;

            //return new SessionViewModel
            //{
            //    Id = session.Id,
            //    Description = session.Description,
            //    StartDate = session.StartDate,
            //    EndDate = session.EndDate,
            //    Capacity = session.Capacity,
            //    TrainerName = session.SessionTrainer.Name, // Related Data not loaded => SessionTrainer is not included in the query, so this will be null => use eager loading in the repository to include SessionTrainer
            //    CategoryName = session.SessionCategory.CategoryName, // Related Data not loaded
            //    //AvailableSlots-- Computed[QuicStreamCapacityChangedArgs - Count Of Booking For Session]
            //    AvailableSlots = session.Capacity - _unitOfWork.SessionRepository.GetCountOfBookingSlots(session.Id),
            //};

            var mappedSession = _mapper.Map<Session, SessionViewModel>(session);
            mappedSession.AvailableSlots = mappedSession.Capacity - _unitOfWork.SessionRepository.GetCountOfBookingSlots(mappedSession.Id);
            return mappedSession;
        }

        public UpdateSessionViewModel? GetSessionForUpdate(int id)
        {
            var session = _unitOfWork.GetRepository<Session>().GetById(id);
            if (session is null) return null;
            if (!IsSessionAvailableToUpdating(session)) return null;

            return _mapper.Map<Session, UpdateSessionViewModel>(session);
        }

        public bool UpdateSession(int id, UpdateSessionViewModel updatedSession)
        {
            try
            {
                var session = _unitOfWork.SessionRepository.GetById(id);
                if (session is null) return false;
                if (!IsSessionAvailableToUpdating(session)) return false;
                if (!IsTrainerExists(updatedSession.TrainerId)) return false;
                if (!IsDateTimeValid(updatedSession.StartDate, updatedSession.EndDate)) return false;
                _mapper.Map(updatedSession, session);
                session.UpdatedAt = DateTime.Now;
                //_unitOfWork.SessionRepository.Update(session);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Update Session Failed: {ex.Message}");
                return false;
            }
        }

        public bool RemoveSession(int id)
        {
            try
            {
                var session = _unitOfWork.SessionRepository.GetById(id);
                if (!IsSessionAvailableToRemoving(session)) return false;

                _unitOfWork.SessionRepository.Delete(session);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Remove Session Failed: {ex.Message}");
                return false;
            }

        }

        public IEnumerable<TrainerSelectViewModel> GetTrainerForDropDown()
        {
            var trainers = _unitOfWork.GetRepository<Trainer>().GetAll();
            return _mapper.Map<IEnumerable<Trainer>, IEnumerable<TrainerSelectViewModel>>(trainers);
        }

        public IEnumerable<CategorySelectViewModel> GetCategoryForDropDown()
        {
            var categories = _unitOfWork.GetRepository<Category>().GetAll();
            return _mapper.Map<IEnumerable<Category>, IEnumerable<CategorySelectViewModel>>(categories);
        }

        #region HelperMethods
        private bool IsSessionAvailableToUpdating(Session session)
        {
            if (session is null) return false;

            // If Session Completed => Not Available For Updating
            if (session.EndDate < DateTime.Now) return false;
            // If Session Started =? Not Available For Updating
            if (session.StartDate <= DateTime.Now) return false;
            // If Session Has Active Bookings => Not Available For Updating
            var HasActiveBooking = _unitOfWork.SessionRepository.GetCountOfBookingSlots(session.Id) > 0;
            if (HasActiveBooking) return false;

            return true;
        }
        private bool IsSessionAvailableToRemoving(Session session)
        {
            if (session is null) return false;

            // If Session Started => Not Available For removing
            if (session.StartDate <= DateTime.Now && session.EndDate > DateTime.Now) return false;
            // If Session is Upcoming => Not Available For removing
            if (session.StartDate > DateTime.Now) return false;
            // If Session Has Active Bookings => Not Available For removing
            var HasActiveBooking = _unitOfWork.SessionRepository.GetCountOfBookingSlots(session.Id) > 0;
            if (HasActiveBooking) return false;

            return true;
        }
        private bool IsTrainerExists(int trainerId)
        {
            return _unitOfWork.GetRepository<Trainer>().GetById(trainerId) != null;
        }
        private bool IsCategoryExists(int categoryId)
        {
            return _unitOfWork.GetRepository<Category>().GetById(categoryId) != null;
        }
        private bool IsDateTimeValid(DateTime startDate, DateTime endDate)
        {
            return startDate < endDate && startDate > DateTime.Now;
        }

        #endregion
    }
}
