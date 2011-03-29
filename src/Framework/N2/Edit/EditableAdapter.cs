using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Principal;
using System.Web.UI;
using N2.Edit.FileSystem;
using N2.Engine;
using N2.Web;
using N2.Web.UI.WebControls;
using N2.Definitions;

namespace N2.Edit
{
	/// <summary>
	/// Controls aspects related to the editor interface and editing content items.
	/// </summary>
	[Adapts(typeof(ContentItem))]
	public class EditableAdapter : AbstractContentAdapter
	{
		IDefaultDirectory defaultDirectory;
		IEditManager editManager;

		public IDefaultDirectory DefaultDirectory
		{
			get { return defaultDirectory ?? engine.Resolve<IDefaultDirectory>(); }
			set { defaultDirectory = value; }
		}

		public IEditManager EditManager
		{
			get { return editManager ?? engine.Resolve<IEditManager>(); }
			set { editManager = value; }
		}

		/// <summary>Adds the editors defined for the item to the control hierarchy.</summary>
		/// <param name="itemType">The type to add editors for.</param>
		/// <param name="container">The container onto which to add editors.</param>
		/// <param name="user">The user to filter access by.</param>
		/// <returns>A editor name to control map of added editors.</returns>
		public virtual IEnumerable<ContainableContext> AddDefinedEditors(ItemDefinition definition, ContentItem item, Control container, IPrincipal user)
		{
			return EditManager.AddEditors(definition, item, container, user);
		}

		/// <summary>Updates editors with values from the item.</summary>
		/// <param name="item">The item containing values.</param>
		/// <param name="addedEditors">A map of editors to update (may have been filtered by access).</param>
		/// <param name="user">The user to filter access by.</param>
		[Obsolete("Use overload with IEnumerable<ContainableContext>")]
		public virtual void LoadAddedEditors(ItemDefinition definition, ContentItem item, IDictionary<string, Control> addedEditors, IPrincipal user)
		{
			EditManager.UpdateEditors(definition, item, addedEditors.Select(e => ContainableContext.WithControl(e.Key, item, e.Value)), user);
		}

		/// <summary>Updates editors with values from the item.</summary>
		/// <param name="item">The item containing values.</param>
		/// <param name="addedEditors">A map of editors to update (may have been filtered by access).</param>
		/// <param name="user">The user to filter access by.</param>
		public virtual void LoadAddedEditors(ItemDefinition definition, ContentItem item, IEnumerable<ContainableContext> addedEditors, IPrincipal user)
		{
			EditManager.UpdateEditors(definition, item, addedEditors, user);
		}

		/// <summary>Updates an item with the values from the editor controls without saving it.</summary>
		/// <param name="item">The item to update.</param>
		/// <param name="addedEditors">Editors containing interesting values.</param>
		/// <param name="user">The user to filter access by.</param>
		/// <returns>Detail names that were updated.</returns>
		[Obsolete("Use overload with IEnumerable<ContainableContext>")]
		public virtual string[] UpdateItem(ItemDefinition definition, ContentItem item, IDictionary<string, Control> addedEditors, IPrincipal user)
		{
			return EditManager.UpdateItem(definition, item, addedEditors.Select(e => ContainableContext.WithControl(e.Key, item, e.Value)), user);
		}

		/// <summary>Updates an item with the values from the editor controls without saving it.</summary>
		/// <param name="item">The item to update.</param>
		/// <param name="addedEditors">Editors containing interesting values.</param>
		/// <param name="user">The user to filter access by.</param>
		/// <returns>Detail names that were updated.</returns>
		public virtual string[] UpdateItem(ItemDefinition definition, ContentItem item, IEnumerable<ContainableContext> addedEditors, IPrincipal user)
		{
			return EditManager.UpdateItem(definition, item, addedEditors, user);
		}

		/// <summary>Saves an item using values from the supplied item editor.</summary>
		/// <param name="item">The item to update.</param>
		/// <param name="addedEditors">The editors to update the item with.</param>
		/// <param name="versioningMode">How to treat the item beeing saved in respect to versioning.</param>
		/// <param name="user">The user that is performing the saving.</param>
		/// <returns>The item to continue using.</returns>
		[Obsolete("Use CommandFactory?", false)]
		public virtual ContentItem SaveItem(ContentItem item, IEnumerable<ContainableContext> addedEditors, ItemEditorVersioningMode versioningMode, IPrincipal user)
		{
			return EditManager.Save(item, addedEditors, versioningMode, user);
		}

		/// <summary>Gets the default folder for an item.</summary>
		/// <param name="item">The item whose default folder is requested.</param>
		/// <returns>The default folder.</returns>
		public virtual string GetDefaultFolder(ContentItem item)
		{
			return DefaultDirectory.GetDefaultDirectory(item);
		}
	}
}
