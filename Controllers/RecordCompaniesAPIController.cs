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
    public class RecordCompaniesAPIController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/RecordCompaniesAPI
        public IQueryable<RecordCompany> GetRecordCompanies()
        {
            return db.RecordCompanies;
        }

        // GET: api/RecordCompaniesAPI/5
        [ResponseType(typeof(RecordCompany))]
        public IHttpActionResult GetRecordCompany(int id)
        {
            RecordCompany recordCompany = db.RecordCompanies.Find(id);
            if (recordCompany == null)
            {
                return NotFound();
            }

            return Ok(recordCompany);
        }

        // PUT: api/RecordCompaniesAPI/5
        [Authorize(Roles = "Admin")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRecordCompany(int id, RecordCompany recordCompany)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != recordCompany.ID)
            {
                return BadRequest();
            }

            db.Entry(recordCompany).State = System.Data.Entity.EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecordCompanyExists(id))
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

        // POST: api/RecordCompaniesAPI
        [Authorize(Roles = "Admin")]
        [ResponseType(typeof(RecordCompany))]
        public IHttpActionResult PostRecordCompany(RecordCompany recordCompany)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.RecordCompanies.Add(recordCompany);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = recordCompany.ID }, recordCompany);
        }

        // DELETE: api/RecordCompaniesAPI/5
        [Authorize(Roles = "Admin")]
        [ResponseType(typeof(RecordCompany))]
        public IHttpActionResult DeleteRecordCompany(int id)
        {
            RecordCompany recordCompany = db.RecordCompanies.Find(id);
            if (recordCompany == null)
            {
                return NotFound();
            }

            db.RecordCompanies.Remove(recordCompany);
            try
            {
                db.SaveChanges();
            }
            catch(DbUpdateException)
            {
                return BadRequest();
            }

            return Ok(recordCompany);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RecordCompanyExists(int id)
        {
            return db.RecordCompanies.Count(e => e.ID == id) > 0;
        }
    }
}