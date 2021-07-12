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
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mercury_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly ModelContext context;
        private static Random random;
        public PostController(ModelContext modelContext)
        {
            context = modelContext;
            random = new Random();
        }
        // GET: api/<PostController>
        [HttpGet]
        public string Get([FromForm] string userId, [FromForm] int maxNumber=10, [FromForm] int pageNumber=1)
        {
            JObject msg = new JObject();
            try
            {
                List<NeedPost> postList = null;
                if(userId != null)
                {
                    postList = context.NeedPosts.Include(post => post.Sender).ThenInclude(sender => sender.Avatar).Where(post => post.SenderId == userId).OrderBy(post => post.Time).ToList();
                }
                else
                {
                    postList = context.NeedPosts.Include(post => post.Sender).ThenInclude(sender => sender.Avatar).OrderBy(post => post.Time).ToList();
                }
                List<SimplifiedPost> result = new List<SimplifiedPost>();
                for (int i = 0; i < maxNumber && i + (pageNumber - 1) * maxNumber < postList.Count(); ++i)
                {
                    result.Add(Simplify.SimplfyPost(postList[i + (pageNumber - 1) * maxNumber]));
                }

                msg["PostList"] = JToken.FromObject(result);
                msg["Status"] = "Success";
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                msg["Status"] = "Fail";
            }
            return JsonConvert.SerializeObject(msg);
        }

        // GET api/<PostController>/5
        [HttpGet("{postId}")]
        public string Get(string postId)
        {
            JObject msg = new JObject();
            try
            {
                var post = context.NeedPosts.Include(post => post.PostComments).Include(post => post.PostImages).Single(post => post.Id == postId);
                var imageList = new List<string>();
                for (int i = 0; i < post.PostImages.Count(); ++i)
                {
                    var image = context.Media.Where(image => image.Id == post.PostImages.ElementAt(i).ImageId).ToList();
                    imageList.Add(image[0].Path);
                }
                msg["ImagePaths"] = JToken.FromObject(imageList);
                msg["Post"] = JToken.FromObject(post, new JsonSerializer()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                //msg["Post"] = JToken.FromObject(post);
                msg["Status"] = "Success";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                msg["Status"] = "Fail";
            }
            return JsonConvert.SerializeObject(msg);
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
                    for (int i = 0; i < photos.Count(); ++i)
                    {
                        var imageStream = System.Drawing.Bitmap.FromStream(photos[i].OpenReadStream());
                        var image = new Medium();
                        image.Id = Generator.GenerateId(20);
                        image.Type = "IMAGE";
                        var path = "Media/Image/" + image.Id + ".png";
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
                }
                post.Time = DateTime.Now;
                context.NeedPosts.Add(post);
                context.SaveChanges();
                msg["Status"] = "Success";
            }
            catch(Exception e)
            {
                msg["Status"] = "Fail";
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
        [HttpDelete("{postId}")]
        public string Delete(string postId)
        {
            JObject msg = new JObject();
            try
            {
                //var commentList = context.PostComments.Where(comment => comment.PostId == postId).ToList();
                //for(int i = 0; i < commentList.Count(); ++i)
                //{
                //    context.PostComments.Remove(commentList[i]);
                //    Console.WriteLine(commentList.Count());
                //}
                //Console.WriteLine("Delete all comments");
                var post = context.NeedPosts.Where(post => post.Id == postId).ToList();
                context.NeedPosts.Remove(post[0]);
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

        // GET api/post/<postId>/comment
        [HttpGet("{postId}/comment")]
        public string PostComment(string postId)
        {
            JObject msg = new JObject();
            try
            {
                var commentList = context.PostComments.Where(comment => comment.PostId == postId).ToList<PostComment>();
                msg["CommentList"] = JToken.FromObject(commentList);
                msg["Status"] = "Success";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                msg["Status"] = "Fail";
            }
            return JsonConvert.SerializeObject(msg);
        }

        // POST api/post/<postId>/comment
        [HttpPost("{postId}/comment")]
        public string PostComment(string postId, [FromForm] PostComment comment)
        {
            JObject msg = new JObject();
            try
            {
                comment.Id = Generator.GenerateId(15);
                comment.Time = DateTime.Now;
                comment.PostId = postId;
                context.PostComments.Add(comment);
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

        // DELETE api/post/<postId>/comment/<commentId>
        [HttpDelete("{postId}/comment/{commentId}")]
        public string DeleteComment(string postId, string commentId)
        {
            JObject msg = new JObject();
            try
            {
                var comment = context.PostComments.Where(comment => comment.Id == commentId).ToList<PostComment>();
                context.PostComments.Remove(comment[0]);
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
