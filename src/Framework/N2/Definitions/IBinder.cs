using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace N2.Definitions
{
	/// <summary>
	/// Gets and sets properties or details on classes.
	/// </summary>
	public interface IBinder
	{
		/// <summary>Gets a property or detial from an object instance.</summary>
		/// <param name="instance">The instance whose property to get.</param>
		/// <param name="propertyName">The name of the proeprty to be retrieved.</param>
		/// <returns>The property value or null if the property doesn't exist or has no value.</returns>
		object Get(object instance, string propertyName);

		/// <summary>Gets a property or detial from an object instance.</summary>
		/// <typeparam name="T">The type of value to retrieve.</typeparam>
		/// <param name="instance">The instance whose property to get.</param>
		/// <param name="propertyName">The name of the proeprty to be retrieved.</param>
		/// <returns>The property value or null if the property doesn't exist or has no value.</returns>
		/// <exception cref="InvalidCastException">The property type doesn't match the type parameter.</exception>
		T Get<T>(object instance, string propertyName);

		/// <summary>Sets a property or detail on an object instance.</summary>
		/// <param name="instance">The instance whose property to set.</param>
		/// <param name="propertyName">The name of the property to set.</param>
		/// <param name="value">The value to set the property to.</param>
		void Set(object instance, string propertyName, object value);
	}
}
