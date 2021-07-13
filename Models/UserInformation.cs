namespace Mercury_Backend.Models
{
    public class UserInformation
    {
        public string SchoolId { get; set; }

        public string Nickname { get; set; }

        public string RealName { get; set; }

        public string Phone { get; set; }

        public string Major { get; set; }

        public byte? Credit { get; set; }

        public string Role { get; set; }

        public byte Grade { get; set; }

        public string Brief { get; set; }

        public string AvatarPath { get; set; }

        public UserInformation(string schoolId, string nickname, string realName, string phone, string major, byte? credit, string role, byte grade, string brief, string avatarPath)
        {
            SchoolId = schoolId;
            Nickname = nickname;
            RealName = realName;
            Phone = phone;
            Major = major;
            Credit = credit;
            Role = role;
            Grade = grade;
            Brief = brief;
            AvatarPath = avatarPath;
        }
    }
}