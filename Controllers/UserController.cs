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
        public string Get(int id)
        {
            return "value";
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
                msg["status"] = "success";
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