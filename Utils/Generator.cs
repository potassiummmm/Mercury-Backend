using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mercury_Backend.Utils
{
    public class Generator
    {
        private static Random random = new();

        public static string GenerateId(int length)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}
