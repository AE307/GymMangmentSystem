using AutoMapper;
using GymMangmentBLL.Service.Interfaces;
using GymMangmentBLL.Service.Interfaces.AttachmentService;
using GymMangmentBLL.ViewModels.MemberViewModel;
using GymMangmerDAL.Entities;
using GymMangmerDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentBLL.Service.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAttachmentService _attachmentService;

        public MemberService(IUnitOfWork unitOfWork,IMapper mapper ,IAttachmentService attachmentService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _attachmentService = attachmentService;
        }


        public bool createMember(CreateMemberViewModel createMember)
        {
            try
            {
                if (IsEmailExists(createMember.Email) || IsPhoneExists(createMember.Phone))
                    return false;
                var photoName = _attachmentService.Upload("members", createMember.PhotoFile);
                if (string.IsNullOrEmpty(photoName))
                    return false;
                var member= _mapper.Map<Member>(createMember);
                member.photo = photoName;
                _unitOfWork.GetRepository<Member>().Add(member);
                var Iscreated= _unitOfWork.SaveChanges() > 0;
                if (!Iscreated)
                {
                    _attachmentService.Delete(photoName, "members");
                    return false;
                }
                else
                {
                    return Iscreated;
                }
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
            var ActiveMembership = _unitOfWork.GetRepository<MemberShip>().GetAll(x => x.MemberId == Memberid && x.Status == "Active").FirstOrDefault();
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
                if (Membership.Any())
                {
                    foreach (var membership in Membership)
                    {
                        MembershipRepo.Delete(membership);
                    }
                }
                MemberRepo.Delete(Member);
                var IsDeleted= _unitOfWork.SaveChanges() > 0;
                if (IsDeleted)
                    _attachmentService.Delete(Member.photo, "members");
                return IsDeleted;

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
                var phoneExists = _unitOfWork.GetRepository<Member>().GetAll(x => x.Phone == memberToUpdate.Phone && x.Id != Memberid);
                var emailExists = _unitOfWork.GetRepository<Member>().GetAll(x => x.Email == memberToUpdate.Email && x.Id != Memberid);
                if (phoneExists.Any() || emailExists.Any()) 
                    return false;

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
                return _unitOfWork.SaveChanges() > 0;
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
