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
using EntityState = System.Data.Entity.EntityState;

namespace WebApi2.Controllers
{
    public class PlaylistsAPIController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/PlaylistsAPI
        [Authorize(Roles = "Admin")]
        public IQueryable<Playlist> GetPlaylists()
        {
            return db.Playlists.AsNoTracking();
        }

        [Authorize]
        [HttpGet]
        [Route("api/GetMyPlaylist")]
        public IHttpActionResult GetMyPlaylist()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var stringID = identity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value);
            int id = int.Parse(stringID.FirstOrDefault());
            var playlist = db.Playlists.Where(p => p.UserID == id); 
            if (playlist == null)
            {
                return NotFound();
            }

            return Ok(playlist);
        }

        // GET: api/PlaylistsAPI/5
        [Authorize(Roles = "Admin")]
        [ResponseType(typeof(Playlist))]
        public IHttpActionResult GetPlaylist(int id)
        {
            Playlist playlist = db.Playlists.Find(id);
            if (playlist == null)
            {
                return NotFound();
            }

            return Ok(playlist);
        }

        // PUT: api/PlaylistsAPI/5
        [Authorize(Roles = "Admin")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPlaylist(int id, Playlist playlist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != playlist.ID)
            {
                return BadRequest();
            }

            db.Entry(playlist).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlaylistExists(id))
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

        // POST: api/PlaylistsAPI
        [Authorize]
        [ResponseType(typeof(Playlist))]
        public IHttpActionResult PostPlaylist(Playlist playlist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!db.Playlists.Any(pl => pl.CompositionID == playlist.CompositionID && pl.UserID == playlist.UserID))
            {
                db.Playlists.Add(playlist);
                db.SaveChanges();
                return CreatedAtRoute("DefaultApi", new { id = playlist.ID }, playlist);
            }
            else
            {
                return Conflict();
            }
        }

        // DELETE: api/PlaylistsAPI/5
        [Authorize]
        [ResponseType(typeof(Playlist))]
        public IHttpActionResult DeletePlaylist(int id)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var role = identity.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (role.First() != "Admin")
            {
                var stringID = identity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value);
                int intId = int.Parse(stringID.FirstOrDefault());
                Playlist playlist = db.Playlists.Where(p => p.UserID == intId && p.ID == id).First();
                if (playlist == null)
                {
                    return NotFound();
                }
                db.Playlists.Remove(playlist);
                db.SaveChanges();

                return Ok(playlist);
            }
            else {
                Playlist playlist = db.Playlists.Find(id);
                if (playlist == null)
                {
                   return NotFound();
                }
                db.Playlists.Remove(playlist);
                db.SaveChanges();

                return Ok(playlist);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PlaylistExists(int id)
        {
            return db.Playlists.Count(e => e.ID == id) > 0;
        }
    }
}