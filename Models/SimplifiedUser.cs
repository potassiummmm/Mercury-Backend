namespace Mercury_Backend.Models
{
    public class SimplifiedUser
    {
        public string Id { get; set; }
        public string NickName { get; set; }
        public string AvatarPath { get; set; }
        public string RealName { get; set; }
        public string Role { get; set; }

        public SimplifiedUser(string id, string nickName, string avatarPath, string realName, string role)
        {
            Id = id;
            NickName = nickName;
            AvatarPath = avatarPath;
            RealName = realName;
            Role = role;
        }
    }
}