using System;
using System.Collections.Generic;
using System.Web.UI;
using N2.Definitions;

namespace N2.Web.UI.WebControls
{
	/// <summary>
	/// Classes implementing this interface can serve as item editors.
	/// </summary>
	public interface IItemEditor : IItemContainer
	{
		/// <summary>Gets or sets versioning mode when saving items.</summary>
		[Obsolete("Now decided by the command factory depending on save or publish command.")]
		ItemEditorVersioningMode VersioningMode { get; set; }
		
		/// <summary>Gets or sets the zone name to use for items saved through this editor.</summary>
		[Obsolete("No longer used.")]
		string ZoneName { get; set; }

		/// <summary>Map of editor names and controls added to this item editor.</summary>
		ContainableContext[] Editors { get; }

		event EventHandler<ItemEventArgs> Saved;
	}
}
