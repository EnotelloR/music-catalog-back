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
    public class CompositorsAPIController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/CompositorsAPI
        public IQueryable<Compositor> GetCompositors()
        {
            return db.Compositors;
        }

        // GET: api/CompositorsAPI/5
        [ResponseType(typeof(Compositor))]
        public IHttpActionResult GetCompositor(int id)
        {
            Compositor compositor = db.Compositors.Find(id);
            if (compositor == null)
            {
                return NotFound();
            }

            return Ok(compositor);
        }

        // PUT: api/CompositorsAPI/5
        [Authorize(Roles = "Admin, MainAdmin")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCompositor(int id, Compositor compositor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != compositor.ID)
            {
                return BadRequest();
            }

            db.Entry(compositor).State = System.Data.Entity.EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompositorExists(id))
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

        // POST: api/CompositorsAPI
        [Authorize(Roles = "Admin, MainAdmin")]
        [ResponseType(typeof(Compositor))]
        public IHttpActionResult PostCompositor(Compositor compositor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Compositors.Add(compositor);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = compositor.ID }, compositor);
        }

        // DELETE: api/CompositorsAPI/5
        [Authorize(Roles = "Admin, MainAdmin")]
        [ResponseType(typeof(Compositor))]
        public IHttpActionResult DeleteCompositor(int id)
        {
            Compositor compositor = db.Compositors.Find(id);
            if (compositor == null)
            {
                return NotFound();
            }

            db.Compositors.Remove(compositor);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return BadRequest();
            }

            return Ok(compositor);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CompositorExists(int id)
        {
            return db.Compositors.Count(e => e.ID == id) > 0;
        }
    }
}