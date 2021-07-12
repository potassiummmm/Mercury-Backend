using Mercury_Backend.Contexts;
using Mercury_Backend.Models;
using Mercury_Backend.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public string Get([FromForm] string userId, [FromForm] int maxNumber = 10, [FromForm] int pageNumber = 1)
        {
            JObject msg = new JObject();
            try
            {
                List<Order> orderList = new List<Order>();
                if(userId != null)
                {
                    orderList = context.Orders.Include(order => order.Commodity)
                        .ThenInclude(commodity => commodity.CommodityImages)
                        .ThenInclude(commodityImages => commodityImages.Image)
                        .Where(order => order.BuyerId == userId).OrderByDescending(order => order.Time).ToList();
                }
                else
                {
                    orderList = context.Orders.Include(order => order.Commodity)
                        .ThenInclude(commodity => commodity.CommodityImages)
                        .ThenInclude(commodityImages => commodityImages.Image)
                        .OrderByDescending(order => order.Time).ToList();
                }
                var simplifiedOrderList = new List<SimplifiedOrder>();
                for(int i = 0; i + (pageNumber - 1) * maxNumber < orderList.Count(); ++i)
                {
                    //var commodity = context.Commodities.Where(commodity => commodity.Id == orderList[i].CommodityId).ToList();
                    //var owner = context.SchoolUsers.Where(user => user.SchoolId == commodity[0].OwnerId).ToList();
                    //commodity[0].Owner = owner[0];
                    //orderList[i].Commodity = commodity[0];
                    simplifiedOrderList.Add(orderList[i + (pageNumber - 1) * maxNumber].Simplify());
                }
                msg["OrderList"] = JToken.FromObject(simplifiedOrderList);
                msg["Status"] = "Success";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                msg["Status"] = "Fail";
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
                msg["order"] = JToken.FromObject(orderList);
                msg["Status"] = "Success";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                msg["Status"] = "Fail";
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
                msg["Status"] = "Success";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                msg["Status"] = "Fail";
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
                msg["Status"] = "Fail";
                msg["FailReason"] = "Wrong status";
                return JsonConvert.SerializeObject(msg);
            }
            try
            {
                var order = context.Orders.Where(order => order.Id == id).ToList<Order>();
                if (order != null)
                {
                    if(order[0].Status != "UNPAID")
                    {
                        msg["Status"] = "Fail";
                        msg["FailReason"] = "Cannot change the status of paid or cancelled order";
                        return JsonConvert.SerializeObject(msg);
                    }
                    order[0].Status = newStatus;
                    context.SaveChanges();
                    msg["Status"] = "Success";
                }
                else
                {
                    msg["Status"] = "Fail";
                    msg["FailReason"] = "Order not found";
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                msg["Status"] = "Fail";
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
                msg["RatingList"] = JToken.FromObject(ratingList);
                msg["Status"] = "Success";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                msg["Status"] = "Fail";
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
                msg["Status"] = "Success";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                msg["Status"] = "Fail";
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
                msg["Status"] = "Success";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                msg["Status"] = "Fail";
            }
            return JsonConvert.SerializeObject(msg);
        }
    }
}
