using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mercury_Backend.Contexts;
using Mercury_Backend.Models;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using JsonSerializer = System.Text.Json.JsonSerializer;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mercury_Backend.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private ModelContext context;
        public UserController(ModelContext modelContext)
        {
            context = modelContext;
        }
        // GET: api/<UserController>
        [HttpGet]
        public String Get()
        {
            JObject msg = new JObject();
            var list = context.SchoolUsers.OrderBy(b => b.SchoolId).ToList<SchoolUser>();
            msg["userList"] = JToken.FromObject(list);
            return JsonConvert.SerializeObject(msg);
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public string Get(string id)
        {
            JObject msg = new JObject();
            String jsonString = "";
            try
            {
                var user = context.SchoolUsers.Find(id);
                jsonString = JsonSerializer.Serialize(user);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                msg["status"] = "fail";
            }
            return jsonString;
        }

        // POST api/<UserController>
        [HttpPost]
        public String Post([FromForm]SchoolUser NewUser)
        {
            //String Id, String NickName, String RealName, String Phone,
            //String Password, String Major, int Credit, String Role, int Grade,
            //String Brief = "", String AvatarId = ""
            JObject msg = new JObject();
            try
            {
                context.SchoolUsers.Add(NewUser);
                Console.WriteLine("haha");
                context.SaveChanges();
            }
            catch (Exception e)
            {
                msg["status"] = "fail";
                Console.WriteLine(e.ToString());
            }
            return JsonConvert.SerializeObject(msg);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put([FromForm] SchoolUser value)
        {
            JObject msg = new JObject();
            var user=context.SchoolUsers.Find(value.SchoolId);
            /*
            if (value.Nickname != null) user.Nickname = value.Nickname;
            if (value.RealName != null) user.RealName = value.RealName;
            if (value.Phone != null) user.Phone = value.Phone;
            if (value.Password != null) user.Password = value.Password;
            if (value.Major != null) user.Major = value.Major;
            if (value.Credit != null) user.Credit = value.Credit;
            if (value.Role != null) user.Nickname = value.Nickname;
            if (value.Nickname != null) user.Nickname = value.Nickname;
            */
            foreach(var p in value.GetType().GetProperties())
            {
                if (p.GetValue(value) != null && p.Name != "SchoolId")
                {
                    context.Entry(value).Property(p.Name).IsModified = true;
                }

            }
            try
            {
                context.SaveChanges();
            }
            catch (Exception e)
            {
                msg["status"] = "fail";
                return;
            }
            return;

        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}