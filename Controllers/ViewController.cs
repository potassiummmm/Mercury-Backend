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

    public class ViewController : ControllerBase
    {
        private ModelContext context;
        public ViewController(ModelContext ModelContext)
        {
            context = ModelContext;
        }
        // GET: api/<ViewController>


        [HttpGet]
        public String Get()
        {
            JObject msg = new JObject();
            try
            {
                var list = context.Views.OrderBy(b => b.UserId);
                msg["ViewList"] = JToken.FromObject(list);
                msg["Code"] = "200";
            }
            catch (Exception e)
            {
                msg["Code"] = "400";
            }
            return JsonConvert.SerializeObject(msg);
        }


        [HttpGet("{userId}")]
        public string Get(string userId)
        {
            JObject msg = new JObject();
            try
            {
                var list = context.Views
                    .Where(e => e.UserId == userId)
                    .OrderBy(e => e.Time);
                msg["ViewList"] = JToken.FromObject(list);
                msg["Code"] = "200";
            }
            catch (Exception e)
            {
                msg["Code"] = "400";
                Console.WriteLine(e.ToString());
            }
            return JsonConvert.SerializeObject(msg);
        }

        [HttpPost]
        public String Post([FromForm] View NewView)
        {
            JObject msg = new JObject();
            try
            {
                NewView.Time = DateTime.Now;
                context.Views.Add(NewView);
                context.SaveChanges();
                msg["Code"] = "200";
            }
            catch (Exception e)
            {
                msg["Code"] = "400";
                Console.WriteLine(e.ToString());
            }
            return JsonConvert.SerializeObject(msg);
        }

        [HttpDelete("{userId}")]
        public string delete(string userId)
        {
            JObject msg = new JObject();
            try
            {
                var views = context.Views.Where(e => e.UserId == userId);
                if (views != null)
                {
                    foreach (var view in views)
                    {
                        context.Views.Remove(view);
                        context.SaveChanges();
                        msg["Code"] = "200";
                    }


                }
            }
            catch (Exception e)
            {
                msg["Code"] = "400";
                Console.WriteLine(e.ToString());
            }
            return JsonConvert.SerializeObject(msg);
        }
    }
















}
