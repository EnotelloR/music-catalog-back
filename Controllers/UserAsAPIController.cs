using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi2.Models;

namespace WebApi2.Controllers
{
    public class UserAsAPIController : ApiController
    {
        private DataContext db = new DataContext();

        [Authorize(Roles = "Admin, MainAdmin")]
        // GET: api/UserAsAPI
        public IQueryable<UserA> GetUserAs()
        {
            return db.UserAs;
        }

        [Authorize(Roles = "MainAdmin")]
        [HttpGet]
        [Route("api/GetAdmins")]
        // GET: api/GetAdmins
        public IQueryable<UserA> GetAdmins()
        {
            return db.UserAs.Where(user => user.UserRole == "Admin");
        }

        [Authorize(Roles = "MainAdmin")]
        [HttpGet]
        [Route("api/GetNotAdmins")]
        // GET: api/GetNotAdmins
        public IQueryable<UserA> GetNotAdmins()
        {
            return db.UserAs.Where(user => user.UserRole == "User");
        }

        [Authorize(Roles = "Admin, MainAdmin")]
        // GET: api/UserAsAPI/5
        [ResponseType(typeof(UserA))]
        public IHttpActionResult GetUserA(int id)
        {
            UserA userA = db.UserAs.Find(id);
            if (userA == null)
            {
                return NotFound();
            }

            return Ok(userA);
        }

        [Authorize(Roles = "Admin, MainAdmin")]
        // PUT: api/UserAsAPI/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUserA(int id, UserA userA)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userA.ID)
            {
                return BadRequest();
            }

            db.Entry(userA).State = System.Data.Entity.EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserAExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPut]
        [Route("api/ChangePassword")]
        [Authorize]
        // PUT: api/ChangePassword/
        [ResponseType(typeof(void))]
        public IHttpActionResult ChangePassword(string oldPassword, string newPassword)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var stringID = identity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value);
            int id = int.Parse(stringID.FirstOrDefault());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UserA userA = db.UserAs.Find(id);

            if (userA == null)
            {
                return Unauthorized();
            }

            else
            {
                if (userA.UserPassword == oldPassword)
                {
                    userA.UserPassword = newPassword;
                }
                else
                {
                    return BadRequest("Wrong password");
                }
            }

            db.Entry(userA).State = System.Data.Entity.EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserAExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPut]
        [Route("api/AddAdmin")]
        [Authorize(Roles = "MainAdmin")]
        // PUT: api/AddAdmin/1
        [ResponseType(typeof(void))]
        public IHttpActionResult AddAdmin(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UserA userA = db.UserAs.Find(id);

            if (userA == null)
            {
                return NotFound();
            }

            else
            {
                userA.UserRole = "Admin";
            }

            db.Entry(userA).State = System.Data.Entity.EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserAExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/UserAsAPI
        [ResponseType(typeof(UserA))]
        public IHttpActionResult PostUserA(UserA userA)
        {
            userA.UserRole = "User";
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                db.UserAs.Add(userA);
                db.SaveChanges();
            }
            catch
            {
                return BadRequest();
            }

            return CreatedAtRoute("DefaultApi", new { id = userA.ID }, userA);
        }

        [Authorize(Roles = "Admin, MainAdmin")]
        // DELETE: api/UserAsAPI/5
        [ResponseType(typeof(UserA))]
        public IHttpActionResult DeleteUserA(int id)
        {
            UserA userA = db.UserAs.Find(id);
            if (userA == null)
            {
                return NotFound();
            }

            db.UserAs.Remove(userA);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return BadRequest();
            }

            return Ok(userA);
        }

        [HttpPut]
        [Route("api/DeleteAdmin")]
        [Authorize(Roles = "MainAdmin")]
        // DELETE: api/DeleteAdmin/5
        [ResponseType(typeof(UserA))]
        public IHttpActionResult DeleteAdmin(int id)
        {
            UserA userA = db.UserAs.Find(id);
            if (userA == null)
            {
                return NotFound();
            }
            userA.UserRole = "User";
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return BadRequest();
            }
            return Ok(userA);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserAExists(int id)
        {
            return db.UserAs.Count(e => e.ID == id) > 0;
        }
    }
}