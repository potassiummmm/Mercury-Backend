using Mercury_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mercury_Backend.Contexts;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mercury_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ShoppingCartController : ControllerBase
    {
        private ModelContext context;
        public ShoppingCartController(ModelContext ModelContext)
        {
            context = ModelContext;
        }
        // GET: api/<ShoppingCartController>


        [HttpGet]
        public String Get()
        {
            String jsonString = "";
            var list = context.ShoppingCarts.OrderBy(b => b.UserId);
            jsonString = JsonSerializer.Serialize(list);
            Console.WriteLine(jsonString);
            return jsonString;

        }

        /*
        [HttpGet("{userId}")]
        public string Get(string userId)
        {
            String jsonString = "";
            var list = context.ShoppingCarts
                .Where(e => e.UserId == userId).ToList();
            jsonString = JsonSerializer.Serialize(list);
            Console.WriteLine(jsonString);
            return jsonString;


        }
        */

        [HttpGet("{userId}")]
        public string Get(string userId)
        {
            JObject msg = new JObject();
            String jsonString = "";
            try
            {
                var list = context.ShoppingCarts.Where(b => b.UserId == userId).ToList<ShoppingCart>();
                jsonString = JsonSerializer.Serialize(list);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                msg["status"] = "fail";
            }
            return jsonString;
        }



        [HttpPost]
        public String Post([FromForm] ShoppingCart ShoppingCartItem)
        {
            JObject msg = new JObject();
            try
            {
                context.ShoppingCarts.Add(ShoppingCartItem);

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


        [HttpDelete("{commodityId,userId}")]
        public string delete(string commodityId, string userId)
        {
            JObject msg = new JObject();
            var ShoppingCartItem = context.ShoppingCarts.Where(e => e.UserId == userId);

            if (ShoppingCartItem == null)
            {
                msg["status"] = "fail";
                return JsonConvert.SerializeObject(msg);
            }
            try
            {
                // return JsonSerializer.Serialize(ShoppingCartItem);
                foreach (var item in ShoppingCartItem)
                {
                    if (item.CommodityId == commodityId)
                    {
                        context.ShoppingCarts.Remove(item);

                        context.SaveChanges();

                    }
                    msg["status"] = "success";
                }

                // msg["status"] = "success";
            }
            catch (Exception e)
            {
                msg["status"] = "fail";
                Console.WriteLine(e.ToString());
            }
            return JsonConvert.SerializeObject(msg);
        }

        [HttpPut("{commdityId,userId,count}")]
        public string Put(string commodityId, string userId,  byte count)
        {
            JObject msg = new JObject();
            var ShoppingCartItem = context.ShoppingCarts.Where(e => e.UserId == userId);

            if (ShoppingCartItem == null)
            {
                msg["status"] = "fail";
                return JsonConvert.SerializeObject(msg); ;
            }
            // return JsonSerializer.Serialize(ShoppingCartItem);
            foreach (var item in ShoppingCartItem)
            {
                if (item.CommodityId == commodityId)
                {

                    
                        item.Count=count;

                       

                        context.SaveChanges();

                    
                        msg["status"] = "success";
                }

                


                
            }
            return JsonConvert.SerializeObject(msg);

        }




    }
}
















