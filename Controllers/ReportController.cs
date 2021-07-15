using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Linq;
using Mercury_Backend.Contexts;
using Mercury_Backend.Models;
using Microsoft.EntityFrameworkCore;
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
            msg["Report"] = JToken.FromObject(list);
            return JsonConvert.SerializeObject(msg);
        }
        
        // POST api/<ReportController>
        [HttpPost]
        public String Post([FromForm] string reporterId,[FromForm] string informantId,[FromForm] string comment)
        {
            JObject msg = new JObject();
            try
            {
                var report = new ReportUser()
                {
                    ReporterId = reporterId,
                    InformantId = informantId,
                    Comment = comment,
                    Time = DateTime.Now,
                    Status = "Y"
                };
                context.ReportUsers.Add(report);
                context.SaveChanges();
                msg["Code"] = "200";
                msg["Description"] = "Reported successfully.";
            }
            catch (DbUpdateException e)
            {
                msg["Code"] = "403";
                msg["Description"] = "Failed to update database.";
                Console.WriteLine(e.ToString());
            }
            catch (DBConcurrencyException e)
            {
                Console.WriteLine(e.ToString());
                msg["Code"] = "403";
                msg["Description"] = "Failed to update database because of concurrent requests.";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                msg["Code"] = "400";
            }
            return JsonConvert.SerializeObject(msg);
        }
    }
}