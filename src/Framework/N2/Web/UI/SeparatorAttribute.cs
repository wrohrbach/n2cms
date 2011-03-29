using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using N2.Definitions;
using N2.Web.UI.WebControls;

namespace N2.Web.UI
{
	/// <summary>
	/// Creates a separator that can be ordered between two editors
	/// </summary>
	public class SeparatorAttribute : EditorContainerAttribute
	{
		public SeparatorAttribute(string name, int sortOrder)
			: base(name, sortOrder)
		{
		}

		public override void AddTo(ContainableContext context)
		{
			var hr = new Hr();
			context.Add(hr);
		}
	}
}
