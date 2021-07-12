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
            JObject msg = new JObject();
            var list = context.ShoppingCarts.OrderBy(b => b.UserId);
            msg["UserList"] = JToken.FromObject(list);
            return JsonConvert.SerializeObject(msg);

        }



        [HttpGet("{userId}")]
        public string Get(string userId)
        {
            JObject msg = new JObject();
            try
            {
                var userList = context.ShoppingCarts.Where(b => b.UserId == userId).ToList<ShoppingCart>();
                msg["UserList"] = JToken.FromObject(userList);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                msg["Status"] = "Fail";
            }
            return JsonConvert.SerializeObject(msg);
        }



        [HttpPost]
        public String Post([FromForm] ShoppingCart ShoppingCartItem)
        {
            JObject msg = new JObject();
            try
            {
                context.ShoppingCarts.Add(ShoppingCartItem);

                context.SaveChanges();
                msg["Status"] = "Success";
            }
            catch (Exception e)
            {
                msg["Status"] = "Fail";
                Console.WriteLine(e.ToString());
            }
            return JsonConvert.SerializeObject(msg);
        }


        [HttpDelete]
        public string delete([FromForm]string commodityId, [FromForm]string userId)
        {
            JObject msg = new JObject();
            var ShoppingCartItem = context.ShoppingCarts.Where(e => e.UserId == userId);

            if (ShoppingCartItem == null)
            {
                msg["Status"] = "Fail";
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
                    msg["Status"] = "Success";
                }

                // msg["status"] = "success";
            }
            catch (Exception e)
            {
                msg["status"] = "Fail";
                Console.WriteLine(e.ToString());
            }
            return JsonConvert.SerializeObject(msg);
        }

        [HttpPut]
        public string Put([FromForm]string commodityId, [FromForm] string userId, [FromForm] byte count)
        {
            JObject msg = new JObject();
            var ShoppingCartItem = context.ShoppingCarts.Where(e => e.UserId == userId);

            if (ShoppingCartItem == null)
            {
                msg["Status"] = "fail";
                return JsonConvert.SerializeObject(msg);
            }
            // return JsonSerializer.Serialize(ShoppingCartItem);
            msg["Status"] = "Not Found";
            foreach (var item in ShoppingCartItem)
            {
                if (item.CommodityId == commodityId)
                {
                        item.Count=count;
                        context.SaveChanges();

                    
                        msg["Status"] = "Success";
                }
            }

            return JsonConvert.SerializeObject(msg);

        }




    }
}
















