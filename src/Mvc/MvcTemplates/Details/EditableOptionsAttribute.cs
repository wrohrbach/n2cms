using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using N2.Details;
using N2.Templates.Mvc.Models.Parts;
using N2.Definitions;

namespace N2.Templates.Details
{
    public class EditableOptionsAttribute : AbstractEditableAttribute
    {
        public override void UpdateItem(ContainableContext context)
        {
            TextBox tb = (TextBox)context.Control;
            string[] rows = tb.Text.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			var item = (ContentItem)context.Content;
            for (int i = item.Children.Count-1; i >= 0; --i)
            {
                int index = Array.FindIndex(rows, delegate(string row)
                                                      {
                                                          return row == item.Children[i].Title;
                                                      }
                    );
                if (index < 0)
                    Context.Persister.Delete(item.Children[i]);
            }
            for (int i = 0; i < rows.Length; i++)
            {
                ContentItem child = FindChild(item, rows[i]);
                if (child == null)
                {
                    child = new Option();
                    child.Title = rows[i];
                    child.AddTo(item);
                }
                child.SortOrder = i;
            }

			context.WasUpdated = true;
        }

        private static ContentItem FindChild(ContentItem item, string row)
        {
            foreach (ContentItem child in item.Children)
            {
                if (child.Title == row)
                    return child;
            }
            return null;
        }

        public override void UpdateEditor(ContainableContext context)
        {
			TextBox tb = (TextBox)context.Control;
            tb.Text = string.Empty;
			var item = (ContentItem)context.Content;
			foreach (ContentItem child in item.GetChildren())
            {
                tb.Text += child.Title + Environment.NewLine;
            }
        }

        protected override Control AddEditor(Control container)
        {
            TextBox tb = new TextBox();
            tb.ID = Name;
            tb.TextMode = TextBoxMode.MultiLine;
            tb.Rows = 5;
            tb.Columns = 40;
            container.Controls.Add(tb);

            return tb;
        }
    }
}