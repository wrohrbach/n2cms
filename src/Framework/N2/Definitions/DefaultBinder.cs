using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace N2.Definitions
{
	public class DefaultBinder : IBinder
	{
		#region IBinder Members

		public object Get(object instance, string propertyName)
		{
			return Get<object>(instance, propertyName);
		}

		public T Get<T>(object instance, string propertyName)
		{
			var map = instance as IActiveBindable;
			object value = (map != null)
				? map[propertyName]
				: Utility.GetProperty(instance, propertyName);
			
			if (value == null)
				return default(T);
			if (value is T)
				return (T)value;

			return Utility.Convert<T>(value);
		}

		public void Set(object instance, string propertyName, object value)
		{
			var map = instance as IActiveBindable;
			if (map != null)
				map[propertyName] = value;
			else
				Utility.SetProperty(instance, propertyName, value);
		}

		#endregion
	}
}
