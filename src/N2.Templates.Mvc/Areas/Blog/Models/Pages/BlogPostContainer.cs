using System.Collections.Generic;
using System.Linq;
using N2.Collections;
using N2.Details;
using N2.Integrity;
using N2.Templates.Mvc.Models.Pages;
using N2.Web.Mvc;
using N2.Web.UI;
using N2.Definitions;

namespace N2.Templates.Mvc.Areas.Blog.Models.Pages
{
    /// <summary>
    /// This is the model for the blog post container.
    /// </summary>
    [PageDefinition("Blog",
        Description = "A blog. Blog posts can be added to this page.",
        SortOrder = 150,
        IconUrl = "~/N2/Resources/icons/user_comment.png")]
    [RestrictParents(typeof(IStructuralPage))]
    [SortChildren(SortBy.PublishedDescending)]
    [TabContainer("BlogSettings", "Blog Settings", 5)]
    [FieldSetContainer("CommentSettings", "Comment Settings", 105, ContainerName = "BlogSettings")]
    public class BlogPostContainer : BlogBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlogPostContainer"/> class.
        /// </summary>
        public BlogPostContainer()
        {
            PostsPerPage = 10;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable comments].
        /// </summary>
        /// <value>
        ///   <c>True</c> if [enable comments]; otherwise, <c>false</c>.
        /// </value>
        [EditableCheckBox("", 105,
            CheckBoxText = "Enable Comments",
            ContainerName = "CommentSettings",
            DefaultValue = true)]
        public virtual bool EnableComments
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [show comments].
        /// </summary>
        /// <value>
        ///   <c>True</c> if [show comments]; otherwise, <c>false</c>.
        /// </value>
        [EditableCheckBox("", 106,
            CheckBoxText = "Show Comments",
            ContainerName = "CommentSettings",
            DefaultValue = true)]
        public virtual bool ShowComments
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the posts per page.
        /// </summary>
        /// <value>
        /// The posts per page.
        /// </value>
        [EditableTextBox("Posts Per Page", 110, 3,
            Columns = 5,
            ContainerName = "BlogSettings",
            Required = true,
            RequiredMessage = "The number of posts per page is required.",
            Validate = true,
            ValidationExpression = @"^[1-9]+[0-9]*$",
            ValidationMessage = "The posts per page must be a number greater than zero.",
            ValidationText = "*")]
        public virtual int PostsPerPage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the blog posts.
        /// </summary>
        public IList<BlogPost> BlogPosts
        {
            get { return GetChildren(new TypeFilter(typeof(BlogPost))).OfType<BlogPost>().ToList(); }
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text value.
        /// </value>
        /// <remarks>
        /// Override Text because I don't want to show it
        /// Remove this if you want a text input to show up in edit mode
        /// </remarks>
        public override string Text
        {
            get
            {
                return base.Text;
            }

            set
            {
                base.Text = value;
            }
        }
    }
}