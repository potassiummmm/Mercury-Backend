using System;
using System.Collections.Generic;

#nullable disable

namespace Mercury_Backend.Models
{
    public partial class Classification
    {
        public Classification()
        {
            Commodities = new HashSet<Commodity>();
        }

        public byte Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Commodity> Commodities { get; set; }
    }
}
