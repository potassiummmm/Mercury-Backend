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
    public class ReportController : ControllerBase
    {
        private readonly ModelContext context;
        private readonly IConfiguration config;
        public ReportController(ModelContext modelContext, IConfiguration configuration)
        {
            context = modelContext;
            config = configuration;
        }
        // GET: api/<ReportController>
        [HttpGet("{userId}")]
        public String Get(string userId)
        {
            JObject msg = new JObject();
            var list = context.ReportUsers
                .Where(report => report.ReporterId == userId || report.InformantId == userId)
                .OrderBy(report => report.Time);
            msg["charRecord"] = JToken.FromObject(list);
            return JsonConvert.SerializeObject(msg);
        }
        
        // POST api/<ReportController>
        [HttpPost]
        public String Post([FromForm]ReportUser report)
        {
            JObject msg = new JObject();
            try
            {
                context.ReportUsers.Add(report);
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
    }
}