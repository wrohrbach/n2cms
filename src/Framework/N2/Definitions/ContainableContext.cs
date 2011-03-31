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

		public ContainableContext(string propertyName, object content)
		{
			PropertyName = propertyName;
			Content = content;
			this.Binder = new DefaultBinder();
		}

		public string PropertyName { get; set; }
		public object Content { get; set; }
		public IBinder Binder { get; set; }

		public bool WasUpdated { get; set; }

		public Control Control { get; set; }
		public Control Container { get; set; }

		public void Add(Control editor)
		{
			Container.Controls.Add(editor);
			Control = editor;
		}

		public static ContainableContext WithContainer(string propertyName, object model, Control container)
		{
			return new ContainableContext(propertyName, model) { Container = container };
		}

		public static ContainableContext WithControl(string propertyName, object model, Control control)
		{
			return new ContainableContext(propertyName, model) { Control = control };
		}

		public T GetValue<T>()
		{
			return GetValue<T>(PropertyName);
		}

		public T GetValue<T>(string propertyName)
		{
			return Binder.Get<T>(Content, PropertyName);
		}

		public void SetValue<T>(T value)
		{
			SetValue<T>(PropertyName, value);
		}

		public void SetValue<T>(string propertyName, T value)
		{
			object existing = Binder.Get(Content, propertyName);
			if (existing == null && value == null)
				return;

			if (value != null && existing != null)
			{
				if (value.GetType() != existing.GetType())
				{
					object convertedValue = Utility.Convert(value, existing.GetType());
					if (existing.Equals(convertedValue))
						return;
				}
				else if (existing.Equals(value))
					return;
			}

			Binder.Set(Content, PropertyName, value);
			WasUpdated |= true;
		}
	}
}
