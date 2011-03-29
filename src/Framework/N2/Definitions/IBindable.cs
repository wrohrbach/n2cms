using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace N2.Definitions
{
	/// <summary>
	/// Represents an object that can be bound from/to values by key.
	/// </summary>
	public interface IBindable
	{
		/// <summary>Gets or sets a value by key.</summary>
		/// <param name="key">The key whose value to get or set.</param>
		/// <returns>The value or null if no value is available.</returns>
		object this[string key] { get; set; }
	}
}
