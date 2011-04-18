using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using N2.Collections;
using N2.Persistence.Finder;
using N2.Templates.Mvc.Areas.Blog.Models;
using N2.Templates.Mvc.Areas.Blog.Models.Pages;
using N2.Templates.Mvc.Controllers;
using N2.Web;
using N2.Web.Mvc;

namespace N2.Templates.Mvc.Areas.Blog.Controllers
{
    /// <summary>
    /// This controller returns a view that displays the item created via the management interface
    /// </summary>
    [Controls(typeof(Models.Pages.BlogPostContainer))]
    public class BlogPostContainerController : ContentController<Models.Pages.BlogPostContainer>
    {
        private IItemFinder finder;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogPostContainerController"/> class.
        /// </summary>
        /// <param name="finder">The finder.</param>
        public BlogPostContainerController(IItemFinder finder)
        {
            this.finder = finder;
        }

        /// <summary>
        /// The Index action.
        /// </summary>
        /// <returns>
        /// The Action Result.
        /// </returns>
        public override ActionResult Index()
        {
            var model = GetPosts(string.Empty, 1, CurrentItem.PostsPerPage);
            return View(model);
        }

        /// <summary>
        /// Pages to the specified page of blog posts.
        /// </summary>
        /// <param name="p">The page number.</param>
        /// <returns>The Action Result.</returns>
        public ActionResult Page(int? p)
        {
            // Default page to 1 if null or <= 0
            int page = p == null || p <= 0 ? 1 : p.Value;

            var model = GetPosts(string.Empty, page, CurrentItem.PostsPerPage);
            return View("Index", model);
        }

        /// <summary>
        /// Returns a collection of posts based on tag number.
        /// </summary>
        /// <param name="t">The tag value.</param>
        /// <param name="p">The page number.</param>
        /// <returns>Action Result.</returns>
        public ActionResult Tag(string t, int? p)
        {
            // If tag is empty redirect to main
            if (string.IsNullOrEmpty(t))
            {
                return RedirectToAction("Index");
            }

            // Default page to 1 if null or <= 0
            p = p == null || p <= 0 ? 1 : p.Value;

            var model = GetPosts(t, p.Value, CurrentItem.PostsPerPage);

            ViewData["Tag"] = t;
            return View("Index", model);
        }

        /// <summary>
        /// Gets the posts.
        /// </summary>
        /// <param name="tag">The tag value.</param>
        /// <param name="page">The page number.</param>
        /// <param name="postCount">The post count per page.</param>
        /// <returns>
        /// The return value.
        /// </returns>
        private BlogPostContainerModel GetPosts(string tag, int page, int postCount)
        {
            int skip = (page - 1) * postCount;
            int take = postCount;

            IList<BlogPost> posts;

            if (!string.IsNullOrEmpty(tag))
            {
                posts = finder.Where.Type.Eq(typeof(BlogPost))
                        .And.Parent.Eq(CurrentPage)
                        .And.Detail("Tags").Like("%" + tag + "%")
                        .FirstResult(skip)
                        .MaxResults(take + 1)
                        .OrderBy.Published.Desc
                        .Select<BlogPost>();
            }
            else
            {
                posts = finder.Where.Type.Eq(typeof(BlogPost))
                        .And.Parent.Eq(CurrentPage)
                        .FirstResult(skip)
                        .MaxResults(take + 1)
                        .OrderBy.Published.Desc
                        .Select<BlogPost>();
            }

            var model = new BlogPostContainerModel
            {
                Container = CurrentItem,
                Posts = posts,
                Page = page,
                Tag = tag,
                IsLast = posts.Count <= take,
                IsFirst = page == 1
            };
            if (!model.IsLast)
            {
                model.Posts.RemoveAt(model.Posts.Count - 1);
            }

            return model;
        }
    }
}