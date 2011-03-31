using System;
using System.Web.UI;
using N2.Web.UI.WebControls;
using N2.Definitions;

namespace N2.Details
{
    /// <summary>
    /// Decorates the content item with a date range editable that will update two date fields.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class WithEditableDateRangeAttribute : AbstractEditableAttribute, IWritingDisplayable, IDisplayable
    {
        private string betweenText = " - ";
        private string nameEndRange;

		public WithEditableDateRangeAttribute()
			: this("Dates", 20, "From", "To")
		{
		}

        public WithEditableDateRangeAttribute(string title, int sortOrder, string name, string nameEndRange)
            : base(title, sortOrder)
        {
            Name = name;
            NameEndRange = nameEndRange;
        }

        /// <summary>End of range detail (property) on the content item's object</summary>
        public string NameEndRange
        {
            get { return nameEndRange; }
            set { nameEndRange = value; }
        }

        /// <summary>Gets or sets a text displayed between the date fields.</summary>
        public string BetweenText
        {
            get { return betweenText; }
            set { betweenText = value; }
        }

        protected override Control AddEditor(Control container)
        {
            DateRange range = new DateRange();
            range.ID = Name + NameEndRange;
            container.Controls.Add(range);
            range.BetweenText = GetLocalizedText("BetweenText") ?? BetweenText;
            return range;
        }

        public override void UpdateEditor(ContainableContext context)
        {
			DateRange range = (DateRange)context.Control;
			range.From = context.GetValue<DateTime?>(Name);
			range.To = context.GetValue<DateTime?>(NameEndRange);
        }

        public override void UpdateItem(ContainableContext context)
        {
			DateRange range = context.Control as DateRange;
			context.SetValue(range.From);
			context.SetValue(NameEndRange, range.To);
        }

		#region IWritingDisplayable Members

		public void Write(ContentItem item, string propertyName, System.IO.TextWriter writer)
		{
			writer.Write(item[propertyName] + BetweenText + item[NameEndRange]);
		}

		#endregion
	}
}
