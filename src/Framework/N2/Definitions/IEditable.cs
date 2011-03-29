using System;
using System.Web.UI;

namespace N2.Definitions
{
    /// <summary>
    /// Attributes implementing this interface defines editors of an object. 
    /// The interface defines methods to add editor controls to a web form 
	/// and updating objects with their values.
    /// </summary>
	public interface IEditable : IContainable
    {
		/// <summary>Gets or sets the label used for presentation.</summary>
		string Title { get; set;}

		/// <summary>Updates the object with the values from the editor.</summary>
		/// <param name="context">The object to update and the editor contorl whose values to update the object with.</param>
		/// <remarks>The context's WasUpdated property must be set to true if the item was changed (and needs to be saved).</remarks>
		void UpdateItem(ContainableContext context);

		/// <summary>Updates the editor with the values from the object.</summary>
		/// <param name="item">The object that contains values to assign to the editor and the editor to load with a value.</param>
		void UpdateEditor(ContainableContext context);
    }
}
