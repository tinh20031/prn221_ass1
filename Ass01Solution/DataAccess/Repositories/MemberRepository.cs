using BusinessObject;
using DataAccess.DAO;
using DataAccess.DTOs.Member;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        public void AddMember(CreateMemberDto createMemberDto)
        {
            MemberDAO.Instance.AddMember(createMemberDto.Adapt<Member>());
            MemberDAO.Instance.SaveChanges();
        }

        public void DeleteMember(int memberId)
        {
            MemberDAO.Instance.DeleteMember(memberId);
            MemberDAO.Instance.SaveChanges();
        }

        public Member? GetMemberById(int id)
        {
            return MemberDAO.Instance.GetMemberById(id);
        }

        public List<GetMemberDto> GetMembers(string keyword = "")
        {
            return MemberDAO.Instance.GetMembers(keyword).Adapt<List<GetMemberDto>>();
        }

        public Member? Login(string email, string password)
        {
            return MemberDAO.Instance.Login(email, password);
        }

        public void UpdateMember(UpdateMemberDto updateMemberDto)
        {
            TypeAdapterConfig<UpdateMemberDto, Member>.NewConfig().IgnoreNullValues(true);

            var member = MemberDAO.Instance.GetMemberById(updateMemberDto.MemberId);

            updateMemberDto.Adapt(member);

            MemberDAO.Instance.SaveChanges();
        }
    }
}
