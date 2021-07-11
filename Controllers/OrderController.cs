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
        private readonly ModelContext context;
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
                var orderList = context.Orders.OrderByDescending(order => order.Time).ToList<Order>();
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
        public string Get(string id)
        {
            JObject msg = new JObject();
            try
            {
                var orderList = context.Orders.Where(order => order.Id == id).ToList<Order>();
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
        public string Put(string id, [FromForm]string newStatus)
        {
            JObject msg = new JObject();
            if(newStatus != "PAID" && newStatus != "CANCELLED")
            {
                msg["status"] = "fail";
                msg["failReason"] = "Wrong status";
                return JsonConvert.SerializeObject(msg);
            }
            try
            {
                var order = context.Orders.Where(order => order.Id == id).ToList<Order>();
                if (order != null)
                {
                    if(order[0].Status != "UNPAID")
                    {
                        msg["status"] = "fail";
                        msg["failReason"] = "Cannot change the status of paid or cancelled order";
                        return JsonConvert.SerializeObject(msg);
                    }
                    order[0].Status = newStatus;
                    context.SaveChanges();
                    msg["status"] = "success";
                }
                else
                {
                    msg["status"] = "fail";
                    msg["failReason"] = "Order not found";
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                msg["status"] = "fail";
            }
            return JsonConvert.SerializeObject(msg);
        }

        // DELETE api/<OrderController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        //GET api/order/<OrderId>/rating
        [HttpGet("{orderId}/rating")]
        public string GetRating(string orderId)
        {
            JObject msg = new JObject();
            try
            {
                var ratingList = context.Ratings.Where(rating => rating.OrderId == orderId).ToList<Rating>();
                msg["ratingList"] = JToken.FromObject(ratingList);
                msg["status"] = "success";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                msg["status"] = "fail";
            }
            return JsonConvert.SerializeObject(msg);
        }

        //POST api/order/<OrderId>/rating
        [HttpPost("{id}/rating")]
        public string PostRating(string id, [FromForm]Rating rating)
        {
            JObject msg = new JObject();
            try
            {
                rating.RatingId = Generator.GenerateId(20);
                rating.OrderId = id;
                rating.Time = DateTime.Now;
                context.Ratings.Add(rating);
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

        //DELETE api/order/<OrderId>/rating/<RatingId>
        [HttpDelete("{orderId}/rating/{ratingId}")]
        public string DeleteRating(string id, string ratingId)
        {
            JObject msg = new JObject();
            try
            {
                var rating = context.Ratings.Where(rating => rating.RatingId == ratingId).ToList<Rating>();
                context.Ratings.Remove(rating[0]);
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
    }
}
