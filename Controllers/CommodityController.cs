using Mercury_Backend.Contexts;
using Mercury_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Mercury_Backend.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Cosmos.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mercury_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommodityController : ControllerBase
    {
        private readonly ModelContext context;
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
                msg["Status"] = "Success";
            }
            catch(Exception e)
            {
                msg["Status"] = "Fail";
            }
            return JsonConvert.SerializeObject(msg);
        }

        // GET api/<CommodityController>/5
        [HttpGet("{id}")]
        public string Get(string id)
        {
            JObject msg = new JObject();
            try
            {
                var commodityList = context.Commodities.FirstOrDefault(b => b.Id == id);
                msg["targetCommodity"] = JToken.FromObject(commodityList);
                msg["Status"] = "Success";
            }
            catch(Exception e)
            {
                msg["Status"] = "Fail";
            }
            return JsonConvert.SerializeObject(msg);
        }

        // POST api/<CommodityController>
        [HttpPost]
        public async Task<string> Post([FromForm]Commodity newCommodity, [FromForm] List<IFormFile> videos)
        {
            JObject msg = new JObject();
            var id = Generator.GenerateId(12);
            newCommodity.Id = id;
            var videoPaths = new List<string>();
            try
            {
                Console.WriteLine(videos.Count());
                if (videos.Count() == 0)
                {
                    Console.WriteLine("No videos uploaded.");
                }
                for (int i = 0; i < videos.Count(); i++)
                {
                    var tmpVideoId = Generator.GenerateId(20);
                    var splitFileName =videos[i].FileName.Split('.');
                    var len = splitFileName.Length;
                    var postFix = splitFileName[len - 1];
                    var path = "Media" + "/Video/" + tmpVideoId + '.' + postFix;
                    if (Directory.Exists(path))
                    {
                        Console.WriteLine("This path exists.");
                    }
                    else
                    {
                        Directory.CreateDirectory("Media");
                        Directory.CreateDirectory("Media/Video");
                    }
                    // Console.WriteLine(path);
                    Console.WriteLine(path);
                    videoPaths.Add(path);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await videos[i].CopyToAsync(stream);
                    }

                    var video = new Medium();
                    video.Id = tmpVideoId;
                    video.Type = "Video";
                    video.Path = path;
                    context.Media.Add(video);
                    newCommodity.VideoId = tmpVideoId;
                }

                
                
                context.Commodities.Add(newCommodity);
                Console.WriteLine("haha");
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
