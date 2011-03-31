using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using N2.Details;
using N2.Definitions;

namespace N2.Templates.Details
{
	public abstract class DropDownAttribute : AbstractEditableAttribute
	{
		public DropDownAttribute(string title, string name, int sortOrder)
			:base(title, name, sortOrder)
		{
		}

		public override void UpdateItem(ContainableContext context)
		{
			DropDownList ddl = (DropDownList)context.Control;
			context.SetValue(ddl.SelectedValue);
		}

		public override void UpdateEditor(ContainableContext context)
		{
			DropDownList ddl = (DropDownList)context.Control;
			ddl.SelectedValue = context.GetValue<string>();
		}

		protected override Control AddEditor(Control container)
		{
			DropDownList ddl = new DropDownList();
			foreach (ListItem li in GetListItems(container))
			{
				ddl.Items.Add(li);
			}
			container.Controls.Add(ddl);
			return ddl;
		}

		protected abstract IEnumerable<ListItem> GetListItems(Control container);
	}
}
