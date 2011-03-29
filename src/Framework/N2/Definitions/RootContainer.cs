using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

namespace N2.Definitions
{
	/// <summary>
	/// Used as root container when retrieving editable attributes and editor
	/// containers.
	/// </summary>
	internal class RootContainer : EditorContainerAttribute
	{
		public RootContainer()
			: base("Root", 0)
		{
		}

		public override void AddTo(ContainableContext context)
		{
			context.Control = context.Container;
		}
	}
}
