using GymMangmentBLL.ViewModels.MemberViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentBLL.Service.Interfaces
{
    internal interface IMemberService
    {
        IEnumerable<MemberViewModels> GetAllMember();
        bool createMember(CreateMemberViewModel createMember);
        MemberViewModels? GetMemberDetails(int Memberid);
        HealthRecordViewModel? GetMemberHealthRecordDetails(int Memberid);
        MemberToUpdateViewModel? GetMemberForUpdate(int Memberid);
        bool UpdateMember(int Memberid, MemberToUpdateViewModel memberToUpdate);
        bool RemoveMember(int Memberid);
    }
}
