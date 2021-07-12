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
using System.Configuration;
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
        public String Get()
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
            //String Id, String NickName, String RealName, String Phone,
            //String Password, String Major, int Credit, String Role, int Grade,
            //String Brief = "", String AvatarId = ""
            JObject msg = new JObject();
            try
            {
                context.SchoolUsers.Add(NewUser);
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

        // POST api/<UserController>
        [HttpPost]
        [Route("register")]
        public String Register()
        {
            return "TBD";
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
            try
            {
                var user = context.SchoolUsers.Where(user => user.SchoolId == userId).ToList();
                if(user == null)
                {
                    msg["Status"] = "Fail";
                    msg["FailReason"] = "User doesn't exist";
                }
                if(user[0].Password == password)
                {
                    var provider = new UtcDateTimeProvider();
                    var now = provider.GetNow();
                    var secondsSinceEpoch = UnixEpoch.GetSecondsSince(now);

                    //add information to dictionary
                    var loginInformation = new Dictionary<string, object>();
                    loginInformation.Add("userId", userId);
                    loginInformation.Add("password", password);
                    loginInformation.Add("exp", secondsSinceEpoch + 300);

                    //encode
                    var secretKey = config["TokenKey"];
                    IJwtAlgorithm algorithm = new HMACSHA256Algorithm(); // symmetric
                    IJsonSerializer serializer = new JsonNetSerializer();
                    IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                    IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
                    var token = encoder.Encode(loginInformation, secretKey);

                    msg["Token"] = token;
                    msg["User"] = JToken.FromObject(user);
                    msg["Status"] = "Success";
                }
                else
                {
                    msg["Status"] = "Fail";
                    msg["FailReason"] = "Incorrect password";
                }
            }
            catch (Exception e)
            {
                msg["Status"] = "Fail";
                Console.WriteLine(e.ToString());
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
                var user = context.SchoolUsers.Where(user => user.SchoolId == (string)loginInformation["userId"]).ToList();
                msg["User"] = JToken.FromObject(user);
                msg["Status"] = "Success";
            }
            catch (TokenExpiredException)
            {
                msg["Status"] = "Fail";
                msg["FailReason"] = "Token has expired";
            }
            catch (SignatureVerificationException)
            {
                msg["Status"] = "Fail";
                msg["FailReason"] = "Token has invalid signature";
            }
            return JsonConvert.SerializeObject(msg);
        }
    }
}