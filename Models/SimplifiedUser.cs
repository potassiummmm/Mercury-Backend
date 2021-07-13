namespace Mercury_Backend.Models
{
    public class SimplifiedUser
    {
        public bool LoggedIn { get; set; }
        public string Id { get; set; }
        public string NickName { get; set; }
        public string AvatarPath { get; set; }
        public string RealName { get; set; }
        public string Role { get; set; }

        public SimplifiedUser(bool loggedIn, string id, string nickName, string avatarPath, string realName, string role)
        {
            LoggedIn = loggedIn;
            Id = id;
            NickName = nickName;
            AvatarPath = avatarPath;
            RealName = realName;
            Role = role;
        }
    }
}