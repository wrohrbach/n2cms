using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using N2.Templates.Mvc.Areas.Blog.Models.Pages;
using N2.Web.Mvc.Html;

namespace N2.Templates.Mvc.Areas.Blog.Models
{
    /// <summary>
    /// The blog post container model.
    /// </summary>
    public class BlogPostContainerModel
    {
        /// <summary>
        /// Gets or sets the container.
        /// </summary>
        /// <value>
        /// The container.
        /// </value>
        public BlogPostContainer Container { get; set; }

        /// <summary>
        /// Gets or sets the posts.
        /// </summary>
        /// <value>
        /// The blog posts.
        /// </value>
        public IList<BlogPost> Posts { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is last.
        /// </summary>
        /// <value>
        ///   <c>True</c> if this instance is last; otherwise, <c>false</c>.
        /// </value>
        public bool IsLast { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is first.
        /// </summary>
        /// <value>
        ///   <c>True</c> if this instance is first; otherwise, <c>false</c>.
        /// </value>
        public bool IsFirst { get; set; }

        /// <summary>
        /// Gets or sets the page.
        /// </summary>
        /// <value>
        /// The page number.
        /// </value>
        public int Page { get; set; }

        /// <summary>
        /// Gets or sets the tag.
        /// </summary>
        /// <value>
        /// The tag values.
        /// </value>
        public string Tag { get; set; }

        /// <summary>
        /// Gets or sets the posts per page.
        /// </summary>
        /// <value>
        /// The posts per page.
        /// </value>
        public int PostsPerPage { get; set; }
    }
}