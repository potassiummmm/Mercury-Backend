using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Mercury_Backend.Contexts;
using Mercury_Backend.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;

namespace Mercury_Backend.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly ModelContext context;
        private readonly IConfiguration config;
        public ChatController(ModelContext modelContext, IConfiguration configuration)
        {
            context = modelContext;
            config = configuration;
        }
        // GET: api/<ChatController>/1/2
        [HttpGet("{userId}/{targetId}")]
        public String Get(string userId, string targetId)
        {
            JObject msg = new JObject();
            var list = context.ChatRecords
                .Where(record => record.SenderId == userId && record.ReceiverId == targetId || record.SenderId == targetId && record.ReceiverId == userId)
                .OrderBy(record => record.Time);
            msg["CharRecord"] = JToken.FromObject(list);
            return JsonConvert.SerializeObject(msg);
        }
        
        // POST api/<ChatController>
        [HttpPost]
        public String Post([FromForm]ChatRecord record)
        {
            JObject msg = new JObject();
            try
            {
                context.ChatRecords.Add(record);
                context.SaveChanges();
                msg["Code"] = "200";
                msg["Description"] = "Message sent successfully.";
            }
            catch (Exception e)
            {
                msg["Code"] = "500";
                msg["Description"] = "Failed to update database.";
                Console.WriteLine(e.ToString());
            }
            return JsonConvert.SerializeObject(msg);
        }
    }
}