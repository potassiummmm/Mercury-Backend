using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Mercury_Backend.Contexts;
using Mercury_Backend.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;
using JWT.Algorithms;
using JWT.Serializers;
using JWT;
using JWT.Exceptions;
using Mercury_Backend.Utils;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mercury_Backend.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ModelContext context;
        private readonly IConfiguration config;
        public UserController(ModelContext modelContext, IConfiguration configuration)
        {
            context = modelContext;
            config = configuration;
        }
        // GET: api/<UserController>
        [HttpGet]
        public string Get()
        {
            JObject msg = new JObject();
            try
            {
                var list = context.SchoolUsers.OrderBy(b => b.SchoolId).ToList();
                msg["UserList"] = JToken.FromObject(list);
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.ToString());
                msg["Status"] = "500";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                msg["Status"] = "400";
            }
            return JsonConvert.SerializeObject(msg);
        }

        // GET api/<UserController>/5
        [HttpGet("{schoolId}")]
        public string Get(string schoolId)
        {
            JObject msg = new JObject();
            try
            {
                var user = context.SchoolUsers.Where(u => u.SchoolId == schoolId)
                    .Include(u => u.Avatar).Single();
                var userInformation = new UserInformation(user.SchoolId, user.Nickname,
                    user.RealName, user.Phone, user.Major, user.Credit, user.Role, user.Grade,
                    user.Brief, user.Avatar.Path);
                msg["User"] = JToken.FromObject(userInformation);

            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.ToString());
                msg["Status"] = "500";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                msg["Status"] = "400";
            }
            return JsonConvert.SerializeObject(msg);
        }

        public class RegisterRequest
        {
            public string SchoolId { get; set; }
            public string Nickname { get; set; }
            public string RealName { get; set; }
            public string Phone { get; set; }
            public string Password { get; set; }
        }
        
        // POST api/<UserController>/register
        [HttpPost]
        [Route("register")]
        public string Register(RegisterRequest request)
        {
            JObject msg = new JObject();
            if (context.SchoolUsers.Find(request.SchoolId) != null)
            {
                msg["Status"] = "403";
                msg["Description"] = "User already exists";
            }
            else
            {
                var newUser = new SchoolUser()
                {
                    SchoolId = request.SchoolId,
                    Nickname = request.Nickname,
                    RealName = request.RealName,
                    Phone = request.Phone,
                    Password = request.Password,
                    Brief = "该用户很懒，还没有写简介。",
                    Major = "SE",
                    Role = "Student",
                    Grade = 1,
                    AvatarId = "1",
                    Credit = 60
                };
                try
                {
                    context.SchoolUsers.Add(newUser);
                    context.SaveChanges();
                    msg["status"] = "success";
                    msg["information"] = "Registered successfully";
                }
                catch (DbUpdateException e)
                {
                    Console.WriteLine(e.ToString());
                    msg["Status"] = "403";
                    msg["Description"] = "Cannot update database";
                }
                catch (DBConcurrencyException e)
                {
                    Console.WriteLine(e.ToString());
                    msg["Status"] = "403";
                    msg["Description"] = "Fail to update database because of concurrent requests";
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    msg["Status"] = "400";
                }
            }
            return JsonConvert.SerializeObject(msg);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public string Put(string id,[FromForm] SchoolUser value)
        {
            JObject msg = new JObject();
            var user=context.SchoolUsers.Find(value.SchoolId);
            msg["Status"] = "Fail";
            if (user != null)
            {
                if (value.Nickname != null) user.Nickname = value.Nickname;
                if (value.RealName != null) user.RealName = value.RealName;
                if (value.Phone != null) user.Phone = value.Phone;
                if (value.Password != null) user.Password = value.Password; 
                if (value.Major != null) user.Major = value.Major;
                if (value.Credit != null) user.Credit = value.Credit;
                if (value.Role != null) user.Nickname = value.Nickname;
                if (value.Brief != null) user.Brief = value.Brief;
                context.SaveChanges();
                msg["Status"] = "200";
            }
            /*
            foreach(var p in value.GetType().GetProperties())
            {
                if (p.GetValue(value) != null && p.Name != "SchoolId")
                {
                    context.Entry(value).Property(p.Name).IsModified = true;
                }

            }
            try
            {
                context.SaveChanges();
            }
            catch (Exception e)
            {
                msg["status"] = "fail";
                return;
            }
            */
            return JsonConvert.SerializeObject(msg);

        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        //POST api/<UserController>/login
        [HttpPost("login")]
        public string Login([FromForm] string userId, [FromForm] string password)
        {
            JObject msg = new JObject();
            var user = context.SchoolUsers.Where(u => u.SchoolId == userId)
                .Include(u => u.Avatar).Single();
            if(user == null)
            {
                msg["Status"] = "404";
                msg["FailReason"] = "User doesn't exist";
            }
            else
            {
                if(user.Password == password)
                {
                    var provider = new UtcDateTimeProvider();
                    var now = provider.GetNow();
                    var secondsSinceEpoch = UnixEpoch.GetSecondsSince(now);

                    //add information to dictionary
                    var loginInformation = new Dictionary<string, object>
                    {
                        {"userId", userId},
                        {"password", password},
                        {"exp", secondsSinceEpoch + 300}
                    };

                    //encode
                    var secretKey = config["TokenKey"];
                    IJwtAlgorithm algorithm = new HMACSHA256Algorithm(); // symmetric
                    IJsonSerializer serializer = new JsonNetSerializer();
                    IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                    IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
                    var token = encoder.Encode(loginInformation, secretKey);

                    msg["Token"] = token;
                    msg["User"] = JToken.FromObject(Simplify.SimplifyUser(user));
                    msg["Status"] = "200";
                }
                else
                {
                    msg["Status"] = "403";
                    msg["FailReason"] = "Incorrect password";
                }
            }
            return JsonConvert.SerializeObject(msg);
        }

        // POST api/<UserController>/autoLogin
        [HttpPost("autoLogin")]
        public string AutoLogin([FromForm] string token)
        {
            JObject msg = new JObject();
            var secretKey = config["TokenKey"];
            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                var provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtAlgorithm algorithm = new HMACSHA256Algorithm(); // symmetric
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);

                var loginInformation = decoder.DecodeToObject<IDictionary<string, object>>(token, secretKey, verify: true);
                var user = context.SchoolUsers.Where(u => u.SchoolId == (string) loginInformation["userId"])
                    .Include(u => u.Avatar).Single();
                msg["User"] = JToken.FromObject(Simplify.SimplifyUser(user));
                msg["Status"] = "200";
            }
            catch (TokenExpiredException)
            {
                msg["Status"] = "403";
                msg["FailReason"] = "Token has expired";
            }
            catch (SignatureVerificationException)
            {
                msg["Status"] = "403";
                msg["FailReason"] = "Token has invalid signature";
            }
            return JsonConvert.SerializeObject(msg);
        }
    }
}