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
            String jsonString = "";
            var list = context.Views.OrderBy(b => b.UserId);
            jsonString = JsonSerializer.Serialize(list);
            Console.WriteLine(jsonString);
            return jsonString;

        }


        [HttpGet("{id}")]
        public string Get(string id)
        {
            String jsonString = "";
            var list = context.Views
                .Where(e => e.UserId == id)
                .OrderBy(e => e.CommodityId);
            jsonString = JsonSerializer.Serialize(list);
            Console.WriteLine(jsonString);
            return jsonString;


        }

        [HttpPost]
        public String Post([FromForm] View newView)
        {
            JObject msg = new JObject();
            try
            {
                context.Views.Add(newView);
                Console.WriteLine("okk");
                context.SaveChanges();
            }
            catch (Exception e)
            {
                msg["status"] = "fail";
                Console.WriteLine(e.ToString());
            }
            return JsonConvert.SerializeObject(msg);
        }

        [HttpDelete("{id}")]
        public string delete(string userId)
        {
            JObject msg = new JObject();

            var views = context.Views.Where(e => e.UserId == userId);
            foreach(var view in views)
            {
                context.Views.Remove(view);
            }
            
            context.SaveChanges();
            return JsonConvert.SerializeObject(msg);
        }


    }
















}
