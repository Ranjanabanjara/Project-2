//namespaces
using Project_2.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using Project_2.helpers;
using System.Diagnostics;

namespace Project_2.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        //represents entire index action
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index(int? page, string searchStr)//show all publishe and unpublished lists
        {

            ViewBag.Search = searchStr;
            var blogList = IndexSearch(searchStr);
            int pageSize = 3; // display three blog posts at a time on this page
            int pageNumber = (page ?? 1);
            var BlogPosts = db.Posts.Where(b => b.Published).AsQueryable();
            return View(blogList.OrderByDescending(b => b.Created).ToPagedList(pageNumber, pageSize));

        }
        public IQueryable<BlogPost> IndexSearch(string searchStr)
        {
            IQueryable<BlogPost> result = null;
            if (searchStr != null)
            {
                result = db.Posts.AsQueryable();
                result = result.Where(p => p.Title.Contains(searchStr) ||
                p.BlogPostBody.Contains(searchStr) ||
                p.Comments.Any(c => c.CommentBody.Contains(searchStr) ||
                c.Author.FirstName.Contains(searchStr) ||
                c.Author.LastName.Contains(searchStr) ||
                c.Author.DisplayName.Contains(searchStr) ||
                c.Author.Email.Contains(searchStr)));
            }
            else
            {
                result = db.Posts.AsQueryable();
            }
            return result.OrderByDescending(p => p.Published);
        }

    
        
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            EmailModel model = new EmailModel();
            return View(model);
        }
        //Adding a new ActionResult in a Home Controller
        // we need to call the service from our Controller.We will format a user’s information (input from the
        //view) and pass it to the service for sending to our email.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(EmailModel email)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await EmailHelper.ComposeEmailAsync(email);
                     return RedirectToAction("Index", "Home");
                }

                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    await Task.FromResult(0);
                }

            }
                return View(email);


        }
    }
}