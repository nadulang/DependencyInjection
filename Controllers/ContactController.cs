using System.Reflection.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ContactApi2.Models;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Http;
using Npgsql;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;

namespace ContactApi2.Controllers
{
    [ApiController]
    [Route("contact")]
    public class ContactController : ControllerBase
    {
        // private static List<UserContact> Contacts = new List<UserContact>()
        // {
        //     new UserContact{Id=1, Username="darwinwatterson", Passkey="sassywizard", Email="darwin.watterson@elmoreplus.com", Full_name="Darwin Watterson"},
        //     new UserContact{Id=2, Username="darwinwatterson", Passkey="sassywizard", Email="darwin.watterson@elmoreplus.com", Full_name="Darwin Watterson"},
        //     new UserContact{Id=3, Username="darwinwatterson", Passkey="sassywizard", Email="darwin.watterson@elmoreplus.com", Full_name="Darwin Watterson"},
        //     new UserContact{Id=4, Username="darwinwatterson", Passkey="sassywizard", Email="darwin.watterson@elmoreplus.com", Full_name="Darwin Watterson"},
        //     new UserContact{Id=5, Username="darwinwatterson", Passkey="sassywizard", Email="darwin.watterson@elmoreplus.com", Full_name="Darwin Watterson"}

        // };

        private readonly IDatabase _database;


        public ContactController(IDatabase database)
        {
           _database = database;
        }

        [HttpGet]
        public IActionResult Get()
        {
               var result = _database.Read();
               return Ok(result); 
        }

           

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _database.GetById(id);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Post(UserContact user)
        {
            var result = _database.Create(user);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
             var result = _database.Delete(id);
            return Ok(result);
        }

        [HttpPatch("{id}")]
        public IActionResult Patch( [FromBody]JsonPatchDocument<UserContact> user, int id)
        {
            var result = _database.Update(user, id);
            return Ok(result);
        }
        


    }


}

