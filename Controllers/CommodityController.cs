using Mercury_Backend.Contexts;
using Mercury_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Mercury_Backend.Utils;
using Microsoft.AspNetCore.Http;
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
        //         msg["Code"] = "success";
        //     }
        //     catch(Exception e)
        //     {
        //         msg["Code"] = "fail";
        //     }
        //     return JsonConvert.SerializeObject(msg);
        // }

        // GET api/<CommodityController>/5
        [HttpGet]
        public string Get()
        {
            // var flag = 0;
            JObject msg = new JObject();
            var commodityList = new List<Commodity>();
            try
            {

                var judge = Request.Form["keyword"].ToString();
                
            }
            catch 
            {

                try
                {
                    var simplifiedList = new List<SimplifiedCommodity>();
                    var tmpList = context.Commodities.Where(s=>true).Include(commodity => commodity.CommodityTags)
                        .Include(commodity => commodity.Owner).ThenInclude(owner => owner.Avatar);
                    
                    commodityList = tmpList.ToList<Commodity>();
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
                
                    

                    msg["Code"] = "200";


                    msg["totalPage"] = commodityList.Count;
                }
                catch (Exception e1)
                {
                    Console.WriteLine(e1);

                    msg["Code"] = "403";
                    msg["Description"] = "Error occured when getting data from model.";

                }
                return JsonConvert.SerializeObject(msg);
            
            }

            
            if (Request.Form["keyword"].ToString() == "" != true)
            {
                var strKeyWord = Request.Form["keyword"].ToString();
                
                var tmpList = context.Commodities.Where(b => b.Name.Contains(strKeyWord)).Include(commodity => commodity.CommodityTags).Include(commodity => commodity.Owner).ThenInclude(owner => owner.Avatar).ToList<Commodity>();
                // entering searching by keyword.
                // var idList = tmpList.Select(s => new {s.Id});
                commodityList = tmpList;
                
                // flag = 1;

                // 
                // todo: 用关键词搜索
            }
            
            
            else if (Request.Form["ownerName"].ToString() == "" != true)
            {
                var ownerName = Request.Form["ownerName"];
                var strOwnerName = ownerName.ToString();
                
                // msg["commodityList"] = JToken.FromObject(commodityList);
                
                var usrs = context.SchoolUsers.Where(b => b.Nickname.Contains(strOwnerName)).ToList();
                var idList = usrs.Select(s => new {s.SchoolId}).ToList();
                
                for (int i = 0; i < idList.Count; i++)
                {
                    var tmpList = context.Commodities.Where(b => b.OwnerId== idList[i].SchoolId).Include(commodity => commodity.CommodityTags).Include(commodity => commodity.Owner).ThenInclude(owner => owner.Avatar).ToList<Commodity>();
                    commodityList = commodityList.Concat(tmpList).ToList<Commodity>();
                }
            }
            
            else if (Request.Form["userId"].ToString() == "" != true)
            {
                var ownerName = Request.Form["userId"];
                var strOwnerName = ownerName.ToString();
                // msg["commodityList"] = JToken.FromObject(commodityList);
                
                var usrs = context.SchoolUsers.Where(b => b.Nickname.Contains(strOwnerName)).ToList();
                var idList = new List<string>();
                idList.Add(ownerName);
                for (int i = 0; i < idList.Count; i++)
                {
                    var tmpList = context.Commodities.Where(b => b.OwnerId == idList[i]);
                    var tmpList1 = tmpList.Include(commodity => commodity.CommodityTags);
                    var tmpList2 = tmpList1.Include(commodity => commodity.Owner);
                    var tmpList3 = tmpList2.ThenInclude(owner => owner.Avatar).ToList<Commodity>();
                    
                    commodityList = commodityList.Concat(tmpList3).ToList<Commodity>();
                }
                
            }
            else if (Request.Form["tag"].ToString() == "" != true)
            {
                var strTagName = Request.Form["tag"].ToString();
                var tagList = context.CommodityTags.Where(b => b.Tag == strTagName);
                var idList = tagList.Select(s => new {s.CommodityId}).ToList();
                for (int i = 0; i < idList.Count; i++)
                {
                    var tmpList = context.Commodities.Where(b => idList[i].CommodityId == b.Id).Include(commodity => commodity.CommodityTags).Include(commodity => commodity.Owner).ThenInclude(owner => owner.Avatar).ToList<Commodity>();
                    commodityList = commodityList.Concat(tmpList).ToList<Commodity>();
                }
            }
            else if (Request.Form["id"].ToString() == "" != true)
            {
                
                try
                {
                    var strId = Request.Form["id"].ToString();
                    commodityList = context.Commodities.Where(s=>s.Id == strId).Include(commodity => commodity.CommodityTags).Include(commodity => commodity.Owner).ThenInclude(owner => owner.Avatar).ToList<Commodity>();;
                    if (commodityList.Count == 0)
                    {
                        msg["Code"] = "404";
                        msg["Description"] = "No such id";
                        return JsonConvert.SerializeObject(msg);
                    }
                    // commodityList.Add(targetComm);
                    var targetComm = commodityList[0];
                    commodityList[0].Clicks++;
                    if (Request.Form["userId"].ToString() == "" != true)
                    {
                        var vw = new View();
                        vw.Time = DateTime.Now;
                        vw.Commodity = targetComm;
                        vw.CommodityId = targetComm.Id;
                        vw.User = context.SchoolUsers.Find(Request.Form["userId"].ToString());
                        vw.UserId = vw.User.SchoolId;
                        
                        targetComm.Views.Add(vw);
                    }
                    try
                    {
                        context.SaveChanges();
                    }
                    catch
                    {
                        msg["Code"] = "403";
                        msg["Description"] = "Error occured when changing data from model.";

                    }
                }
                catch
                {
                    msg["Code"] = "403";
                    msg["Description"] = "Error occured when getting data from model.";
                }
            }
            else
            {

                msg["Code"] = "400";
                msg["Description"] = "You have not submitted any effective form data.";

                return JsonConvert.SerializeObject(msg);
            }
            try
            {
                var simplifiedList = new List<SimplifiedCommodity>();
                
                for (int i = 0; i < commodityList.Count; i++)
                {
                    try
                    {
                        simplifiedList.Add(Simplify.SimplifyCommodity(commodityList[i]));
                    }
                    catch
                    {
                        continue;
                    }

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
                
                    

                msg["Code"] = "200";
                msg["totalPage"] = commodityList.Count;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                msg["Code"] = "403";
                msg["Description"] = "Error occured when getting data from model.";

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
                if (files.Any())
                {
                    Console.WriteLine("No files uploaded.");
                }

                var flag = 0;
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
                        if (flag == 0)
                        {
                            newCommodity.Cover = path;
                            flag = 1;
                        }
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

                    if (flag == 0)
                    {
                        newCommodity.Cover = "Media/Image/Default.png";
                    }
                }
                context.Commodities.Add(newCommodity);
                // Console.WriteLine("haha")
                
                
                context.SaveChanges();
                

                msg["Code"] = "201";
            }
            catch (Exception e)
            {
                msg["Code"] = "403";
                msg["Description"] = "Internal error occured when posting";
                Console.WriteLine(e.ToString());
            }
            return JsonConvert.SerializeObject(msg);
        }

        // PUT api/<CommodityController>/5
        [HttpPut("{id}")]
        public string Put(string id)
        {
            JObject msg = new JObject();

            msg["Code"] = "200";

            try
            {
                var test = Request.Form["test"].ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                msg["Code"] = "400";
                msg["Description"] = "You have not submitted any form data.";

                
                return JsonConvert.SerializeObject(msg);
            }
            var commodityToChange = context.Commodities.Find(id);
            if (commodityToChange == null)
            {

                msg["Code"] = "404";
                msg["Description"] = "The ID does not exist in database.";

                 return JsonConvert.SerializeObject(msg);
            }
            var detailMsg = "Integrity constraint invoked by";
            
            
           
            
            if (Request.Form["id"].ToString() != "")
            {
                // commodityToChange.OwnerId = Request.Form["owner_id"].ToString();
                // context.SaveChanges();

                msg["Code"] = "403";

                detailMsg += " id ";
            }   
            if (Request.Form["owner_id"].ToString() != "")
            {
                detailMsg += " owner_id ";
                
            }

            if (Request.Form["video_id"].ToString() != "")
            {
                // commodityToChange.VideoId = Request.Form["video_id"].ToString();

                msg["Code"] = "403";

                detailMsg += " video_id ";
                
            }
            if (Request.Form["condition"].ToString() != "")
            {
                commodityToChange.Condition = Request.Form["condition"].ToString();
                detailMsg += " owner_id ";
                
            }

            if (Request.Form["price"].ToString() != "")
            {
                try
                {
                    commodityToChange.Price = Decimal.ToInt32(int.Parse(Request.Form["price"].ToString()));
                }
                catch 
                {

                    msg["Code"] = "403";

                    detailMsg += " price ";
                }
            }
            
            if (Request.Form["stock"].ToString() != "")
            {
                try
                {
                    commodityToChange.Stock = Decimal.ToByte(int.Parse(Request.Form["stock"].ToString()));
                }
                catch
                {

                    msg["Code"] = "403";

                    detailMsg += " stock ";
                }
            }
            
            if (Request.Form["forRent"].ToString() != "")
            {
                try
                {
                    commodityToChange.ForRent = Request.Form["forRent"].ToString() != "0";
                }
                catch 
                {

                    msg["Code"] = "403";

                    detailMsg += " forRent ";
                }
            }
            if (Request.Form["clicks"].ToString() != "")
            {
                try
                {
                    commodityToChange.Clicks = int.Parse(Request.Form["clicks"].ToString());
                }
                catch 
                {

                    msg["Code"] = "403";

                    detailMsg += " clicks ";
                }
            }
            
            if (Request.Form["likes"].ToString() != "")
            {
                try
                {
                    commodityToChange.Likes = int.Parse(Request.Form["likes"].ToString());
                }
                catch 
                {
                    msg["Code"] = "403";
                    detailMsg += " likes ";
                }
            }
            if (Request.Form["popularity"].ToString() != "")
            {
                try
                {
                    commodityToChange.Popularity = byte.Parse(Request.Form["popularity"].ToString());
                }
                catch 
                {

                    msg["Code"] = "403";


                    detailMsg += " popularity ";
                }
            }
            
            if (Request.Form["popularity"].ToString() != "")
            {
                try
                {
                    commodityToChange.Likes = int.Parse(Request.Form["popularity"].ToString());
                }
                catch 
                {

                    msg["Code"] = "403";

                    detailMsg += " popularity ";
                }
            }
            if (Request.Form["classification"].ToString() != "")
            {
                

                msg["Code"] = "403";

                detailMsg += " classification ";
                
            }
            if (Request.Form["unit"].ToString() != "")
            {
                try
                {
                    commodityToChange.Unit = Request.Form["unit"].ToString();
                }
                catch 
                {

                    msg["Code"] = "403";

                    detailMsg += " unit ";
                }
            }
            if (Request.Form["name"].ToString() != "")
            {
                try
                {
                    commodityToChange.Name = Request.Form["name"].ToString();
                }
                catch 
                {

                    msg["Code"] = "403";

                    detailMsg += " name ";
                }
            }


            if (msg["Code"].ToString() == "200")
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
                msg["Description"] = detailMsg;
                // detailMsg += ".";
            }

            // msg["detailMessage"] = detailMsg;
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

                    msg["Code"] = "404";
                    msg["Description"] = "No such id.";

                    return JsonConvert.SerializeObject(msg);
                }

                context.Commodities.Remove(commodityToDelete);
                context.SaveChanges();

                msg["Code"] = "200";

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                msg["Code"] = "403";
                msg["Description"] = "Error occured when putting data into model.";

            }
            return JsonConvert.SerializeObject(msg);
        }
    }
}
