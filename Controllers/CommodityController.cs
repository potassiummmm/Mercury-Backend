using Mercury_Backend.Contexts;
using Mercury_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;

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
using Microsoft.EntityFrameworkCore;


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
        // [HttpGet]
        // public string Get()
        // {
        //     JObject msg = new JObject();
        //     try
        //     {
        //         var commodityList = context.Commodities.OrderBy(b => b.Id).ToList<Commodity>();
        //         msg["commodityList"] = JToken.FromObject(commodityList);
        //         msg["status"] = "success";
        //     }
        //     catch(Exception e)
        //     {
        //         msg["status"] = "fail";
        //     }
        //     return JsonConvert.SerializeObject(msg);
        // }

        // GET api/<CommodityController>/5
        [HttpGet]
        public string Get()
        {
            var flag = 0;
            JObject msg = new JObject();
            var commodityList = new List<Commodity>();
            try
            {

                var judge = Request.Form["keyword"].ToString();

            }
            catch (Exception e)
            {

                msg["status"] = "fail";
                return JsonConvert.SerializeObject(msg);
            }

            
            if (Request.Form["keyword"].ToString() == "" != true)
            {
                var strKeyWord = Request.Form["keyword"].ToString();
                
                var tmpList = context.Commodities.Where(b => b.Name.Contains(strKeyWord)).Include(commodity => commodity.Owner).ThenInclude(owner => owner.Avatar).ToList<Commodity>();
                // entering searching by keyword.
                var idList = tmpList.Select(s => new {s.Id});
                commodityList = tmpList;
                
                flag = 1;
                try
                {
                    msg["status"] = "success";
                }
                catch(Exception e)
                {
                    msg["status"] = "fail";
                }
                // 
                // todo: 用关键词搜索
            }
            
            
            else if (Request.Form["owner_name"].ToString() == "" != true)
            {
                var ownerName = Request.Form["owner_name"];
                var strOwnerName = ownerName.ToString();
                
                // msg["commodityList"] = JToken.FromObject(commodityList);
                
                var usrs = context.SchoolUsers.Where(b => b.Nickname.Contains(strOwnerName)).ToList();
                var idList = usrs.Select(s => new {s.SchoolId}).ToList();
                
                for (int i = 0; i < idList.Count; i++)
                {
                    var tmpList = context.Commodities.Where(b => b.OwnerId== idList[i].SchoolId).Include(commodity => commodity.Owner).ThenInclude(owner => owner.Avatar).ToList<Commodity>();
                    commodityList = commodityList.Concat(tmpList).ToList<Commodity>();
                }
            }
            

            else if (Request.Form["tag"].ToString() == "" != true)
            {
                var strTagName = Request.Form["tag"].ToString();
                var tagList = context.CommodityTags.Where(b => b.Tag == strTagName);
                var idList = tagList.Select(s => new {s.CommodityId}).ToList();
                for (int i = 0; i < idList.Count; i++)
                {
                    var tmpList = context.Commodities.Where(b => idList[i].CommodityId == b.Id).Include(commodity => commodity.Owner).ThenInclude(owner => owner.Avatar).ToList<Commodity>();
                    commodityList = commodityList.Concat(tmpList).ToList<Commodity>();
                }
            }

            else
            {
                msg["status"] = "fail";
                return JsonConvert.SerializeObject(msg);
            }
            try
            {
                var simplifiedList = new List<SimplifiedCommodity>();
                
                for (int i = 0; i < commodityList.Count; i++)
                {
                    
                    simplifiedList.Add(Simplify.SimplifyCommodity(commodityList[i]));
                }
                msg["commodityList"] = JToken.FromObject(simplifiedList, new JsonSerializer()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore //忽略循环引用，默认是throw exception
                });
                var idList = commodityList.Select(s => s.Id).ToList();
                var tags = new List<CommodityTag>();
                for (int i = 0; i < idList.Count; i++)
                {
                    var tmpTag = context.CommodityTags.Where(tag => tag.CommodityId == idList[i])
                        .ToList();

                    tags = tags.Concat(tmpTag).ToList();
                    
                }

                var tagSet = tags.Select(s => s.Tag).ToList();
                tagSet = tagSet.Distinct().ToList();
                msg["tags"] = JToken.FromObject(tagSet);
                
                    
                msg["status"] = "success";
                msg["totalPage"] = commodityList.Count;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                msg["Status"] = "fail";
            }
            return JsonConvert.SerializeObject(msg);
        }
        
        

        // POST api/<CommodityController>
        [HttpPost]
        public async Task<string> Post([FromForm]Commodity newCommodity, [FromForm] List<IFormFile> files)
        {
            JObject msg = new JObject();
            var id = Generator.GenerateId(12);
            newCommodity.Id = id;
            var pathList = new List<string>();
            try
            {
                Console.WriteLine(files.Count());
                if (files.Count() == 0)
                {
                    Console.WriteLine("No files uploaded.");
                }
                for (int i = 0; i < files.Count(); i++)
                {
                    var tmpVideoId = Generator.GenerateId(20);
                    var splitFileName = files[i].FileName.Split('.');
                    var len = splitFileName.Length;
                    var postFix = splitFileName[len - 1];
                    var path = "";
                    if (postFix == "jpg" || postFix == "jpeg" || postFix == "gif" || postFix == "png")
                    {
                        path = "Media" + "/Image/" + tmpVideoId + '.' + postFix;
                        if (Directory.Exists(path))
                        {
                            Console.WriteLine("This path exists.");
                        }
                        else
                        {
                            Directory.CreateDirectory("Media");
                            Directory.CreateDirectory("Media/Image");
                        }

                        var med = new Medium();
                        med.Id = tmpVideoId;
                        med.Type = "Image";
                        context.Media.Add(med);
                        var comImg = new CommodityImage
                        {
                            Commodity = newCommodity,
                            CommodityId = newCommodity.Id,
                            Image = med,
                            ImageId = med.Id
                        };
                        
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await files[i].CopyToAsync(stream);
                        }
                        // imageStream.Save(path);
                        newCommodity.CommodityImages.Add(comImg);
                    }
                    else if (postFix == "mov" || postFix == "mp4" || postFix == "wmv" || postFix == "rmvb" || postFix == "3gp")
                    {
                        path = "Media" + "/Video/" + tmpVideoId + '.' + postFix;
                        Console.WriteLine(path);
                        pathList.Add(path);
                        
                        if (Directory.Exists(path))
                        {
                            Console.WriteLine("This path exists.");
                        }
                        else
                        {
                            Directory.CreateDirectory("Media");
                            Directory.CreateDirectory("Media/Video");
                        }
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await files[i].CopyToAsync(stream);
                        }

                        var video = new Medium();
                        video.Id = tmpVideoId;
                        video.Type = "Video";
                        video.Path = path;
                        context.Media.Add(video);
                        newCommodity.VideoId = tmpVideoId;
                    }
                    else
                    {
                        Console.WriteLine("Not a media file.");
                    }
                }
                context.Commodities.Add(newCommodity);
                // Console.WriteLine("haha");
                context.SaveChanges();
                msg["Status"] = "success";
            }
            catch (Exception e)
            {
                msg["Status"] = "fail";
                Console.WriteLine(e.ToString());
            }
            return JsonConvert.SerializeObject(msg);
        }

        // PUT api/<CommodityController>/5
        [HttpPut("{id}")]
        public string Put(string id)
        {
            JObject msg = new JObject();
            msg["Status"] = "success";
            try
            {
                var test = Request.Form["test"].ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                msg["Status"] = "fail";
                msg["Detail"] = "No form data";
                return JsonConvert.SerializeObject(msg);
            }
            var commodityToChange = context.Commodities.Find(id);
            if (commodityToChange == null)
            {
                msg["Status"] = "fail";
                msg["Detail"] = "No such Id.";
                 return JsonConvert.SerializeObject(msg);
            }
            var detailMsg = "Integrity constraint invoked by";
            
            
           
            
            if (Request.Form["id"].ToString() != "" == true)
            {
                // commodityToChange.OwnerId = Request.Form["owner_id"].ToString();
                // context.SaveChanges();
                msg["Status"] = "fail";
                detailMsg += " id ";
            }   
            if (Request.Form["owner_id"].ToString() != "" == true)
            {
                detailMsg += " owner_id ";
                
            }

            if (Request.Form["video_id"].ToString() != "" == true)
            {
                // commodityToChange.VideoId = Request.Form["video_id"].ToString();
                msg["Status"] = "fail";
                detailMsg += " video_id ";
                
            }
            if (Request.Form["condition"].ToString() != "" == true)
            {
                commodityToChange.Condition = Request.Form["condition"].ToString();
                detailMsg += " owner_id ";
                
            }

            if (Request.Form["price"].ToString() != "" == true)
            {
                try
                {
                    commodityToChange.Price = Decimal.ToInt32(int.Parse(Request.Form["price"].ToString()));
                }
                catch (Exception e)
                {
                    msg["Status"] = "fail";
                    detailMsg += " price ";
                }
            }
            
            if (Request.Form["stock"].ToString() != "" == true)
            {
                try
                {
                    commodityToChange.Stock = Decimal.ToByte(int.Parse(Request.Form["stock"].ToString()));
                }
                catch (Exception e)
                {
                    msg["Status"] = "fail";
                    detailMsg += " stock ";
                }
            }
            
            if (Request.Form["forRent"].ToString() != "" == true)
            {
                try
                {
                    commodityToChange.ForRent = Request.Form["forRent"].ToString() != "0";
                }
                catch (Exception e)
                {
                    msg["Status"] = "fail";
                    detailMsg += " forRent ";
                }
            }
            if (Request.Form["clicks"].ToString() != "" == true)
            {
                try
                {
                    commodityToChange.Clicks = int.Parse(Request.Form["clicks"].ToString());
                }
                catch (Exception e)
                {
                    msg["Status"] = "fail";
                    detailMsg += " clicks ";
                }
            }
            
            if (Request.Form["likes"].ToString() != "" == true)
            {
                try
                {
                    commodityToChange.Likes = int.Parse(Request.Form["likes"].ToString());
                }
                catch (Exception e)
                {
                    msg["Status"] = "fail";
                    detailMsg += " likes ";
                }
            }
            if (Request.Form["popularity"].ToString() != "" == true)
            {
                try
                {
                    commodityToChange.Popularity = byte.Parse(Request.Form["popularity"].ToString());
                }
                catch (Exception e)
                {
                    msg["Status"] = "fail";
                    detailMsg += " popularity ";
                }
            }
            
            if (Request.Form["popularity"].ToString() != "" == true)
            {
                try
                {
                    commodityToChange.Likes = int.Parse(Request.Form["popularity"].ToString());
                }
                catch (Exception e)
                {
                    msg["Status"] = "fail";
                    detailMsg += " popularity ";
                }
            }
            if (Request.Form["classification"].ToString() != "" == true)
            {
                
                msg["Status"] = "fail";
                detailMsg += " classification ";
                
            }
            if (Request.Form["unit"].ToString() != "" == true)
            {
                try
                {
                    commodityToChange.Unit = Request.Form["unit"].ToString();
                }
                catch (Exception e)
                {
                    msg["Status"] = "fail";
                    detailMsg += " unit ";
                }
            }
            if (Request.Form["name"].ToString() != "" == true)
            {
                try
                {
                    commodityToChange.Name = Request.Form["name"].ToString();
                }
                catch (Exception e)
                {
                    msg["Status"] = "fail";
                    detailMsg += " name ";
                }
            }

            if (msg["Status"].ToString() == "success")
            {
                context.SaveChanges();
                msg["changedCommodity"] = JToken.FromObject(commodityToChange);
            }

            if (detailMsg == "Integrity constraint invoked by")
            {
                detailMsg += " nothing.";
            }
            else
            {
                detailMsg += ".";
            }

            msg["detailMessage"] = detailMsg;
            return JsonConvert.SerializeObject(msg);
        }

        // DELETE api/<CommodityController>/5
        [HttpDelete("{id}")]
        public string Delete(string id)
        {
            JObject msg = new JObject();
            try
            {

                var commodityToDelete = context.Commodities.Find(id);
                if (commodityToDelete == null)
                {
                    msg["Status"] = "fail";
                    msg["detailMessage"] = "No such id.";
                    return JsonConvert.SerializeObject(msg);
                }

                context.Commodities.Remove(commodityToDelete);
                context.SaveChanges();
                msg["Status"] = "success";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                msg["Status"] = "fail";
            }
            return JsonConvert.SerializeObject(msg);
        }
    }
}
