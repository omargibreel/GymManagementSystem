using AutoMapper;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.DAL.Entities;
using GymManagement.DAL.Repositories.Interfaces;
using GymManagement.DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MemberService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public bool CreateMember(CreateMemberViewModel memberVM)
        {

            if (IsEmailExist(memberVM.Email) || IsPhoneExist(memberVM.Phone))
                return false;

            var member = _mapper.Map<Member>(memberVM);

            _unitOfWork.GetRepository<Member>().Add(member); // added locally to the context, not yet saved to the database
            return _unitOfWork.SaveChanges() > 0; // saves all changes made in the context to the database
        }

        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var members = _unitOfWork.GetRepository<Member>().GetAll();
            if (members is null || !members.Any()) return [];

            var memberViewModels = _mapper.Map<IEnumerable<MemberViewModel>>(members);

            return memberViewModels;
        }

        public MemberViewModel? GetMemberDetails(int memberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(memberId);
            if (member is null) return null;

            var memberVM = _mapper.Map<MemberViewModel>(member);

            var activeMembership = _unitOfWork.GetRepository<Membership>()
                .GetAll(m => m.MemberId == memberId && m.EndDate > DateTime.UtcNow).FirstOrDefault();

            if (activeMembership is not null)
            {
                var plan = _unitOfWork.GetRepository<Plan>().GetById(activeMembership.PlanId);

                memberVM.PlanName = plan?.Name;
                memberVM.MembershipStarDate = activeMembership.CreatedAt.ToShortDateString();
                memberVM.MembershipEndDate = activeMembership.EndDate.ToShortDateString();
            }
            return memberVM;
        }

        public HealthRecordViewModel? GetMemberHealthRecord(int memberId)
        {
            var memberHealthRecord = _unitOfWork.GetRepository<HealthRecord>().GetById(memberId);
            if (memberHealthRecord is null) return null;

            return _mapper.Map<HealthRecordViewModel>(memberHealthRecord);
        }

        public MemberToUpdateViewModel? GetMemberToUpdate(int memberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(memberId);
            if (member is null) return null;

            return _mapper.Map<MemberToUpdateViewModel>(member);
        }

        public bool UpdateMemberDetails(int memberId, MemberToUpdateViewModel memberVM)
        {
            try
            {
                var emailExists = _unitOfWork.GetRepository<Member>()
                     .GetAll(x => x.Email == memberVM.Email && x.Id != memberId).Any();

                var phoneExists = _unitOfWork.GetRepository<Member>()
                     .GetAll(x => x.Phone == memberVM.Phone && x.Id != memberId).Any();

                if (emailExists || phoneExists)
                    return false;

                var memberRepo = _unitOfWork.GetRepository<Member>();
                var member = memberRepo.GetById(memberId);

                if (member is null)
                    return false;

                _mapper.Map(memberVM, member);

                memberRepo.Update(member);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {

                return false;
            }
        }

        public bool DeleteMember(int memberId)
        {
            var memberRepo = _unitOfWork.GetRepository<Member>();

            var member = memberRepo.GetById(memberId);
            if (member is null)
                return false;

            var sessionIds = _unitOfWork.GetRepository<MemberSession>()
                .GetAll(ms => ms.MemberId == memberId)
                .Select(x => x.SessionId);

            var HasFutureSessions = _unitOfWork.GetRepository<Session>()
                .GetAll(s => sessionIds.Contains(s.Id) && s.StartDate > DateTime.Now).Any();

            if (HasFutureSessions)
                return false;

            var membershipRepo = _unitOfWork.GetRepository<Membership>();
            var memberships = membershipRepo.GetAll(m => m.MemberId == memberId).ToList();

            try
            {
                if (memberships.Any())
                {
                    foreach (var membership in memberships)
                    {
                        membershipRepo.Delete(membership);
                    }
                }
                memberRepo.Delete(member);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {

                return false;
            }
        }

        #region Helper Methods
        private bool IsEmailExist(string email)
        {
            return _unitOfWork.GetRepository<Member>().GetAll(m => m.Email == email).Any();
        }
        private bool IsPhoneExist(string phone)
        {
            return _unitOfWork.GetRepository<Member>().GetAll(m => m.Phone == phone).Any();
        }
        #endregion

    }
}