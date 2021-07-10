using Mercury_Backend.Contexts;
using Mercury_Backend.Models;
using Mercury_Backend.Utils;
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
    public class OrderController : ControllerBase
    {
        private ModelContext context;
        private static Random random;
        public OrderController(ModelContext modelContext)
        {
            context = modelContext;
            random = new Random();
        }
        // GET: api/<OrderController>
        [HttpGet]
        public string Get()
        {
            JObject msg = new JObject();
            try
            {
                var orderList = context.Orders.OrderByDescending(b => b.Time).ToList<Order>();
                msg["orderList"] = JToken.FromObject(orderList);
                msg["status"] = "success";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                msg["status"] = "fail";
            }
            return JsonConvert.SerializeObject(msg);
        }

        // GET api/<OrderController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<OrderController>
        [HttpPost]
        public string Post([FromForm] Order order)
        {
            JObject msg = new JObject();
            try
            {
                order.Id = Generator.GenerateId(20);
                order.Time = DateTime.Now;
                order.ReturnTime = Convert.ToDateTime(order.ReturnTime);
                order.Status = "UNPAID";
                context.Orders.Add(order);
                context.SaveChanges();
                msg["status"] = "success";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                msg["status"] = "fail";
            }
            return JsonConvert.SerializeObject(msg);
        }

        // PUT api/<OrderController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<OrderController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
