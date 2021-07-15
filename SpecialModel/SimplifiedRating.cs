using System;

namespace Mercury_Backend.Models
{
    public class SimplifiedRating
    {
        public string UserName { get; set; }
        public string UserImage { get; set; }
        public string UserId { get; set; }
        public string Comment { get; set; }
        public byte? Rating { get; set; }
        

        public SimplifiedRating(string userName, string userImage, string userId, 
            string comment, byte? rating)
        {
            UserName = userName;
            UserImage = userImage;
            UserId = userId;
            Comment = comment;
            Rating = rating;
        }
    }
}