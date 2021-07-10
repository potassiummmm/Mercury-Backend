using Azure.Core;
using Mercury_Backend.Contexts;
using Mercury_Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mercury_Backend.Utils;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mercury_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private ModelContext context;
        private static Random random;
        public PostController(ModelContext modelContext)
        {
            context = modelContext;
            random = new Random();
        }
        // GET: api/<PostController>
        [HttpGet]
        public string Get()
        {
            JObject msg = new JObject();
            try
            {
                var postList = context.NeedPosts.OrderBy(b => b.Time);
                msg["postList"] = JToken.FromObject(postList);
                msg["status"] = "success";
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                msg["status"] = "fail";
            }
            return JsonConvert.SerializeObject(msg);
        }

        // GET api/<PostController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<PostController>
        [HttpPost]
        public string Post([FromForm] NeedPost post, [FromForm] List<IFormFile> photos)
        {
            JObject msg = new JObject();
            try
            {
                post.Id = Generator.GenerateId(12);
                if (photos == null)
                {
                    Console.WriteLine("GG");
                }
                else
                {
                    Console.WriteLine(photos.Count());
                    for(int i = 0; i < photos.Count(); ++i)
                    {
                        var imageStream = System.Drawing.Bitmap.FromStream(photos[i].OpenReadStream());
                        var image = new Medium();
                        image.Id = Generator.GenerateId(20);
                        image.Type = "IMAGE";
                        var path = "Media/Picture/" + image.Id + ".png";
                        imageStream.Save(path);
                        image.Path = path;
                        context.Media.Add(image);

                        var postImage = new PostImage
                        {
                            ImageId = image.Id,
                            PostId = post.Id,
                            Position = "CENTER"
                        };
                        context.PostImages.Add(postImage);

                        post.PostImages.Add(postImage);
                    }
                    context.NeedPosts.Add(post);
                }
                context.SaveChanges();
                msg["status"] = "success";
            }
            catch(Exception e)
            {
                msg["status"] = "fail";
                Console.WriteLine(e.ToString());
            }
            return JsonConvert.SerializeObject(msg);
        }

        // PUT api/<PostController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PostController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
