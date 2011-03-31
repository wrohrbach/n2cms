using System;
using System.Reflection;
using System.Web.UI;
using N2.Web.UI;
using N2.Web.UI.WebControls;
using N2.Edit.Workflow;
using N2.Definitions;
using N2.Edit.Web;

namespace N2.Details
{
	/// <summary>
	/// Defines a deletedChild item editor. Renders a drop down list where you can 
	/// select what item to add and edit forms of added items.
	/// </summary>
	public class EditableChildrenAttribute : AbstractEditableAttribute
	{
		private string zoneName;

		public EditableChildrenAttribute()
			: base(null, 200)
		{
		}

		public EditableChildrenAttribute(string title, string zoneName, int sortOrder)
			: this(title, zoneName, zoneName, sortOrder)
		{
		}

		public EditableChildrenAttribute(string title, string zoneName, string name, int sortOrder)
			: base(title, name, sortOrder)
		{
			this.zoneName = zoneName;
		}

		public string ZoneName
		{
			get { return zoneName; }
			set { zoneName = value; }
		}

		public override void UpdateItem(ContainableContext context)
		{
			bool updated = false;
			ItemEditorList listEditor = (ItemEditorList)context.Control;
			for (int i = 0; i < listEditor.ItemEditors.Count; i++)
			{
				if (listEditor.DeletedIndexes.Contains(i))
				{
					var deletedChild = listEditor.ItemEditors[i].CurrentItem;
					if (deletedChild.ID == 0)
						deletedChild.AddTo(null);
					else
						Engine.Persister.Delete(deletedChild);
				}
				else
				{
					ItemEditor childEditor = listEditor.ItemEditors[i];
					ItemEditor parentEditor = ItemUtility.FindInParents<ItemEditor>(context.Control.Parent);
					if (parentEditor != null)
					{
						var subContext = parentEditor.BinderContext.CreateNestedContext(childEditor, childEditor.CurrentItem, childEditor.GetDefinition());
						if (subContext.Binder.UpdateObject(subContext))
							updated = true;
					}
					else
					{
						IItemEditor fallbackEditor = ItemUtility.FindInParents<IItemEditor>(context.Control.Parent);
						if (fallbackEditor != null)
						{
							fallbackEditor.Saved += delegate 
							{
								var cc = childEditor.CreateCommandContext();
								Engine.Resolve<CommandDispatcher>().Publish(cc);
							};
						}
					}
				}
			}
			context.WasUpdated = updated || listEditor.DeletedIndexes.Count > 0 || listEditor.AddedTypes.Count > 0;
		}

		public override void UpdateEditor(ContainableContext context)
		{
			ItemEditorList listEditor = (ItemEditorList)context.Control;
			listEditor.ParentItem = (ContentItem)context.Content;
			listEditor.ZoneName = ZoneName;

            // filtering of children by property generic type
            PropertyInfo info = context.Content.GetType().GetProperty(Name);
            if (info != null)
            {
                foreach (Type argument in info.PropertyType.GetGenericArguments())
                {
                    if (typeof(ContentItem).IsAssignableFrom(argument) || argument.IsInterface)
                    {
                        listEditor.MinimumType = argument;
                        break;
                    }
                }
            }
		}

		public override void AddTo(ContainableContext context)
		{
			Control panel = AddPanel(context.Container);
			context.Control = AddEditor(panel);
		}

		protected override Control AddEditor(Control container)
		{
			ItemEditorList listEditor = new ItemEditorList();
			listEditor.ID = Name;
			listEditor.ParentItem = ItemUtility.FindInParents<IItemEditor>(container).CurrentItem;
			container.Controls.Add(listEditor);
			return listEditor;
		}
	}
}