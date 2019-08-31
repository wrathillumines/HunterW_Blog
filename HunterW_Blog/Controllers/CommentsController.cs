﻿using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using HunterW_Blog.Models;
using Microsoft.AspNet.Identity;

namespace HunterW_Blog.Controllers
{
    [RequireHttps]
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Comments
        public ActionResult Index()
        {
            var comments = db.Comments.Include(c => c.Author).Include(c => c.BlogPostId);
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
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.blogpost = new SelectList(db.BlogPosts, "Id", "Title");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BlogPostId")] Comment comment, string CommentBody, string slug)
        {
            if (ModelState.IsValid)
            {
                comment.Body = CommentBody;
                comment.AuthorId = User.Identity.GetUserId();
                comment.Created = DateTimeOffset.Now;
                db.Comments.Add(comment);
                db.SaveChanges();
                //return RedirectToAction("index");
                return RedirectToAction("Details", "BlogPosts", new { slug = slug });
            }
            return View(comment);
        }

        // GET: Comments/Edit/5
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
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "FirstName", comment.AuthorId);
            ViewBag.BlogPostId = new SelectList(db.BlogPosts, "Id", "Title", comment.BlogPostId);
            return View(comment);
        }

        // POST: Comments/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Edit([Bind(Include = "Id,BlogPostId,AuthorId,Body,Created,Updated,UpdateReason")] Comment comment, string slug)
        {
            if (string.IsNullOrEmpty(comment.UpdateReason))
            {
                ModelState.AddModelError("UpdateReason", "Please provide a reason for editing this post.");
                comment.BlogPost = db.BlogPosts.AsNoTracking().FirstOrDefault(b => b.Id == comment.BlogPostId);
                return View(comment);
            }
            if (ModelState.IsValid)
            {
                comment.Updated = DateTimeOffset.Now;
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "BlogPosts", new { slug = slug });
            }
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "FirstName", comment.AuthorId);
            ViewBag.BlogPostId = new SelectList(db.BlogPosts, "Id", "Title", comment.BlogPostId);
            return View(comment);
        }

        // GET: Comments/Delete/5
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
        public ActionResult DeleteConfirmed(int id, string slug)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("Details", "BlogPosts", new { slug = slug });
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
