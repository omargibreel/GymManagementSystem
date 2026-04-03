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
    internal class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MemberService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public bool CreateMember(CreateMemberViewModel memberVM)
        {

            if (IsEmailExist(memberVM.Email) || IsPhoneExist(memberVM.Phone))
                return false;

            var member = new Member
            {
                Name = memberVM.Name,
                Email = memberVM.Email,
                Phone = memberVM.Phone,
                Gender = memberVM.Gender,
                DateOfBirth = memberVM.DateOfBirth,
                Address = new Address
                {
                    BuildingNumber = memberVM.BuildingNumber,
                    City = memberVM.City,
                    Street = memberVM.Street
                },
                HealthRecord = new HealthRecord
                {
                    BloodType = memberVM.HealthRecordViewModel.BloodType,
                    Height = memberVM.HealthRecordViewModel.Height,
                    Weight = memberVM.HealthRecordViewModel.Weight,
                    Note = memberVM.HealthRecordViewModel.Note
                },
            };

            _unitOfWork.GetRepository<Member>().Add(member); // added locally to the context, not yet saved to the database
            return _unitOfWork.SaveChanges() > 0; // saves all changes made in the context to the database
        }

        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var members = _unitOfWork.GetRepository<Member>().GetAll();
            if (members is null || !members.Any()) return [];

            var memberViewModels = members.Select(m => new MemberViewModel
            {
                Id = m.Id,
                Name = m.Name,
                Email = m.Email,
                Phone = m.Photo,
                Photo = m.Photo,
                Gender = m.Gender.ToString(),
            }).ToList();

            return memberViewModels;
        }

        public MemberViewModel? GetMemberDetails(int memberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(memberId);
            if (member is null) return null;

            var memberVM = new MemberViewModel
            {
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                Photo = member.Photo,
                Gender = member.Gender.ToString(),
                DataOfBirth = member.DateOfBirth.ToShortDateString(),
                Address = $"{member.Address.BuildingNumber} {member.Address.Street} {member.Address.City}",
            };

            var activeMembership = _unitOfWork.GetRepository<Membership>().GetAll(m => m.MemberId == memberId && m.Status == "Active").FirstOrDefault();

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

            return new HealthRecordViewModel
            {
                BloodType = memberHealthRecord.BloodType,
                Height = memberHealthRecord.Height,
                Weight = memberHealthRecord.Weight,
                Note = memberHealthRecord.Note
            };
        }

        public MemberToUpdateViewModel? GetMemberToUpdate(int memberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(memberId);
            if (member is null) return null;

            return new MemberToUpdateViewModel
            {
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                Photo = member.Photo,
                BuildingNumber = member.Address.BuildingNumber,
                City = member.Address.City,
                Street = member.Address.Street
            };
        }

        public bool UpdateMemberDetails(int memberId, MemberToUpdateViewModel memberVM)
        {
            try
            {
                if (IsEmailExist(memberVM.Email) || IsPhoneExist(memberVM.Phone))
                    return false;

                var memberRepo = _unitOfWork.GetRepository<Member>();
                var member = memberRepo.GetById(memberId);

                if (member is null)
                    return false;

                member.Name = memberVM.Name;
                member.Email = memberVM.Email;
                member.Phone = memberVM.Phone;
                member.Address.BuildingNumber = memberVM.BuildingNumber;
                member.Address.City = memberVM.City;
                member.Address.Street = memberVM.Street;
                member.UpdatedAt = DateTime.UtcNow;

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

            var hasActiveMemberSession = _unitOfWork.GetRepository<MemberSession>().GetAll(ms => ms.MemberId == memberId && ms.Session.StartDate > DateTime.UtcNow).Any();
            if (hasActiveMemberSession)
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