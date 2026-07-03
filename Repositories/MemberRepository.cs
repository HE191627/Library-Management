using BusinessObjects;
using DataAccessLayer;

namespace Repositories
{
    public class MemberRepository : IMemberRepository
    {
        public List<Member> GetMembers() => MemberDAO.GetMembers();
        public Member? GetMemberById(int id) => MemberDAO.GetMemberById(id);
        public void AddMember(Member member) => MemberDAO.AddMember(member);
        public void UpdateMember(Member member) => MemberDAO.UpdateMember(member);
        public void DeleteMember(int id) => MemberDAO.DeleteMember(id);
    }
}