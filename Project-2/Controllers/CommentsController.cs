using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Project_2.Models;

namespace Project_2.Controllers
{
    [RequireHttps]
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Comments
        public ActionResult Index()
        {
            var comments = db.Comments.Include(c => c.Author).Include(c => c.BlogPost);
            return View(comments.ToList());
        }

        // GET: Comments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comments/Create

        public ActionResult Create()
        {
            //ViewBag.AuthorID = new SelectList(db.Users, "Id", "FirstName");
            //ViewBag.BlogPostId = new SelectList(db.Posts, "ID", "Title");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,BlogPostId,AuthorID,Created,CommentBody,Updated,UpdateReason")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.AuthorID = User.Identity.GetUserId();
                comment.Created = DateTime.Now;
                db.Comments.Add(comment);
                db.SaveChanges();
                var mySlug = db.Posts.Find(comment.BlogPostId).Slug;
                return RedirectToAction("DetailSlug", "BlogPosts", new { slug = mySlug}); 
            }

            //ViewBag.AuthorID = new SelectList(db.Users, "Id", "FirstName", comment.AuthorID);
            //ViewBag.BlogPostId = new SelectList(db.Posts, "ID", "Title", comment.BlogPostId);
            return View(comment);
        }

        // GET: Comments/Edit/5
        [Authorize(Roles = "Admin")]
        [Authorize(Roles = "Moderator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.AuthorID = new SelectList(db.Users, "Id", "FirstName", comment.AuthorID);
            ViewBag.BlogPostId = new SelectList(db.Posts, "ID", "Title", comment.BlogPostId);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,BlogPostId,AuthorID,Created,CommentBody,Updated,UpdateReason")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AuthorID = new SelectList(db.Users, "Id", "FirstName", comment.AuthorID);
            ViewBag.BlogPostId = new SelectList(db.Posts, "ID", "Title", comment.BlogPostId);
            return View(comment);
        }

        // GET: Comments/Delete/5
        [Authorize(Roles = "Admin")]
        [Authorize(Roles = "Moderator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
