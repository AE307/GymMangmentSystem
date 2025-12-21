using GymMangmentBLL.Service.Interfaces;
using GymMangmentBLL.ViewModels.MemberViewModel;
using GymMangmerDAL.Entities;
using GymMangmerDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentBLL.Service.Classes
{
    internal class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MemberService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public bool createMember(CreateMemberViewModel createMember)
        {
            try
            {
                //check if email or phone already exists if one exists return false
                if (IsEmailExists(createMember.Email) || IsPhoneExists(createMember.Phone)) return false;
                var member = new Member()
                {
                    Name = createMember.Name,
                    Email = createMember.Email,
                    Phone = createMember.Phone,
                    Gender = createMember.Gender,
                    DateOfBirth = createMember.DateOfBirth,
                    Address = new Address()
                    {
                        BuildingNumber = createMember.BuildingNumber,
                        Street = createMember.Street,
                        City = createMember.City
                    },
                    HealthRecord = new HealthRecord()
                    {
                        Height = createMember.HealthRecord.Height,
                        Weight = createMember.HealthRecord.Weight,
                        BloodType = createMember.HealthRecord.BloodType,
                        Note = createMember.HealthRecord.Note
                    }

                };
                _unitOfWork.GetRepository<Member>().Add(member);
                return _unitOfWork.SaveChanges()>0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<MemberViewModels> GetAllMember()
        {
            var Members = _unitOfWork.GetRepository<Member>().GetAll();
            if (Members is null || !Members.Any()) return [];
            #region Way01
            //var MemberViewModel = new List<MemberViewModels>();
            //foreach (var Member in Members)
            //{
            //    var memberviewmodel = new MemberViewModels()
            //    {
            //        Id = Member.Id,
            //        Name = Member.Name,
            //        Email = Member.Email,
            //        Phone = Member.Phone,
            //        Photo = Member.photo,
            //        Gender = Member.Gender.ToString()
            //    };
            //    MemberViewModel.Add(memberviewmodel);
            //}
            #endregion
            #region Way02
            var MemberViewModel = Members.Select(member => new MemberViewModels
            {
                Id = member.Id,
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                Photo = member.photo,
                Gender = member.Gender.ToString()
            });
            #endregion
            return MemberViewModel;
        }

        public MemberViewModels? GetMemberDetails(int Memberid)
        {
            var Member = _unitOfWork.GetRepository<Member>().GetById(Memberid);
            if (Member is null) return null;
            var viewmodel = new MemberViewModels()
            {
                Name = Member.Name,
                Email = Member.Email,
                Phone = Member.Phone,
                Gender = Member.Gender.ToString(),
                DateOfBirth = Member.DateOfBirth.ToShortDateString(),
                Address = $"{Member.Address.BuildingNumber}, {Member.Address.Street}, {Member.Address.City}",
                Photo = Member.photo,
            };
            //Active membership
            var ActiveMembership = _unitOfWork.GetRepository<MemberShip>().GetAll(x => x.MemberId == Memberid && x.Status=="Active").FirstOrDefault();
            if (ActiveMembership is not null)
            {
                viewmodel.MemberShipStartDate = ActiveMembership.CreatedAt.ToShortDateString();
                viewmodel.MemberShipEndDate = ActiveMembership.EndDate.ToShortDateString();
                var plan = _unitOfWork.GetRepository<Plan>().GetById(ActiveMembership.PlanId);
                viewmodel.planName = plan?.Name;
            }
            return viewmodel;
        }

        public MemberToUpdateViewModel? GetMemberForUpdate(int Memberid)
        {
            var Member = _unitOfWork.GetRepository<Member>().GetById(Memberid);
            if (Member is null) return null;
            return new MemberToUpdateViewModel()
            {
                Email = Member.Email,
                Name = Member.Name,
                Phone = Member.Phone,
                Photo = Member.photo,
                BuildingNumber = Member.Address.BuildingNumber,
                Street = Member.Address.Street,
                City = Member.Address.City
            };
        }

        public HealthRecordViewModel? GetMemberHealthRecordDetails(int Memberid)
        {
            var MemberHealthRecord = _unitOfWork.GetRepository<HealthRecord>().GetById(Memberid);
            if (MemberHealthRecord is null) return null;
            return new HealthRecordViewModel()
            {
                BloodType = MemberHealthRecord.BloodType,
                Height = MemberHealthRecord.Height,
                Weight = MemberHealthRecord.Weight,
                Note = MemberHealthRecord.Note
            };
        }

        public bool RemoveMember(int Memberid)
        {
            var MemberRepo = _unitOfWork.GetRepository<Member>();
            var Member = MemberRepo.GetById(Memberid);
            if (Member is null) return false;
            var HasactiveMembersession = _unitOfWork.GetRepository<MemberSession>()
                .GetAll(x => x.MemberId == Memberid && x.Session.StartDate > DateTime.Now).Any();
            if (HasactiveMembersession) return false;
            var MembershipRepo = _unitOfWork.GetRepository<MemberShip>();
            var Membership = MembershipRepo.GetAll(x => x.MemberId == Memberid);
            try
            {
                if(Membership.Any())
                {
                    foreach (var membership in Membership)
                    {
                        MembershipRepo.Delete(membership);
                    }
                }
                MemberRepo.Delete(Member);
                return _unitOfWork.SaveChanges()>0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateMember(int Memberid, MemberToUpdateViewModel memberToUpdate)
        {            
            try
            {
                if (IsEmailExists(memberToUpdate.Email) || IsPhoneExists(memberToUpdate.Phone)) return false;

                var MemberRepo = _unitOfWork.GetRepository<Member>();
                var Member = MemberRepo.GetById(Memberid);
                if (Member is null) return false;
                Member.Email = memberToUpdate.Email;
                Member.Phone = memberToUpdate.Phone;
                Member.Address.City = memberToUpdate.City;
                Member.Address.Street = memberToUpdate.Street;
                Member.Address.BuildingNumber = memberToUpdate.BuildingNumber;
                Member.UpdatedAt = DateTime.Now;
                MemberRepo.Update(Member);
                return _unitOfWork.SaveChanges()>0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #region Helper Method
        private bool IsEmailExists(string email)
        {
            return _unitOfWork.GetRepository<Member>().GetAll(x => x.Email == email).Any();
        }
        private bool IsPhoneExists(string phone)
        {
            return _unitOfWork.GetRepository<Member>().GetAll(x => x.Phone == phone).Any();
        }
        #endregion
    }
}
