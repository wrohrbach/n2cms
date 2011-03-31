using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using N2.Web;
using N2.Web.UI.WebControls;
using N2.Definitions;

namespace N2.Details
{
	/// <summary>
	/// Defines an editable link to another item on this site. The item is 
	/// selected through a popup window displaying the item tree.
	/// </summary>
	/// <example>
	///		[EditableLink("Feed root", 90)]
	///		public virtual ContentItem FeedRoot
	///		{
	/// 		get { return (ContentItem)GetDetail("FeedRoot"); }
	/// 		set { SetDetail("FeedRoot", value); }
	///		}
	/// </example>
	public class EditableLinkAttribute : AbstractEditableAttribute, IRelativityTransformer, IWritingDisplayable, IDisplayable
	{
		public EditableLinkAttribute()
			: this(null, 100)
		{
		}

		public EditableLinkAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{
		}

		protected override Control AddEditor(Control container)
		{
			ItemSelector selector = new ItemSelector();
			selector.ID = Name;
			container.Controls.Add(selector);
			return selector;
		}
		
		public override void UpdateEditor(ContainableContext context)
		{
			ItemSelector selector = (ItemSelector)context.Control;
			selector.SelectedItem = context.GetValue<ContentItem>();
			var pi = context.Content.GetType().GetProperty(Name);
			if(pi != null)
				selector.RequiredType = pi.PropertyType;
		}
		
		public override void UpdateItem(ContainableContext context)
		{
			ItemSelector selector = (ItemSelector)context.Control;
			context.SetValue(selector.SelectedItem);
		}
		
		#region IDisplayable Members
		public override Control AddTo(ContentItem item, string detailName, Control container)
		{
			ContentItem linkedItem = item[detailName] as ContentItem;
			if (linkedItem != null)
			{
				if (linkedItem.IsPage)
					return DisplayableAnchorAttribute.GetLinkBuilder(item, linkedItem, detailName, null, null).AddTo(container);
				else
					return Web.UI.ItemUtility.AddUserControl(container, linkedItem);
			}
			return null;
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

		public void Write(ContentItem item, string detailName, System.IO.TextWriter writer)
		{
			ContentItem linkedItem = item[detailName] as ContentItem;
			if (linkedItem != null && linkedItem.IsPage)
			{
				DisplayableAnchorAttribute.GetLinkBuilder(item, linkedItem, detailName, null, null).WriteTo(writer);
			}
		}

		#endregion
	}
}
