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
            var list =  context.ShoppingCarts.OrderBy(b => b.UserId);
            jsonString = JsonSerializer.Serialize(list);
            Console.WriteLine(jsonString);
            return jsonString;
          
        }


        [HttpGet("{id}")]
        public string Get(string id)
        {
            String jsonString = "";
            var list = context.ShoppingCarts
                .Where(e => e.UserId == id)
                .OrderBy(e => e.CommodityId);
            jsonString = JsonSerializer.Serialize(list);
            Console.WriteLine(jsonString);
            return jsonString;


        }

        [HttpPost]
        public String Post([Bind("CommodityId,UserId,Count,AddTime")] ShoppingCart ShoppingCartItem)
        {
            JObject msg = new JObject();
            try
            {
                context.ShoppingCarts.Add(ShoppingCartItem);
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
        public string delete(string commodityId,string userId)
        {
            JObject msg = new JObject();
            var ShoppingCartItem= context.ShoppingCarts.Find(commodityId,userId);
            if (ShoppingCartItem == null)
            {
                msg["status"] = "fail";
            }
            context.ShoppingCarts.Remove(ShoppingCartItem);
            context.SaveChanges();
            return JsonConvert.SerializeObject(msg);
        }

        
    }
















}
