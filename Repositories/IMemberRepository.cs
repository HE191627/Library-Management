using BusinessObjects;

namespace Repositories
{
    public interface IMemberRepository
    {
        List<Member> GetMembers();
        Member? GetMemberById(int id);
        void AddMember(Member member);
        void UpdateMember(Member member);
        void DeleteMember(int id);
    }
}
