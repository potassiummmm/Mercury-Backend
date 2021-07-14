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
            msg["Code"] = "200";
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
                msg["Code"] = "200";

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                msg["Code"] = "405";
            }
            return JsonConvert.SerializeObject(msg);
        }



        [HttpPost]
        public String Post([FromForm] ShoppingCart ShoppingCartItem)
        {
            JObject msg = new JObject();
            try
            {
                if (context.ShoppingCarts.Find(ShoppingCartItem.CommodityId, ShoppingCartItem.UserId) == null)
                {
                    if (ShoppingCartItem.Count == null) ShoppingCartItem.Count = 1;
                    if (ShoppingCartItem.AddTime == null) ShoppingCartItem.AddTime= DateTime.Now;
                    context.ShoppingCarts.Add(ShoppingCartItem);

                    
                }
                else
                {
                    var item = context.ShoppingCarts.Find(ShoppingCartItem.CommodityId, ShoppingCartItem.UserId);
                    item.Count += ShoppingCartItem.Count== null?1: ShoppingCartItem.Count;
                }
                context.SaveChanges();
                msg["Code"] = "200";
            }
            catch (Exception e)
            {
                msg["Code"] = "405";
                Console.WriteLine(e.ToString());
            }
            return JsonConvert.SerializeObject(msg);
        }


        [HttpDelete]
        public string delete([FromForm]string commodityId, [FromForm]string userId)
        {
            JObject msg = new JObject();
            var ShoppingCartItem = context.ShoppingCarts.Find(commodityId, userId);

            if (ShoppingCartItem == null)
            {
                msg["Code"] = "404";
                return JsonConvert.SerializeObject(msg);
            }
            try
            {
                context.ShoppingCarts.Remove(ShoppingCartItem);
                context.SaveChanges();
                msg["Code"] = "200";
            }
            catch (Exception e)
            {
                msg["Code"] = "405";
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
                msg["Code"] = "404";
                return JsonConvert.SerializeObject(msg);
            }
            // return JsonSerializer.Serialize(ShoppingCartItem);
            foreach (var item in ShoppingCartItem)
            {
                if (item.CommodityId == commodityId)
                {
                        item.Count=count;
                        context.SaveChanges();

                    
                        msg["Code"] = "200";
                }
            }

            return JsonConvert.SerializeObject(msg);

        }




    }
}
















