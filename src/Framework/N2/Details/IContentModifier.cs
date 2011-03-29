using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using N2.Edit.Workflow;
using N2.Definitions;

namespace N2.Details
{
	/// <summary>
	/// Applies modifications to an object before it enters a state.
	/// </summary>
	public interface IContentModifier
	{
		/// <summary>Apply modifications before transitioning item to this state.</summary>
		/// <remarks>Only New is currently supported.</remarks>
		ContentState ChangingTo { get; }

		/// <summary>Applies modifications to the given item.</summary>
		/// <param name="item">The item to modify.</param>
		void Modify(IBindable item);
	}
}
