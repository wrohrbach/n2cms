using System;
using System.ComponentModel;
using N2.Templates.Mvc.Areas.Blog.Models.Pages;
using N2.Web.Mvc;
using N2.Web.UI;

namespace N2.Templates.Mvc.Areas.Blog.Models
{
    /// <summary>
    /// Model for blog comment inputs.
    /// </summary>
    public class BlogCommentInputModel : IItemContainer<BlogPost>, IDataErrorInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlogCommentInputModel"/> class.
        /// </summary>
        public BlogCommentInputModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogCommentInputModel"/> class.
        /// </summary>
        /// <param name="currentItem">The current item.</param>
        public BlogCommentInputModel(BlogPost currentItem)
        {
            Update(currentItem);
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text value.
        /// </value>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The website URL.
        /// </value>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The commenter's email.
        /// </value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The commenter's name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title of the comment.
        /// </value>
        public string Title { get; set; }

        #region IItemContainer<BlogPost> Members

        /// <summary>
        /// Gets the current item.
        /// </summary>
        public BlogPost CurrentItem { get; private set; }

        /// <summary>
        /// Gets the current item.
        /// </summary>
        ContentItem IItemContainer.CurrentItem
        {
            get { return CurrentItem; }
        }

        #endregion

        #region IDataErrorInfo Members

        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// </summary>
        /// <returns>
        /// An error message indicating what is wrong with this object. The default is an empty string ("").
        /// </returns>
        public string Error
        {
            get { return String.Empty; }
        }

        /// <summary>
        /// Gets the error message for the property with the given name.
        /// </summary>
        /// <param name="columnName">The column name.</param>
        /// <returns>
        /// The error message for the property. The default is an empty string ("").
        /// </returns>
        public string this[string columnName]
        {
            get { return Validate(columnName); }
        }

        #endregion

        /// <summary>
        /// Updates the specified current item.
        /// </summary>
        /// <param name="currentItem">The current item.</param>
        /// <returns>Returns the model</returns>
        public BlogCommentInputModel Update(BlogPost currentItem)
        {
            CurrentItem = currentItem;

            return this;
        }

        /// <summary>
        /// Validates the specified property name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>Error message.</returns>
        private string Validate(string propertyName)
        {
            switch (propertyName.ToLower())
            {
                case "title":
                    if (String.IsNullOrEmpty(Title))
                    {
                        return "Title cannot be empty";
                    }

                    break;
                case "name":
                    if (String.IsNullOrEmpty(Name))
                    {
                        return "Name cannot be empty";
                    }

                    break;
                case "text":
                    if (String.IsNullOrEmpty(Text))
                    {
                        return "Text cannot be empty";
                    }

                    break;
                case "email":
                    if (String.IsNullOrEmpty(Email))
                    {
                        return "Email cannot be empty";
                    }

                    break;
            }

            return String.Empty;
        }
    }
}