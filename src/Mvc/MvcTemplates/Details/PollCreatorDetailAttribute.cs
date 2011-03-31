using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using N2.Templates.Details;
using N2.Templates.Mvc.Models.Parts;
using N2.Templates.Mvc.Models.Parts.Questions;
using N2.Definitions;

namespace N2.Templates.Mvc.Details
{
	public class PollCreatorDetailAttribute : EditableOptionsAttribute
	{
		private string createNewText = "Create new";
		private string questionText;

		public string CreateNewText
		{
			get { return createNewText; }
			set { createNewText = value; }
		}

		public string QuestionText
		{
			get { return questionText; }
			set { questionText = value; }
		}

		public override void UpdateItem(ContainableContext context)
		{
			ContentItem questionItem = context.GetValue<ContentItem>();

			CheckBox cb = context.Control.FindControl(GetCheckBoxName()) as CheckBox;
			var item = (ContentItem)context.Content;
			if (cb.Checked || questionItem == null)
			{
				questionItem = new SingleSelect();
				questionItem.AddTo(item);
				Utility.UpdateSortOrder(item.Children);
			}

			TextBox tb = context.Control.FindControl(GetTextBoxName()) as TextBox;
			questionItem.Title = tb.Text;

			var cc2 = ContainableContext.WithControl(null, questionItem, context.Control);
			base.UpdateItem(cc2);
			context.WasUpdated = cc2.WasUpdated;
		}

		public override void UpdateEditor(ContainableContext context)
		{
			ContentItem questionItem = context.GetValue<ContentItem>() ?? new SingleSelect();

			TextBox tb = context.Control.FindControl(GetTextBoxName()) as TextBox;
			tb.Text = questionItem.Title;

			var cc2 = ContainableContext.WithControl(null, questionItem, context.Control);
			base.UpdateEditor(cc2);
			context.WasUpdated = cc2.WasUpdated;
		}

		public override void AddTo(ContainableContext context)
		{
			Control panel = AddPanel(context.Container);
			
			Label label = new Label();
			label.ID = Name + "-Label";
			label.Text = QuestionText;
			label.CssClass = "editorLabel";
			panel.Controls.Add(label);

			TextBox tb = new TextBox();
			tb.ID = GetTextBoxName();
			panel.Controls.Add(tb);

			CheckBox cb = new CheckBox();
			cb.ID = GetCheckBoxName();
			cb.Text = CreateNewText;
			panel.Controls.Add(cb);

			base.AddTo(context);
		}

		private string GetTextBoxName()
		{
			return Name + "-Title";
		}

		private string GetCheckBoxName()
		{
			return Name + "-CreateNew";
		}
	}
}