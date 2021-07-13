using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
            var list = context.SchoolUsers.OrderBy(b => b.SchoolId).ToList<SchoolUser>();
            msg["UserList"] = JToken.FromObject(list);
            return JsonConvert.SerializeObject(msg);
        }

        // GET api/<UserController>/5
        [HttpGet("{schoolId}")]
        public string Get(string schoolId)
        {
            JObject msg = new JObject();

            try
            {
                var user = context.SchoolUsers.Find(schoolId);
                msg["User"] = JToken.FromObject(user);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                msg["Status"] = "Fail";
            }
            return JsonConvert.SerializeObject(msg);
        }

        // POST api/<UserController>
        [HttpPost]
        public String Post([FromForm] SchoolUser NewUser)

        {
            JObject msg = new JObject();
            try
            {
                context.SchoolUsers.Add(NewUser);
                Console.WriteLine("haha");
                context.SaveChanges();
                msg["status"] = "success";
            }
            catch (Exception e)
            {
                msg["status"] = "fail";
                Console.WriteLine(e.ToString());
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
                msg["status"] = "fail";
                msg["information"] = "User already exists.";
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
                catch (Exception e)
                {
                    msg["status"] = "fail";
                    msg["information"] = "Fail to modify database";
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
                if (value.Role != null) user.Role = value.Role;
                if (value.Brief != null) user.Brief = value.Brief;
                context.SaveChanges();
                msg["Status"] = "Success";
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
            var user = context.SchoolUsers.Find(userId);
            if(user == null)
            {
                msg["status"] = "fail";
                msg["failReason"] = "User doesn't exist";
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

                    msg["_token"] = token;
                    msg["user"] = JToken.FromObject(user);
                    msg["status"] = "success";
                }
                else
                {
                    msg["status"] = "fail";
                    msg["failReason"] = "Incorrect password";
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
                var user = context.SchoolUsers.Where(u => u.SchoolId == (string)loginInformation["userId"]).ToList();
                msg["user"] = JToken.FromObject(user);
                msg["status"] = "success";
            }
            catch (TokenExpiredException)
            {
                msg["status"] = "fail";
                msg["failReason"] = "Token has expired";
            }
            catch (SignatureVerificationException)
            {
                msg["status"] = "fail";
                msg["failReason"] = "Token has invalid signature";
            }
            return JsonConvert.SerializeObject(msg);
        }
    }
}