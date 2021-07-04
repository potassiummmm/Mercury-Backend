using System;
using System.Collections.Generic;

#nullable disable

namespace Mercury_Backend.Models
{
    public partial class PostImage
    {
        public string ImageId { get; set; }
        public string PostId { get; set; }
        public string Position { get; set; }

        public virtual Medium Image { get; set; }
        public virtual NeedPost Post { get; set; }
    }
}
