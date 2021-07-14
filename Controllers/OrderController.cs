using Mercury_Backend.Contexts;
using Mercury_Backend.Models;
using Mercury_Backend.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
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
        public OrderController(ModelContext modelContext)
        {
            context = modelContext;
        }
        // GET: api/<OrderController>
        [HttpGet]
        public string Get([FromForm] string userId, [FromForm] string status, [FromForm] int maxNumber = 10, [FromForm] int pageNumber = 1)
        {
            JObject msg = new JObject();
            if (status != null && status != "PAID" && status != "UNPAID")
            {
                msg["Code"] = "403";
                msg["Description"] = "Invalid status";
                return JsonConvert.SerializeObject(msg);
            }
            try
            {
                List<Order> orderList = new List<Order>();
                if (userId != null)
                {
                    if (status != null)
                    {
                        orderList = context.Orders.Where(order => order.BuyerId == userId && order.Status == status)
                            .Include(order => order.Commodity)
                            .ThenInclude(commodity => commodity.CommodityImages)
                            .ThenInclude(commodityImages => commodityImages.Image)
                            .OrderByDescending(order => order.Time).ToList();
                    }
                    else
                    {
                        orderList = context.Orders.Where(order => order.BuyerId == userId)
                            .Include(order => order.Commodity)
                            .ThenInclude(commodity => commodity.CommodityImages)
                            .ThenInclude(commodityImages => commodityImages.Image)
                            .OrderByDescending(order => order.Time).ToList();
                    }
                }
                else
                {
                    if (status != null)
                    {
                        orderList = context.Orders.Where(order => order.Status == status)
                            .Include(order => order.Commodity)
                            .ThenInclude(commodity => commodity.CommodityImages)
                            .ThenInclude(commodityImages => commodityImages.Image)
                            .OrderByDescending(order => order.Time).ToList();
                    }
                    else
                    {
                        orderList = context.Orders.Include(order => order.Commodity)
                            .ThenInclude(commodity => commodity.CommodityImages)
                            .ThenInclude(commodityImages => commodityImages.Image)
                            .OrderByDescending(order => order.Time).ToList();
                    }
                }

                var simplifiedOrderList = new List<SimplifiedOrder>();
                for (int i = 0; i + (pageNumber - 1) * maxNumber < orderList.Count() && i < maxNumber; ++i)
                {
                    simplifiedOrderList.Add(Simplify.SimplifyOrder(orderList[i + (pageNumber - 1) * maxNumber]));
                }

                msg["OrderList"] = JToken.FromObject(simplifiedOrderList);
                msg["Code"] = "200";
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.ToString());
                msg["Code"] = "500";
                msg["Description"] = "Internal exception happens";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                msg["Code"] = "400";
                msg["Description"] = "Unknown exception";
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
                var order = context.Orders.Where(o => o.Id == id).Select(o => new
                {
                    OrderId = o.Id,
                    BuyerId = o.BuyerId,
                    Commodity = o.Commodity,
                    Count = o.Count,
                    Time = o.Time,
                    Location = o.Location,
                    ReturnTime = o.ReturnTime,
                    ReturnLocation = o.ReturnLocation,
                    Status = o.Status,
                }).Single();
                msg["order"] = JToken.FromObject(order, new JsonSerializer()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore //忽略循环引用，默认是throw exception
                });
                msg["Code"] = "200";
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.ToString());
                msg["Code"] = "500";
                msg["Description"] = "Internal exception happens";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                msg["Code"] = "400";
                msg["Description"] = "Unknown exception";
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
                // order.ReturnTime = Convert.ToDateTime(order.ReturnTime);
                order.Status = "UNPAID";
                context.Orders.Add(order);
                context.SaveChanges();
                msg["Code"] = "201";
                msg["OrderId"] = order.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                msg["Code"] = "400";
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
                msg["Code"] = "403";
                msg["Description"] = "Cannot change status to unpaid";
                return JsonConvert.SerializeObject(msg);
            }
            try
            {
                var order = context.Orders.Single(o => o.Id == id);
                if(order.Status != "UNPAID")
                {
                    msg["Code"] = "403";
                    msg["Description"] = "Cannot update a paid or cancelled order";
                    return JsonConvert.SerializeObject(msg);
                }
                order.Status = newStatus;
                context.SaveChanges();
                msg["Code"] = "200";
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e.ToString());
                msg["Code"] = "403";
                msg["Description"] = "Cannot update database";
            }
            catch (DBConcurrencyException e)
            {
                Console.WriteLine(e.ToString());
                msg["Code"] = "500";
                msg["Description"] = "Fail to update database because of concurrent requests";
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                msg["Code"] = "400";
                msg["Description"] = "Unknown exception";
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
                msg["Code"] = "200";
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.ToString());
                msg["Code"] = "500";
                msg["Description"] = "Internal exception happens";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                msg["Code"] = "400";
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
                msg["Code"] = "201";
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e.ToString());
                msg["Code"] = "403";
                msg["Description"] = "Cannot update database";
            }
            catch (DBConcurrencyException e)
            {
                Console.WriteLine(e.ToString());
                msg["Code"] = "500";
                msg["Description"] = "Fail to update database because of concurrent requests";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                msg["Code"] = "400";
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
                msg["Code"] = "200";
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e.ToString());
                msg["Code"] = "403";
                msg["Description"] = "Cannot update database";
            }
            catch (DBConcurrencyException e)
            {
                Console.WriteLine(e.ToString());
                msg["Code"] = "500";
                msg["Description"] = "Fail to update database because of concurrent requests";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                msg["Code"] = "400";
            }
            return JsonConvert.SerializeObject(msg);
        }
        
        // GET api/order/orderNumber
        [HttpGet("orderNumber")]
        public string GetOrderNumber([FromForm] string userId)
        {
            JObject msg = new JObject();
            try
            {
                int number;
                if (userId != null)
                {
                    number = context.Orders.Count(o => o.BuyerId == userId);
                }
                number = context.Orders.Count();
                msg["Code"] = "200";
                msg["PostNumber"] = number;
            }
            catch (Exception e)
            {
                msg["Code"] = "400";
            }
            return JsonConvert.SerializeObject(msg);
        }
    }
}
