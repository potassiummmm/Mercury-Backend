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
            msg["ItemList"] = JToken.FromObject(list);
            msg["Code"] = "200";
            return JsonConvert.SerializeObject(msg);

        }



        [HttpGet("{userId}")]
        public string Get(string userId)
        {
            JObject msg = new JObject();
            try
            {
                var itemList = context.ShoppingCarts.Where(b => b.UserId == userId).ToList<ShoppingCart>();
                msg["ItemList"] = JToken.FromObject(itemList);
                msg["Code"] = "200";
                //var k = context.Commodities.Find(userId).Price.GetType();
                var imageList = new List<string>();
                var priceList = new List<decimal?>();
                var nameList = new List<string>();
                foreach(var item in itemList)
                {
                    imageList.Add(context.Commodities.Find(item.CommodityId).Cover);
                    priceList.Add(context.Commodities.Find(item.CommodityId).Price);
                    nameList.Add(context.Commodities.Find(item.CommodityId).Name);
                }
                msg["ImageList"] = JToken.FromObject(imageList);
                msg["PriceList"] = JToken.FromObject(priceList);
                msg["NameList"] =  JToken.FromObject(nameList);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                msg["Code"] = "400";
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
                msg["Code"] = "400";
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
                msg["Code"] = "400";
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
















