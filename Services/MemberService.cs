using BusinessObjects;
using Repositories;

namespace Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepo = new MemberRepository();

        public List<Member> GetMembers() => _memberRepo.GetMembers();

        public List<Member> GetActiveMembers()
        {
            return _memberRepo.GetMembers().Where(m => m.Status == "Active").ToList();
        }

        public Member? GetMemberById(int id) => _memberRepo.GetMemberById(id);

        public void RegisterMember(Member member)
        {
            // LOGIC: Kiểm tra Email trùng trước khi cho đăng ký
            var list = _memberRepo.GetMembers();
            if (list.Any(m => m.Email == member.Email))
                throw new Exception("Email này đã được sử dụng!");

            _memberRepo.AddMember(member);
        }

        public void UpdateMember(Member member)
        {
            _memberRepo.UpdateMember(member);
        }

        public void DeleteMember(int id)
        {
            _memberRepo.DeleteMember(id);
        }
    }
}