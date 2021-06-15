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
    public class CompositionsAPIController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/CompositionsAPI
        public IQueryable<Composition> GetCompositions()
        {
            return db.Compositions.AsNoTracking();
        }

        // GET: api/CompositionsAPI/5
        [ResponseType(typeof(Composition))]
        public IHttpActionResult GetComposition(int id)
        {
            Composition composition = db.Compositions.Find(id);
            if (composition == null)
            {
                return NotFound();
            }

            return Ok(composition);
        }

        // PUT: api/CompositionsAPI/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutComposition(int id, Composition composition)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != composition.ID)
            {
                return BadRequest();
            }

            db.Entry(composition).State = System.Data.Entity.EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompositionExists(id))
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

        // POST: api/CompositionsAPI
        [ResponseType(typeof(Composition))]
        public IHttpActionResult PostComposition(Composition composition)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Compositions.Add(composition);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = composition.ID }, composition);
        }

        // DELETE: api/CompositionsAPI/5
        [ResponseType(typeof(Composition))]
        public IHttpActionResult DeleteComposition(int id)
        {
            Composition composition = db.Compositions.Find(id);
            if (composition == null)
            {
                return NotFound();
            }

            db.Compositions.Remove(composition);
            db.SaveChanges();

            return Ok(composition);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CompositionExists(int id)
        {
            return db.Compositions.Count(e => e.ID == id) > 0;
        }
    }
}