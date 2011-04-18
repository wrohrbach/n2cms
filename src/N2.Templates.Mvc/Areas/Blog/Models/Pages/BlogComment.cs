using System.Web.UI.WebControls;
using N2.Definitions;
using N2.Details;
using N2.Edit.Trash;
using N2.Integrity;
using N2.Templates.Mvc.Models.Pages;
using N2.Templates.Mvc.Services;
using N2.Web.Mvc;

namespace N2.Templates.Mvc.Areas.Blog.Models.Pages
{
    /// <summary>
    /// This is the Model for a blog comment.
    /// </summary>
    [Disable]
    [PageDefinition("Blog Post Comment", Description = "A Comment on a blog post.", SortOrder = 155,
        IconUrl = "~/Content/Img/comment.png")]
    [Throwable(AllowInTrash.No)]
    [Versionable(AllowVersions.No)]
    [RestrictParents(typeof(BlogPost))]
    public class BlogComment : BlogBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlogComment"/> class.
        /// </summary>
        public BlogComment()
        {
            Visible = false;
        }

        /// <summary>
        /// Gets or sets the comment ID.
        /// </summary>
        /// <value>
        /// The comment ID.
        /// </value>
        public virtual int CommentID
        {
            get { return GetDetail("CommentID", 1); }
            set { SetDetail("CommentID", value, 1); }
        }

        /// <summary>
        /// Gets or sets the name of the author.
        /// </summary>
        /// <value>
        /// The name of the author.
        /// </value>
        [EditableTextBox("AuthorName", 90, ContainerName = Tabs.Content)]
        public virtual string AuthorName { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email address.
        /// </value>
        [EditableTextBox("Email", 93, ContainerName = Tabs.Content)]
        public virtual string Email { get; set; }

        /// <summary>
        /// Gets or sets the author URL.
        /// </summary>
        /// <value>
        /// The author URL.
        /// </value>
        [EditableTextBox("AuthorUrl", 96, ContainerName = Tabs.Content)]
        public virtual string AuthorUrl { get; set; }
    }
}