using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using N2.Definitions;
using N2.Web.UI.WebControls;

namespace N2.Web.UI
{
	/// <summary>
	/// Defines a horizontal ruler on the edit interface.
	/// </summary>
	public class DividerAttribute : EditorContainerAttribute
	{
		public DividerAttribute(string name, int sortOrder)
			:base(name, sortOrder)
		{
		}

		public override void AddTo(ContainableContext context)
		{
			Hr hr = new Hr();
			context.Add(hr);
		}
	}
}
