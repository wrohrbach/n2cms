using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace N2.Definitions
{
	public class ContainableContext
	{
		public ContainableContext()
		{
		}

		public ContainableContext(string propertyName, IBindable content)
		{
			PropertyName = propertyName;
			Content = content;
		}

		public string PropertyName { get; set; }
		public IBindable Content { get; set; }
		public bool WasUpdated { get; set; }

		public Control Control { get; set; }
		public Control Container { get; set; }

		public void Add(Control editor)
		{
			Container.Controls.Add(editor);
			Control = editor;
		}

		public static ContainableContext WithContainer(string propertyName, IBindable model, Control container)
		{
			return new ContainableContext(propertyName, model) { Container = container };
		}

		public static ContainableContext WithControl(string propertyName, IBindable model, Control control)
		{
			return new ContainableContext(propertyName, model) { Control = control };
		}
	}
}
