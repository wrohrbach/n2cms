using System;
using System.Collections.Generic;
using System.Text;
using N2.Details;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using N2.Definitions;

namespace N2.Security.Details
{
	public class EditableRolesAttribute : AbstractEditableAttribute
	{
		public override void UpdateItem(ContainableContext context)
		{
			CheckBoxList cbl = context.Control as CheckBoxList;
			List<string> roles = new List<string>();
			foreach (ListItem li in cbl.Items)
				if (li.Selected)
					roles.Add(li.Value);

			DetailCollection dc = ((ContentItem)context.Content).GetDetailCollection(Name, true);
			dc.Replace(roles);

			context.WasUpdated = true;
		}

		public override void UpdateEditor(ContainableContext context)
		{
			CheckBoxList cbl = context.Control as CheckBoxList;
			DetailCollection dc = ((ContentItem)context.Content).GetDetailCollection(Name, false);
			if (dc != null)
			{
				foreach (string role in dc)
				{
					ListItem li = cbl.Items.FindByValue(role);
					if (li != null)
					{
						li.Selected = true;
					}
					else
					{
						li = new ListItem(role);
						li.Selected = true;
						li.Attributes["style"] = "color:silver";
						cbl.Items.Add(li);
					}
				}
			}
		}

		protected override Control AddEditor(Control container)
		{
			CheckBoxList cbl = new CheckBoxList();
			foreach (string role in Roles.GetAllRoles())
			{
				cbl.Items.Add(role);
			}
			container.Controls.Add(cbl);
			return cbl;
		}
	}

}
