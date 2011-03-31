using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using N2.Definitions;
using N2.Edit;
using N2.Web;

namespace N2.Details
{
    /// <summary>Attribute used to mark properties as editable. This is used to associate the control used for the editing with the property/detail on the content item whose value we are editing.</summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class EditableUserControlAttribute : AbstractEditableAttribute
    {
		#region Constructors
		/// <summary>Don't forget to set the UserControlpath if you use this constructor.</summary>
		public EditableUserControlAttribute()
		{
		}

		/// <summary>Initializes a new instance of the EditableAttribute class set to use a user control.</summary>
		/// <param name="title">The label displayed to editors</param>
		/// <param name="userControlPath">The virtual path of a user control used for editing</param>
		/// <param name="editorPropertyName">The property on the edit control that will update the unit's property</param>
		/// <param name="sortOrder">The order of this editor</param>
		public EditableUserControlAttribute(string title, string userControlPath, string editorPropertyName, int sortOrder)
			: base(title, sortOrder)
		{
			this.UserControlPath = userControlPath;
			this.UserControlPropertyName = editorPropertyName;
		}
		/// <summary>Initializes a new instance of the EditableAttribute class set to use a user control.</summary>
		/// <param name="title">The label displayed to editors</param>
		/// <param name="userControlPath">The virtual path of a user control used for editing</param>
		/// <param name="sortOrder">The order of this editor</param>
		public EditableUserControlAttribute(string title, string userControlPath, int sortOrder)
			: base(title, sortOrder)
		{
			this.UserControlPath = userControlPath;
			this.UserControlPropertyName = null;
		}
		/// <summary>Initializes a new instance of the EditableAttribute class set to use a user control.</summary>
		/// <param name="userControlPath">The virtual path of a user control used for editing</param>
		/// <param name="sortOrder">The order of this editor</param>
		public EditableUserControlAttribute(string userControlPath, int sortOrder)
			: base("", sortOrder)
		{
			this.UserControlPath = userControlPath;
			this.UserControlPropertyName = null;
		}
		#endregion

		#region Properties
		/// <summary>Gets or sets the virtual path of a user control. This property is only considered when ControlType is <see cref="System.Web.UI.UserControl"/>.</summary>
		public string UserControlPath { get; set; }
		
		/// <summary>The name of the property on the user control to get/set the value from/to.</summary>
		public string UserControlPropertyName { get; set; }
		#endregion


		public override void UpdateItem(ContainableContext context)
		{
			if (context.Control is IContentForm)
			{
				IContentForm binder = context.Control as IContentForm;
				context.WasUpdated = binder.UpdateObject((ContentItem)context.Content);
			}
			else
			{
				var updated = Utility.GetProperty(context.Control, UserControlPropertyName);
				context.SetValue<object>(updated);
			}
		}

		public override void UpdateEditor(ContainableContext context)
		{
			if (context.Control is IContentForm)
			{
				IContentForm binder = context.Control as IContentForm;
				binder.UpdateInterface((ContentItem)context.Content);
			}
			else
			{
				Utility.SetProperty(context.Control, UserControlPropertyName, context.GetValue<object>());
			}
		}

		public override void AddTo(ContainableContext context)
		{
			if (string.IsNullOrEmpty(Title))
			{
				context.Control = AddEditor(context.Container);
				AddHelp(context.Container);
				return;
			}

			base.AddTo(context);
		}

		protected override Control AddEditor(Control container)
		{
			Control c = container.Page.LoadControl(Url.ResolveTokens(this.UserControlPath));
			container.Controls.Add(c);
			return c;
		}
	}
}
