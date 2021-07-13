using System;
using System.Collections.Generic;

namespace Mercury_Backend.Models
{
    public class PostDetail
    {
        public string Id { get; set; }
        public string SenderId { get; set; }
        public string SenderName { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }
        public string AvatarPath { get; set; }
        public List<SimplifiedComment> Comments { get; set; }
        public List<string> Images { get; set; }

        public PostDetail(string id, string senderId, string senderName, string title, string content, DateTime time, string avatarPath, List<SimplifiedComment> comments, List<string> images)
        {
            Id = id;
            SenderId = senderId;
            SenderName = senderName;
            Title = title;
            Content = content;
            Time = time;
            AvatarPath = avatarPath;
            Comments = comments;
            Images = images;
        }
    }
}