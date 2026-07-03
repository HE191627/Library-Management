using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class MemberDAO
    {
        public static List<Member> GetMembers()
        {
            using var db = new LibraryEliteDBContext();
            return db.Members.ToList();
        }

        public static Member? GetMemberById(int id)
        {
            using var db = new LibraryEliteDBContext();
            return db.Members.Find(id);
        }

        public static void AddMember(Member member)
        {
            using var db = new LibraryEliteDBContext();
            db.Members.Add(member);
            db.SaveChanges();
        }

        public static void UpdateMember(Member member)
        {
            using var db = new LibraryEliteDBContext();
            db.Entry(member).State = EntityState.Modified;
            db.SaveChanges();
        }

        public static void DeleteMember(int id)
        {
            using var db = new LibraryEliteDBContext();
            var member = db.Members.Find(id);
            if (member != null)
            {
                db.Members.Remove(member);
                db.SaveChanges();
            }
        }
    }
}