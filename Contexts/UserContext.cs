using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Mercury_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Mercury_Backend.Contexts
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseOracle(@"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=150.158.185.96)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=xe)));Persist Security Info=True;User ID=yuanzhang;Password=123456;");
        }
 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
