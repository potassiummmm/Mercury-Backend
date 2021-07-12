using Mercury_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mercury_Backend.Contexts;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mercury_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikesController : ControllerBase
    {
        private readonly ModelContext context;
        public LikesController(ModelContext modelContext)
        {
            context = modelContext;
        }
        // GET: api/<LikesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<LikesController>/5
        [HttpGet("{userId}")]
        public string Get(string userId)
        {
            JObject msg = new JObject();
            try
            {
                var userList = context.Likes.Where(b => b.UserId == userId).ToList<Like>();
                msg["UserList"] = JToken.FromObject(userList);
                msg["User"] = JToken.FromObject(userList[0].User);
                msg["Status"] = "Success";
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                msg["Status"] = "Fail";
            }
            return JsonConvert.SerializeObject(msg);
        }

        // POST api/<LikesController>
        [HttpPost]
        public string Post([FromForm] Like like)
        {
            JObject msg = new JObject();
            try
            {
                context.Likes.Add(like);
                context.SaveChanges();
                msg["Status"] = "Success";
            }
            catch(Exception e)
            {
                msg["Status"] = "Fail";
                Console.WriteLine(e.ToString());
            }
            return JsonConvert.SerializeObject(msg);
        }

        // PUT api/<LikesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<LikesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
