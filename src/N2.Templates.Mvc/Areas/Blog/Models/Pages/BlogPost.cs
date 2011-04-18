using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

using N2.Collections;
using N2.Details;
using N2.Edit.FileSystem;
using N2.Integrity;
using N2.Templates.Mvc.Models.Pages;
using N2.Templates.Mvc.Models.Parts;
using N2.Templates.Mvc.Services;
using N2.Web.Drawing;
using N2.Web.Mvc;
using N2.Web.UI;
using N2.Persistence.Serialization;
using N2.Definitions;

namespace N2.Templates.Mvc.Areas.Blog.Models.Pages
{
    /// <summary>
    /// A Blog Posting
    /// </summary>
    [PageDefinition("Blog Post", Description = "A blog posting.", SortOrder = 155,
        IconUrl = "~/N2/Resources/icons/script_edit.png")]
    [RestrictParents(typeof(BlogPostContainer))]
    [TabContainer("PostSettings", "Post Settings", 5)]
    [FieldSetContainer("CommentSettings", "Comment Settings", 105, ContainerName = "PostSettings")]
    public class BlogPost : BlogBase, ISyndicatable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlogPost"/> class.
        /// </summary>
        public BlogPost()
        {
            Visible = false;
            Tags = "uncategorized";
        }

        #region Properties
        /// <summary>
        /// Gets or sets the image URL.
        /// </summary>
        /// <value>
        /// The image URL.
        /// </value>
        [EditableFileUpload("Title Image", 35, ContainerName = Tabs.Content)]
        public virtual string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the image caption.
        /// </summary>
        /// <value>
        /// The image caption.
        /// </value>
        [EditableTextBox("Image Caption", 36, ContainerName = Tabs.Content)]
        public virtual string ImageCaption { get; set; }

        /// <summary>
        /// Gets or sets the author.
        /// </summary>
        /// <value>
        /// The author.
        /// </value>
        [EditableTextBox("Author <br><span style=\"font-size: 10px; font-style: italic; color: #666666;\">(Defaults to current user)</span>", 90, ContainerName = Tabs.Content)]
        public virtual string Author
        {
            get { return (string)(GetDetail("Author") ?? SavedBy); }
            set { SetDetail("Author", value, string.Empty); }
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text value.
        /// </value>
        [EditableFreeTextArea("Text", 100, ContainerName = Tabs.Content)]
        public override string Text { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>
        /// The tag values.
        /// </value>
        [EditableTextBox("Tags <br><span style=\"font-size: 10px; font-style: italic; color: #666666;\">(Comma Delimited)</span>", 110,
            ContainerName = Tabs.Content)]
        public virtual string Tags
        {
            get 
            { 
                return (string)(GetDetail("Tags") ?? string.Empty); 
            }

            set
            {
                string val = !string.IsNullOrEmpty(value.Trim()) ? value : "uncategorized";
                SetDetail("Tags", val.ToLower());
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable comments].
        /// </summary>
        /// <value>
        ///   <c>True</c> if [enable comments]; otherwise, <c>false</c>.
        /// </value>
        [EditableCheckBox("", 10,
            CheckBoxText = "Enable Comments",
            ContainerName = "CommentSettings",
            DefaultValue = true)]
        public virtual bool EnableComments { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [show comments].
        /// </summary>
        /// <value>
        ///   <c>True</c> if [show comments]; otherwise, <c>false</c>.
        /// </value>
        [EditableCheckBox("", 15,
            CheckBoxText = "Show Comments",
            ContainerName = "CommentSettings",
            DefaultValue = true)]
        public virtual bool ShowComments { get; set; }

        /// <summary>
        /// Gets or sets the introduction.
        /// </summary>
        /// <value>
        /// The introduction.
        /// </value>
        [DisplayableLiteral()]
        public virtual string Introduction
        {
            get
            {
                return Text.Split(new string[] { "<!--more-->" }, 2, System.StringSplitOptions.None)[0];
            }

            set 
            { 
                SetDetail("Introduction", value, string.Empty); 
            }
        }

        /// <summary>
        /// Gets the comments.
        /// </summary>
        public IList<BlogComment> Comments
        {
            get { return GetChildren(new TypeFilter(typeof(BlogComment))).OfType<BlogComment>().ToList(); }
        }
        
        /// <summary>
        /// Gets a content summary.
        /// </summary>
        string ISyndicatable.Summary
        {
            get { return Introduction; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is summarized.
        /// </summary>
        /// <value>
        ///     <c>True</c> if this instance is summarized; otherwise, <c>false</c>.
        /// </value>
        public bool IsSummarized
        {
            get
            {
                return Text.Split(new string[] { "<!--more-->" }, 2, System.StringSplitOptions.None).Count() > 1;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this particular item is to be syndicated.
        /// </summary>
        public bool Syndicate { get; set; } 

        #endregion

        #region Methods
        /// <summary>
        /// Gets the next comment ID.
        /// </summary>
        /// <returns>The next comment ID.</returns>
        public int GetNextCommentID()
        {
            return Comments.Count > 0 ? Comments.Max(c => c.CommentID) + 1 : 1;
        }

        /// <summary>
        /// Gets the resized image URL.
        /// </summary>
        /// <param name="fs">The filesystem.</param>
        /// <returns>The URL to the resized image.</returns>
        public virtual string GetResizedImageUrl(IFileSystem fs)
        {
            return GetReizedUrl(fs, "blog");
        }

        /// <summary>
        /// Gets the reized URL.
        /// </summary>
        /// <param name="fs">The filesystem.</param>
        /// <param name="imageSize">Size of the image.</param>
        /// <returns>The URL to the resized image.</returns>
        private string GetReizedUrl(IFileSystem fs, string imageSize)
        {
            string resizedUrl = ImagesUtility.GetResizedPath(ImageUrl, imageSize);
            if (fs.FileExists(resizedUrl))
            {
                return resizedUrl;
            }

            return ImageUrl;
        } 
        #endregion
    }
}