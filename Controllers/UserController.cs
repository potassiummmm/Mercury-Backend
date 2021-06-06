using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mercury_Backend.Contexts;
using Mercury_Backend.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mercury_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // GET: api/<UserController>
        [HttpGet]
        public String Get()
        {
            var context = new UserContext();
            var list = context.Users.OrderBy(b => b.Id);
            System.Console.WriteLine(list);
            String jsonString = JsonSerializer.Serialize(list);
            Console.WriteLine(jsonString);
            return jsonString;
            //return new string[] { "value1", "value2" };
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UserController>
        [HttpPost]
        public void Post([FromForm]User NewUser)
        {
            //String Id, String NickName, String RealName, String Phone,
            //String Password, String Major, int Credit, String Role, int Grade,
            //String Brief = "", String AvatarId = ""
            using (var context = new UserContext())
            {
                var user = new User
                {
                    Id = NewUser.Id,
                    NickName = NewUser.NickName,
                    RealName = NewUser.RealName,
                    Phone = NewUser.Phone,
                    Password = NewUser.Password,
                    Major = NewUser.Major,
                    Credit = NewUser.Credit,
                    Role = NewUser.Role,
                    Grade = NewUser.Grade,
                    Brief = NewUser.Brief,
                    AvatarId = NewUser.AvatarId
                };

                context.Users.Add(user);
                context.SaveChanges();
            }
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
