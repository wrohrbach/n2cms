using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using N2.Definitions;

namespace N2.Details
{
	/// <summary>
	/// An abstract base class that implements editable list functionality.
	/// Override and implement GetListItems to use.
	/// Implement a CreateEditor() method to instantiate a desired editor control.
	/// </summary>
	public abstract class EditableListControlAttribute : AbstractEditableAttribute, IDisplayable, IWritingDisplayable
	{
		public EditableListControlAttribute(): base() { }

		public EditableListControlAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{
		}

		public override void UpdateItem(ContainableContext context)
		{
			ListControl ddl = context.Control as ListControl;
			object value = GetValue(ddl);
			context.SetValue(value);
		}

        /// <summary>Gets the object to store as content from the drop down list editor.</summary>
        /// <param name="ddl">The editor.</param>
        /// <returns>The value to store.</returns>
        protected virtual object GetValue(ListControl ddl)
        {
            return ddl.SelectedValue;
        }

		public override void UpdateEditor(ContainableContext context)
		{
			ListControl ddl = context.Control as ListControl;
			ddl.SelectedValue = context.GetValue<string>();
        }

        /// <summary>Gets a string value from the drop down list editor from the content item.</summary>
        /// <param name="item">The item containing the value.</param>
        /// <returns>A string to use as selected value.</returns>
        protected virtual string GetValue(ContentItem item)
        {
            return (item[Name] ?? DefaultValue) as string;
        }
		
		protected abstract ListControl CreateEditor();
		 
		protected override Control AddEditor(Control container)
		{
			ListControl ddl = this.CreateEditor();
            ddl.ID = Name;
			if (!Required)
				ddl.Items.Add(new ListItem());

			ddl.Items.AddRange(GetListItems());
			container.Controls.Add(ddl);
			return ddl;
		}

		protected abstract ListItem[] GetListItems();

		#region IDisplayable Members

		Control IDisplayable.AddTo(ContentItem item, string detailName, Control container)
		{
			string value = item[Name] as string;
			if (!string.IsNullOrEmpty(value))
			{
				foreach (ListItem li in GetListItems())
				{
					if (li.Value == value)
					{
						Literal l = new Literal();
						l.Text = li.Text;
						container.Controls.Add(l);
						return l;
					}
				}
			}
			return null;
		}

		#endregion

		#region IWritingDisplayable Members

		public void Write(ContentItem item, string propertyName, System.IO.TextWriter writer)
		{
			var selected = item[propertyName] as string;
			if (selected != null)
				writer.Write(GetListItems().Where(li => li.Value == selected).Select(li => li.Text).FirstOrDefault());
		}

		#endregion
	}
}
