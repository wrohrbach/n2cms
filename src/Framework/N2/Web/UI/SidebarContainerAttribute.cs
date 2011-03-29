using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using N2.Definitions;
using System.Web.UI;
using System.Web.UI.WebControls;
using N2.Web.UI.WebControls;

namespace N2.Web.UI
{
	public class SidebarContainerAttribute : EditorContainerAttribute
	{
		public SidebarContainerAttribute(string name, int sortOrder)
			: base(name, sortOrder)
		{
		}

		public string HeadingText { get; set; }

		public override void AddTo(ContainableContext context)
		{
			var accessor = ItemUtility.FindInParents<Edit.IPlaceHolderAccessor>(context.Container);
			if (accessor == null)
				return;
			
			Box box = new Box();
			box.ID = Name;
			box.HeadingText = HeadingText;
			var placeholder = accessor.GetPlaceHolder("Sidebar");
			if (placeholder == null)
				return;

			context.Add(box);
		}
	}
}
