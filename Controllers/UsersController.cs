using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebApplicationGF.Models;

namespace WebApplicationGF.Controllers
{
    public class UsersController : ApiController
    {
        private static int nextId = 20;

        // For now use a hard-coded static list for users
        private static Dictionary<int, User> users = new Dictionary<int, User>() {
            { 0,  new User { id = 0, name = "Sarah", points = 10 } },
            { 1,  new User { id = 1, name = "Jon", points =  15 } },
            { 2,  new User { id = 2, name = "Elle", points = 30 } },
            { 3,  new User { id = 3, name = "Jeremy", points = 45 } },
            { 4,  new User { id = 4, name = "Tamara", points = 60 } },
            { 5,  new User { id = 5, name = "Zane", points = 75 } },
            { 6,  new User { id = 6, name = "Angie", points = 90 } },
            { 7,  new User { id = 7, name = "Phil", points = 105 } },
            { 8,  new User { id = 8, name = "Milly", points = 120 } },
            { 9,  new User { id = 9, name = "Terrence", points = 135 } },
        };

        // GET api/users
        public Task<IHttpActionResult> Get()
        {
            return Task.FromResult<IHttpActionResult>(Ok(users.Values.ToArray()));
        }

        // GET api/users/{id}
        public Task<IHttpActionResult> Get(int id)
        {
            if (!users.ContainsKey(id)) {
                return Task.FromResult<IHttpActionResult>(NotFound());
            }
            return Task.FromResult<IHttpActionResult>(Ok(users[id]));
        }

        // POST api/users
        public Task<IHttpActionResult> Post([FromBody]User user)
        {
            if (user == null || string.IsNullOrEmpty(user.name)) {
                return Task.FromResult<IHttpActionResult>(BadRequest("The request body could not be parsed into a User object."));
            }

            User resultUser = users.Values.FirstOrDefault(u => String.Compare(u.name, user.name, true) == 0);
            if (null == resultUser) {
                user.id = nextId++;
                users.Add(user.id, user);
                resultUser = user;
            }
            return Task.FromResult<IHttpActionResult>(Ok(resultUser));
        }

        // PUT api/users/{id}
        public Task<IHttpActionResult> Put(int id, [FromBody]User user)
        {
            if (!users.ContainsKey(id)) {
                return Task.FromResult<IHttpActionResult>(NotFound());
            }
            users[id] = user;
            return Task.FromResult<IHttpActionResult>(Ok(user));
        }

        // DELETE api/users/{id}
        public Task<IHttpActionResult> Delete(int id)
        {
            if (users.ContainsKey(id)) {
                users.Remove(id);
            }
            return Task.FromResult<IHttpActionResult>(Ok());
        }

        [HttpPatch]
        public Task<IHttpActionResult> Patch(int id, [FromBody] JObject jo) {
            if (!users.ContainsKey(id)) {
                return Task.FromResult<IHttpActionResult>(NotFound());
            }

            // TODO (Chris): Hacky way to get a simple PATCH merge.
            // Depending on requirements a JSON Patch (rfc6902) or JSON Merge Patch (rfc7386) approach would be better.
            User user = users[id];

            if (jo.SelectToken("name") != null) {
                user.name = (string)jo.SelectToken("name");
            }

            if (jo.SelectToken("points") != null) {
                user.points = (int)jo.SelectToken("points");
            }

            users[id] = user;
            return Task.FromResult<IHttpActionResult>(Ok(user));
        }

        // TODO (Chris): This is an inadvisable design. We should instead follow RESTful API 
        // conventions and implement a proper PATCH method.
        //[Route("api/users/{id}/setPoints")]
        //[HttpPost]
        //public Task<IHttpActionResult> setPoints(int id, [FromBody] int points) {
        //    if (!users.ContainsKey(id)) {
        //        return Task.FromResult<IHttpActionResult>(NotFound());
        //    }
        //    User user = users[id];
        //    user.points = points;
        //    return Task.FromResult<IHttpActionResult>(Ok(user));
        //}
    }
}
