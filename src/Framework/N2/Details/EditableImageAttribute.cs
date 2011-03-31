using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using N2.Web.UI.WebControls;
using System.Web.UI.WebControls;
using N2.Definitions;

namespace N2.Details
{
	/// <summary>
	/// Defines an editable image this allows to select an image with the file picker.
	/// </summary>
	/// <example>
	///		public class MyItem : N2.ContentItem
	///		{
	/// 		[N2.Details.EditableImage("Image", 100)]
	/// 		public virtual string ImageUrl
	/// 		{
	/// 			get { return (string)(GetDetail("ImageUrl")); }
	/// 			set { SetDetail("ImageUrl", value); }
	/// 		}
	///		}
	/// </example>
	public class EditableImageAttribute : AbstractEditableAttribute, IDisplayable, IRelativityTransformer, IWritingDisplayable
	{
		private string alt = string.Empty;
		private string cssClass = string.Empty;



		public EditableImageAttribute()
			: this(null, 40)
		{
		}

		public EditableImageAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{
		}



		/// <summary>Image alt text.</summary>
		public string Alt
		{
			get { return alt; }
			set { alt = value; }
		}

		/// <summary>CSS class on the image element.</summary>
		public string CssClass
		{
			get { return cssClass; }
			set { cssClass = value; }
		}

		/// <summary>The image size to display by default if available.</summary>
		public string PreferredSize { get; set; }



		protected override Control AddEditor(Control container)
		{
			FileSelector selector = new FileSelector();
			selector.ID = Name;
			container.Controls.Add(selector);
			return selector;
		}
		
		public override void UpdateEditor(ContainableContext context)
		{
			FileSelector selector = (FileSelector)context.Control;
			selector.Url = context.GetValue<string>();
		}

		public override void UpdateItem(ContainableContext context)
		{
			FileSelector selector = (FileSelector)context.Control;
			context.SetValue(selector.Url);
		}

		#region IDisplayable Members
		public override Control AddTo(ContentItem item, string detailName, Control container)
		{
			return DisplayableImageAttribute.AddImage(container, item, detailName, PreferredSize, CssClass, Alt);
		}

		[Obsolete("Use DisplayableImageAttribute.AddImage")]
		public static Control AddImage(Control container, ContentItem item, string detailName, string cssClass, string altText)
		{
			return DisplayableImageAttribute.AddImage(container, item, detailName, null, cssClass, altText);
		}
		#endregion

		#region IRelativityTransformer Members

		public RelativityMode RelativeWhen { get; set; }

		string IRelativityTransformer.Rebase(string currentPath, string fromAppPath, string toAppPath)
		{
			return N2.Web.Url.Rebase(currentPath, fromAppPath, toAppPath);
		}

		#endregion

		#region IWritingDisplayable Members

		public void Write(ContentItem item, string propertyName, System.IO.TextWriter writer)
		{
			DisplayableImageAttribute.WriteImage(item, propertyName, PreferredSize, alt, CssClass, writer);
		}

		#endregion
	}
}
