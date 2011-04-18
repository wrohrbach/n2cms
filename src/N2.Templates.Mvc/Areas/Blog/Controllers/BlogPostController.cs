﻿using System;
using System.Web.Mvc;
using N2.Edit;
using N2.Templates.Mvc.Areas.Blog.Models;
using N2.Templates.Mvc.Areas.Blog.Models.Pages;
using N2.Templates.Mvc.Areas.Blog.Services;
using N2.Templates.Mvc.Controllers;
using N2.Web;

namespace N2.Templates.Mvc.Areas.Blog.Controllers
{
    /// <summary>
    /// Controller for blog posts.
    /// </summary>
    [Controls(typeof(BlogPost))]
    public class BlogPostController : TemplatesControllerBase<BlogPost>
    {
        /// <summary>
        /// The Index action.
        /// </summary>
        /// <returns>
        /// The Action Result.
        /// </returns>
        public override ActionResult Index()
        {
            return View(CurrentItem);
        }
        
        /// <summary>
        /// Blogs the comment input form.
        /// </summary>
        /// <returns>
        /// The return value.
        /// </returns>
        public ActionResult BlogCommentInputForm()
        {
            return View(new BlogCommentInputModel(CurrentItem));
        }

        /// <summary>
        /// Submits the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>The return value.</returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Submit(BlogCommentInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", CurrentItem);
            }

            BlogComment comment = Engine.Definitions.CreateInstance<BlogComment>(CurrentPage);
            comment.CommentID = ((BlogPost)CurrentPage).GetNextCommentID();
            comment.Title = Server.HtmlEncode(model.Title);
            comment.AuthorName = Server.HtmlEncode(model.Name);
            comment.Email = Server.HtmlEncode(model.Email);
            comment.AuthorUrl = Server.HtmlEncode(model.Url);
            comment.Text = model.Text.ToSafeHtml();

            Engine.Persister.Save(comment);

            return Redirect(CurrentPage.Url + "#c" + comment.CommentID);
        }
    }
}
