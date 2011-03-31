using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using N2.Definitions;
using System.Web.UI.WebControls;

namespace N2.Details
{
	public class EditableNumberAttribute : EditableTextAttribute
	{
		/// <summary>Initializes a new instance of the EditableTextBoxAttribute class.</summary>
		public EditableNumberAttribute()
			: this(null, 50)
		{
		}

		/// <summary>Initializes a new instance of the EditableTextBoxAttribute class.</summary>
		/// <param name="title">The label displayed to editors</param>
		/// <param name="sortOrder">The order of this editor</param>
		public EditableNumberAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{
			DefaultValue = 0;
			Required = true;
			MaxLength = 11;
			Validate = true;
			ValidationExpression = "[-]?\\d+";
		}

		public override void UpdateItem(ContainableContext context)
		{
			TextBox tb = context.Control as TextBox;
			context.SetValue(int.Parse(tb.Text));
		}

		public override void UpdateEditor(ContainableContext context)
		{
			TextBox tb = context.Control as TextBox;
			tb.Text = context.GetValue<int>().ToString();
		}
	}
}
