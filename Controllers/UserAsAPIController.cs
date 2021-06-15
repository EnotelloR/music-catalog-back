using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi2.Models;

namespace WebApi2.Controllers
{
    public class UserAsAPIController : ApiController
    {
        private DataContext db = new DataContext();
        [Authorize(Roles = "Admin")]
        // GET: api/UserAsAPI
        public IQueryable<UserA> GetUserAs()
        {
            return db.UserAs;
        }
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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

        // POST: api/UserAsAPI
        [ResponseType(typeof(UserA))]
        public IHttpActionResult PostUserA(UserA userA)
        {
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
        [Authorize(Roles = "Admin")]
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
            db.SaveChanges();

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