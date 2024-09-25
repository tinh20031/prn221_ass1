using BusinessObject;
using DataAccess.DTOs.Member;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public interface IMemberRepository
    {
        Member? Login(string email, string password);

        void AddMember(CreateMemberDto createMemberDto);

        List<GetMemberDto> GetMembers(string keyword = "");

        Member? GetMemberById(int id);

        void UpdateMember(UpdateMemberDto updateMemberDto);

        void DeleteMember(int memberId);
    }

}
