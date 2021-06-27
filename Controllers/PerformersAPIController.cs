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
    public class PerformersAPIController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/PerformersAPI
        public IQueryable<Performer> GetPerformers()
        {
            return db.Performers;
        }

        // GET: api/PerformersAPI/5
        [ResponseType(typeof(Performer))]
        public IHttpActionResult GetPerformer(int id)
        {
            Performer performer = db.Performers.Find(id);
            if (performer == null)
            {
                return NotFound();
            }

            return Ok(performer);
        }

        // PUT: api/PerformersAPI/5
        [Authorize(Roles = "Admin")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPerformer(int id, Performer performer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (performer == null)
            {
                return BadRequest();
            }

            if (id != performer.ID)
            {
                return BadRequest();
            }

            db.Entry(performer).State = System.Data.Entity.EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PerformerExists(id))
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

        // POST: api/PerformersAPI
        [Authorize(Roles = "Admin")]
        [ResponseType(typeof(Performer))]
        public IHttpActionResult PostPerformer(Performer performer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Performers.Add(performer);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = performer.ID }, performer);
        }

        // DELETE: api/PerformersAPI/5
        [Authorize(Roles = "Admin")]
        [ResponseType(typeof(Performer))]
        public IHttpActionResult DeletePerformer(int id)
        {
            Performer performer = db.Performers.Find(id);
            if (performer == null)
            {
                return NotFound();
            }

            db.Performers.Remove(performer);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return BadRequest();
            }

            return Ok(performer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PerformerExists(int id)
        {
            return db.Performers.Count(e => e.ID == id) > 0;
        }
    }
}