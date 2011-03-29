using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using N2.Web.UI;
using N2.Web.UI.WebControls;
using N2.Definitions;
using N2.Edit.Workflow;
using N2.Edit.Web;

namespace N2.Edit
{
	/// <summary>
	/// Used internally to add the save changes during on page editing.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class ControlPanelEditingSaveAttribute : ControlPanelLinkAttribute
	{
		public ControlPanelEditingSaveAttribute(string toolTip, int sortOrder)
            : base("cpEditingSave", "{ManagementUrl}/Resources/icons/disk.png", null, toolTip, sortOrder, ControlPanelState.Editing)
		{
		}

		public override Control AddTo(Control container, PluginContext context)
		{
			if(!ActiveFor(container, context.State))
				return null;

			LinkButton btn = new LinkButton();
			btn.Text = GetInnerHtml(context, IconUrl, ToolTip, Title);
			btn.ToolTip = Utility.GetResourceString(GlobalResourceClassName, Name + ".ToolTip") ?? context.Format(ToolTip, false);
			btn.CssClass = "save";
			container.Controls.Add(btn);
			btn.Command += delegate
				{
					IList<IItemEditor> itemEditors = GetEditedItems(container.Page);

					foreach (IItemEditor itemEditor in itemEditors)
					{
						var definition = Engine.Definitions.GetDefinition(itemEditor.CurrentItem);
						Engine.Resolve<CommandDispatcher>().Publish(
							new CommandContext(
								definition,
								itemEditor.CurrentItem,
								Interfaces.Viewing,
								container.Page.User,
								new EditorCollectionBinder(definition, itemEditor.Editors),
								new NullValidator<CommandContext>()));
								
						//Context.Current.EditManager.Save(itemEditor.CurrentItem, itemEditor.Editors, itemEditor.VersioningMode, container.Page.User);
					}

					RedirectTo(container.Page, context.Selected);
				};
			return btn;
		}

		protected virtual IList<IItemEditor> GetEditedItems(Page page)
		{
			Dictionary<ContentItem, IList<ContainableContext>> itemsEditors = new Dictionary<ContentItem, IList<ContainableContext>>();

			IEnumerable<IEditableEditor> editors = ItemUtility.FindInChildren<IEditableEditor>(page);
			foreach (EditableDisplay ed in editors)
			{
				if (!itemsEditors.ContainsKey(ed.CurrentItem))
				{
					itemsEditors[ed.CurrentItem] = new List<ContainableContext>();
				}
				itemsEditors[ed.CurrentItem].Add(new ContainableContext(ed.PropertyName, ed.CurrentItem) { Control = ed.Editor, Container = ed });
			}

			IList<IItemEditor> items = new List<IItemEditor>();
			foreach (ContentItem item in itemsEditors.Keys)
			{
				items.Add(new OnPageItemEditor(ItemEditorVersioningMode.VersionAndSave, item.ZoneName, itemsEditors[item].ToArray(), item));
			}
			return items;
		}

		protected void RedirectTo(Page page, ContentItem item)
		{
			string url = page.Request["returnUrl"];
			if (string.IsNullOrEmpty(url))
				url = Engine.GetContentAdapter<NodeAdapter>(item).GetPreviewUrl(item);

			page.Response.Redirect(url);
		}


		#region class OnPageItemEditor

		private class OnPageItemEditor : IItemEditor
		{
			public OnPageItemEditor(ItemEditorVersioningMode versioningMode, string zoneName,
			                        ContainableContext[] addedEditors, ContentItem currentItem)
			{
				this.versioningMode = versioningMode;
				this.zoneName = zoneName;
				this.addedEditors = addedEditors;
				this.currentItem = currentItem;
			}

			#region IItemEditor Members

			private ItemEditorVersioningMode versioningMode = ItemEditorVersioningMode.VersionAndSave;
			private string zoneName = string.Empty;
			private readonly ContainableContext[] addedEditors;
			private ContentItem currentItem;

			public ItemEditorVersioningMode VersioningMode
			{
				get { return versioningMode; }
				set { versioningMode = value; }
			}

			public string ZoneName
			{
				get { return zoneName; }
				set { zoneName = value; }
			}

			public ContainableContext[] Editors
			{
				get { return addedEditors; }
			}

			#endregion

			#region IItemContainer Members

			public ContentItem CurrentItem
			{
				get { return currentItem; }
				set { currentItem = value; }
			}

			#endregion


			#region IItemEditor Members

			public event EventHandler<ItemEventArgs> Saved = delegate {};

			#endregion
		}

		#endregion
	}
}