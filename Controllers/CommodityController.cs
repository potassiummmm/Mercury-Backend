using Mercury_Backend.Contexts;
using Mercury_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mercury_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommodityController : ControllerBase
    {
        private ModelContext context;
        public CommodityController(ModelContext modelContext)
        {
            context = modelContext;
        }
        // GET: api/<CommodityController>
        [HttpGet]
        public string Get()
        {
            JObject msg = new JObject();
            try
            {
                var commodityList = context.Commodities.OrderBy(b => b.Id).ToList<Commodity>();
                msg["commodityList"] = JToken.FromObject(commodityList);
                msg["status"] = "success";
            }
            catch(Exception e)
            {
                msg["status"] = "fail";
            }
            return JsonConvert.SerializeObject(msg);
        }

        // GET api/<CommodityController>/5
        [HttpGet("{id}")]
        public string Get(string id)
        {
            var list = context.Commodities.OrderBy(b => b.Id == id);
            string jsonString = JsonSerializer.Serialize(list);
            Console.WriteLine(jsonString);
            return jsonString;
        }

        // POST api/<CommodityController>
        [HttpPost]
        public string Post([FromForm]Commodity commodity)
        {
            JObject msg = new JObject();
            try
            {
                context.Commodities.Add(commodity);
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

        // PUT api/<CommodityController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CommodityController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
