using System;

namespace Mercury_Backend.Models
{
    public class SimplifiedComment
    {
        public string CommentId { get; set; }
        public string SenderId { get; set; }
        public string SenderName { get; set; }
        public string AvatarPath { get; set; }
        public DateTime Time { get; set; }
        public string Content { get; set; }

        public SimplifiedComment(string commentId, string senderId, string senderName, string avatarPath, DateTime time, string content)
        {
            CommentId = commentId;
            SenderId = senderId;
            SenderName = senderName;
            AvatarPath = avatarPath;
            Time = time;
            Content = content;
        }
    }
}