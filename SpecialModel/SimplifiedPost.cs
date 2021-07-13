using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mercury_Backend.Models
{
    public class SimplifiedPost
    {
        public string PostId { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string SenderId { get; set; }
        public string AvatarPath { get; set; }

        public SimplifiedPost(string postId, string title, string name, string content, string senderId, string avatarPath)
        {
            PostId = postId;
            Title = title;
            Name = name;
            SenderId = senderId;
            AvatarPath = avatarPath;
            if (content.Length > 30)
            {
                Content = content.Substring(0, 30) + "...";
            }
            else
            {
                Content = content;
            }
        }
    }
}
