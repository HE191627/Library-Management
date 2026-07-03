using BusinessObjects;

namespace Services
{
    public interface IMemberService
    {
        List<Member> GetMembers();
        List<Member> GetActiveMembers();
        Member? GetMemberById(int id);
        void RegisterMember(Member member);
        void UpdateMember(Member member);
        void DeleteMember(int id);
    }
}
